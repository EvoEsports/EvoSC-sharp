using EvoSC.Common.Events;
using EvoSC.Common.Interfaces.Controllers;
using GbxRemoteNet;

namespace EvoSC.Common.Interfaces;

public interface IEventManager : IControllerActionRegistry
{
    /// <summary>
    /// Create a new event subscription that has a callback method.
    /// </summary>
    /// <param name="subscription"></param>
    public void Subscribe(EventSubscription subscription);
    /// <summary>
    /// Create a new event subscription that has a callback method.
    /// </summary>
    /// <param name="subscription"></param>
    public void Subscribe(Action<EventSubscriptionBuilder> builder);
    /// <summary>
    /// Subscribe to an event given the provided callback handler.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="handler"></param>
    /// <param name="priority"></param>
    /// <param name="runAsync"></param>
    /// <typeparam name="TArgs"></typeparam>
    public void Subscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler, EventPriority priority=EventPriority.Medium, bool runAsync=false) where TArgs : EventArgs;
    /// <summary>
    /// Remove an event subscription.
    /// </summary>
    /// <param name="subscription"></param>
    public void Unsubscribe(EventSubscription subscription);
    /// <summary>
    /// Remove an event subscription of a provided callback handler.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="handler"></param>
    /// <typeparam name="TArgs"></typeparam>
    public void Unsubscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler) where TArgs : EventArgs;
    /// <summary>
    /// Dispatch an event with event args.
    /// </summary>
    /// <param name="name">Name of the event to dispatch.</param>
    /// <param name="args">Event arguments.</param>
    /// <param name="sender">The entity that triggered the event.</param>
    /// <returns></returns>
    public Task Raise(string name, EventArgs args, object? sender=null);
}
