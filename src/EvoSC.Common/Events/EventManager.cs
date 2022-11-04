using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Util;
using GbxRemoteNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace EvoSC.Common.Events;

public class EventManager : IEventManager
{
    private readonly ILogger<EventManager> _logger;
    private readonly IEvoSCApplication _app;
    private readonly IControllerManager _controllers;
    
    private Dictionary<string, List<EventSubscription>> _subscriptions = new();

    public EventManager(ILogger<EventManager> logger, IEvoSCApplication app, IControllerManager controllers)
    {
        _logger = logger;
        _app = app;
        _controllers = controllers;
    }

    public void Subscribe(EventSubscription subscription)
    {
        if (!_subscriptions.ContainsKey(subscription.Name))
        {
            _subscriptions.Add(subscription.Name, new List<EventSubscription>());
        }
        
        _subscriptions[subscription.Name].Add(subscription);
        _subscriptions[subscription.Name].Sort((a, b) =>
        {
            int ap = (int)a.Priority;
            int bp = (int)b.Priority;

            if (ap > bp)
                return -1;
            else if (ap < bp)
                return 1;
            return 0;
        });
        _subscriptions[subscription.Name].Sort((a, b) =>
            a.RunAsync ? -1 : (b.RunAsync ? 1 : 0));
    }

    public void Subscribe(Action<EventSubscriptionBuilder> builderAction)
    {
        var builder = new EventSubscriptionBuilder();
        builderAction(builder);
        Subscribe(builder.Build());
    }

    public void Subscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler,
        EventPriority priority = EventPriority.Medium, bool runAsync = false) where TArgs : EventArgs
    {
        Subscribe(new EventSubscription(
            name,
            handler.Target.GetType(),
            handler.Method,
            handler.Target,
            priority,
            runAsync)
        );
    }

    public void Unsubscribe(EventSubscription subscription)
    {
        if (!_subscriptions.ContainsKey(subscription.Name))
        {
            throw new EventSubscriptionNotFound();
        }

        _subscriptions[subscription.Name].Remove(subscription);

        if (_subscriptions[subscription.Name].Count == 0)
        {
            _subscriptions.Remove(subscription.Name);
        }
    }

    public void Unsubscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler) where TArgs : EventArgs
    {
        Unsubscribe(new EventSubscription(name, handler.Target.GetType(), handler.Method));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public async Task Raise(string name, EventArgs args, object? sender = null)
    {
        if (!_subscriptions.ContainsKey(name))
        {
            return;
        }

        var tasks = InvokeEventTasks(name, args, sender ?? this);
        await WaitEventTasks(tasks);
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private ConcurrentQueue<(Task, EventSubscription)> InvokeEventTasks(string name, EventArgs args, object? sender)
    {
        ConcurrentQueue<(Task, EventSubscription)> tasks = new ConcurrentQueue<(Task, EventSubscription)>();

        foreach (var subscription in _subscriptions[name])
        {
            if (subscription.RunAsync)
            {
                Task.Run(() =>
                {
                    _logger.LogInformation("run async");
                    InvokeTaskMethod(args, sender, subscription, tasks).GetAwaiter().GetResult();
                });
            }
            else
            {
                InvokeTaskMethod(args, sender, subscription, tasks).GetAwaiter().GetResult();
            }
        }

        return tasks;
    }

    private Task InvokeTaskMethod(EventArgs args, object? sender, EventSubscription subscription,
        ConcurrentQueue<(Task, EventSubscription)> tasks)
    {
        try
        {
            Task? task = null;
            var target = GetTarget(subscription);

            task = (Task?)subscription.HandlerMethod.Invoke(target, new[] {sender, args});

            if (task == null)
            {
                _logger.LogError("An error occured while calling event, task is null for event: {Name}",
                    subscription.Name);
                return Task.CompletedTask;
            }

            tasks.Enqueue((task, subscription));

            return task;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to execute subscription: {Msg} | Stacktrace: {St}", ex.Message,
                ex.StackTrace);
        }

        return Task.CompletedTask;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task WaitEventTasks(ConcurrentQueue<(Task, EventSubscription)> tasks)
    {
        while (tasks.TryDequeue(out var result))
        {
            var (task, sub) = result;

            try
            {
                await task;
            }
            catch (Exception ex)
            {
            }

            if (!task.IsCompletedSuccessfully)
            {
                _logger.LogError("Event execution failed for {Name}, status: {Status}", sub.Name, task.Status);

                if (task.IsFaulted)
                {
                    _logger.LogError("Event handler faulted, exception: {Msg} | Stacktrace: {St}",
                        task.Exception?.InnerException?.Message, task.Exception?.InnerException?.StackTrace);
                }
            }
        }
    }

    private object GetTarget(EventSubscription subscription)
    {
        if (subscription.Instance != null)
        {
            return subscription.Instance;
        }

        if (subscription.IsController)
        {
            return CreateControllerInstance(subscription);
        }
        
        return ActivatorUtilities.CreateInstance(_app.Services, subscription.InstanceClass);
    }

    private IController CreateControllerInstance(EventSubscription subscription)
    {
        var (instance, scopeContext) = _controllers.CreateInstance(subscription.InstanceClass);
        var context = new EventControllerContext(scopeContext);
        instance.SetContext(context);
        
        return instance;
    }

    public void RegisterForController(Type controllerType)
    {
        var methods = controllerType.GetMethods(ReflectionUtils.InstanceMethods);

        foreach (var method in methods)
        {
            var attr = method.GetCustomAttribute<Subscribe>();

            if (attr == null)
            {
                continue;
            }

            var subscription = new EventSubscription(attr.Name, controllerType, method, null, attr.Priority,
                attr.IsAsync, true);
            Subscribe(subscription);
        }
    }
}
