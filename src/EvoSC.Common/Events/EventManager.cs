using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using GbxRemoteNet;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Events;

public class EventManager : IEventManager
{
    private readonly ILogger<EventManager> _logger;
    private Dictionary<string, List<EventSubscription>> _subscriptions = new();

    public EventManager(ILogger<EventManager> logger)
    {
        _logger = logger;
        _logger.LogInformation("event manager");
    }

    public void Subscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler, bool runAsync=false) where TArgs : EventArgs
    {
        if (!_subscriptions.ContainsKey(name))
        {
            _subscriptions.Add(name, new List<EventSubscription>());
        }

        var subscription = new EventSubscription(handler as AsyncEventHandler<EventArgs>, runAsync);
        _subscriptions[name].Add(subscription);
    }

    public void Unsubscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler) where TArgs : EventArgs
    {
        if (!_subscriptions.ContainsKey(name))
        {
            throw new EventSubscriptionNotFound();
        }

        var subscription = _subscriptions[name].FirstOrDefault(s => s.Handler == handler as AsyncEventHandler<EventArgs>);

        if (subscription == null)
        {
            throw new EventSubscriptionNotFound();
        }

        _subscriptions[name].Remove(subscription);
    }

    public async Task Fire(string name, EventArgs args)
    {
        if (!_subscriptions.ContainsKey(name))
        {
            return;
        }

        foreach (var subscription in _subscriptions[name])
        {
            var task = subscription.Handler.Invoke(this, args);

            if (!subscription.RunAsync)
            {
                await task;
            }
        }
    }
}
