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

namespace EvoSC.Common.Events;

public class EventManager : IEventManager
{
    private readonly ILogger<EventManager> _logger;
    private readonly IServiceProvider _services;
    private readonly IControllerManager _controllers;
    
    private Dictionary<string, List<EventSubscription>> _subscriptions = new();

    public EventManager(ILogger<EventManager> logger, IServiceProvider services, IControllerManager controllers)
    {
        _logger = logger;
        _services = services;
        _controllers = controllers;
    }

    public void Subscribe(EventSubscription subscription)
    {
        if (!_subscriptions.ContainsKey(subscription.Name))
        {
            _subscriptions.Add(subscription.Name, new List<EventSubscription>());
        }
        
        _subscriptions[subscription.Name].Add(subscription);
    }

    public void Subscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler, EventPriority priority=EventPriority.Medium, bool runAsync=false) where TArgs : EventArgs
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

    public void Unsubscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler) where TArgs : EventArgs
    {
        
    }

    public async Task Fire(string name, EventArgs args, object? sender=null)
    {
        if (!_subscriptions.ContainsKey(name))
        {
            return;
        }

        object senderArg = sender ?? this;
        List<Task> tasks = new List<Task>();

        foreach (var subscription in _subscriptions[name])
        {
            Task? task = null;
            var target = GetTarget(subscription);
            
            task = (Task?)subscription.HandlerMethod.Invoke(target, new[] {senderArg, args});

            if (task == null)
            {
                throw new InvalidOperationException("An error occured while calling event, task is null.");
            }
            
            tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());
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
        
        return ActivatorUtilities.CreateInstance(_services, subscription.InstanceClass);
    }

    private IController CreateControllerInstance(EventSubscription subscription)
    {
        var context = new EventControllerContext(); 
        var instance = _controllers.CreateInstance(subscription.InstanceClass, context);
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

            var subscription = new EventSubscription(attr.Name, controllerType, method, null, attr.Priority, attr.IsAsync);
            Subscribe(subscription);
        }
    }
}
