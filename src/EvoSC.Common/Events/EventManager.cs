using System.Reflection;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using GbxRemoteNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Events;

public class EventManager : IEventManager
{
    private readonly ILogger<EventManager> _logger;
    private readonly IServiceProvider _services;
    
    private Dictionary<string, List<EventSubscription>> _subscriptions = new();

    public EventManager(ILogger<EventManager> logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
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

        foreach (var subscription in _subscriptions[name])
        {
            Task? task = null;
            var target = subscription.Instance == null
                ? ActivatorUtilities.CreateInstance(_services, subscription.InstanceClass)
                : subscription.Instance;
            
            task = (Task?)subscription.HandlerMethod.Invoke(target, new[] {senderArg, args});

            if (task == null)
            {
                throw new InvalidOperationException("An error occured while calling event, task is null.");
            }
            
            if (!subscription.RunAsync)
            {
                await task;
            }
        }
    }
}
