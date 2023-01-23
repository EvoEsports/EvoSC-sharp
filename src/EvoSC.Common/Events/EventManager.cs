using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Events;

public class EventManager : IEventManager
{
    private readonly ILogger<EventManager> _logger;
    private readonly IEvoSCApplication _app;
    private readonly IControllerManager _controllers;
    
    private readonly Dictionary<string, List<EventSubscription>> _subscriptions = new();
    private readonly Dictionary<Type, List<EventSubscription>> _controllerSubscriptions = new();

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
        _subscriptions[subscription.Name].Sort(CompareSubscription);

        _logger.LogDebug("Subscribed to event '{Name}' with handler '{Handler}' in class '{Class}'. In Controller: {IsController}",
            subscription.Name,
            subscription.HandlerMethod,
            subscription.InstanceClass,
            subscription.IsController);
    }

    private static int CompareSubscription(EventSubscription a, EventSubscription b)
    {
        if (a.RunAsync)
        {
            return -1;
        }
        
        return b.RunAsync ? 1 : 0;
    }


    public void Subscribe(Action<EventSubscriptionBuilder> builderAction)
    {
        var builder = new EventSubscriptionBuilder();
        builderAction(builder);
        Subscribe(builder.Build());
    }

    public void Subscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler, EventPriority priority, bool runAsync)
        where TArgs : EventArgs
    {
        Subscribe(new EventSubscription
        {
            Name = name,
            InstanceClass = handler.Target.GetType(),
            HandlerMethod = handler.Method,
            Instance = handler.Target,
            Priority = priority,
            RunAsync = runAsync
        });
    }

    public void Unsubscribe(EventSubscription subscription)
    {
        if (!_subscriptions.ContainsKey(subscription.Name))
        {
            throw new EventSubscriptionNotFound();
        }

        _subscriptions[subscription.Name].Remove(subscription);

        _logger.LogDebug("handler '{Handler}' unsubscribed to event {Name}.",
            subscription.HandlerMethod,
            subscription.Name);

        if (_subscriptions[subscription.Name].Count == 0)
        {
            _subscriptions.Remove(subscription.Name);
        }
    }

    public void Unsubscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler) where TArgs : EventArgs
    {
        Unsubscribe(new EventSubscription
        {
            Name = name, InstanceClass = handler.Target.GetType(), HandlerMethod = handler.Method
        });
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public async Task RaiseAsync(string name, EventArgs args, object? sender)
    {
        if (!_subscriptions.ContainsKey(name))
        {
            return;
        }

        _logger.LogTrace("Attempting to fire event '{Event}'", name);
        
        var tasks = InvokeEventTasks(name, args, sender ?? this);
        await WaitEventTasksAsync(tasks);
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
                    _logger.LogTrace("run async");
                    InvokeTaskMethodAsync(args, sender, subscription, tasks).GetAwaiter().GetResult();
                });
            }
            else
            {
                InvokeTaskMethodAsync(args, sender, subscription, tasks).GetAwaiter().GetResult();
            }
        }

        return tasks;
    }

    private Task InvokeTaskMethodAsync(EventArgs args, object? sender, EventSubscription subscription,
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
    private async Task WaitEventTasksAsync(ConcurrentQueue<(Task, EventSubscription)> tasks)
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
                // ignore exception to handle it later when checking for task errors
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

        if (!_controllerSubscriptions.ContainsKey(controllerType))
        {
            _controllerSubscriptions[controllerType] = new List<EventSubscription>();
        }
        
        foreach (var method in methods)
        {
            var attr = method.GetCustomAttribute<SubscribeAttribute>();

            if (attr == null)
            {
                continue;
            }

            var subscription = new EventSubscription
            {
                Name = attr.Name,
                InstanceClass = controllerType,
                HandlerMethod = method,
                Instance = null,
                Priority = attr.Priority,
                RunAsync = attr.IsAsync,
                IsController = true,
            };
            
            Subscribe(subscription);
            _controllerSubscriptions[controllerType].Add(subscription);
        }
    }

    public void UnregisterForController(Type controllerType)
    {
        if (!_controllerSubscriptions.ContainsKey(controllerType))
        {
            return;
        }

        foreach (var subscription in _controllerSubscriptions[controllerType])
        {
            Unsubscribe(subscription);
        }

        _controllerSubscriptions.Remove(controllerType);
    }
}
