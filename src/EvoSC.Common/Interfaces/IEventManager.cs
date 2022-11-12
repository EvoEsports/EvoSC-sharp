using EvoSC.Common.Events;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Util;
using GbxRemoteNet;

namespace EvoSC.Common.Interfaces;

public interface IEventManager : IControllerActionRegistry
{
    /// <summary>
    /// Create a new event subscription that has a callback method.
    /// </summary>
    /// <param name="subscription">Subscription info that is used to subscribe to the event.</param>
    public void Subscribe(EventSubscription subscription);
    /// <summary>
    /// Create a new event subscription that has a callback method.
    /// </summary>
    /// <param name="subscription">Builder that provides a way to create the subscription info.</param>
    public void Subscribe(Action<EventSubscriptionBuilder> builder);

    /// <summary>
    /// Subscribe to an event given the provided callback handler.
    /// </summary>
    /// <param name="name">The name of the event to subscribe to.</param>
    /// <param name="handler">The callback method of the subscription.</param>
    /// <param name="priority">Callback priority.</param>
    /// <param name="runAsync">If true, the callback is run in a new thread.</param>
    /// <typeparam name="TArgs">The event argument's type.</typeparam>
    public void Subscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler, EventPriority priority, bool runAsync)
        where TArgs : EventArgs;
    /// <summary>
    /// Subscribe to an event given the provided callback handler.
    /// </summary>
    /// <param name="name">The name of the event to subscribe to.</param>
    /// <param name="handler">The callback method of the subscription.</param>
    /// <param name="priority">Callback priority.</param>
    /// <typeparam name="TArgs">The event argument's type.</typeparam>
    public void Subscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler, EventPriority priority)
        where TArgs : EventArgs => Subscribe(name, handler, priority, false);
    /// <summary>
    /// Subscribe to an event given the provided callback handler.
    /// </summary>
    /// <param name="name">The name of the event to subscribe to.</param>
    /// <param name="handler">The callback method of the subscription.</param>
    /// <typeparam name="TArgs">The event argument's type.</typeparam>
    public void Subscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler) where TArgs : EventArgs =>
        Subscribe(name, handler, EventPriority.Medium, false);

    /// <summary>
    /// Remove an event subscription.
    /// </summary>
    /// <param name="subscription">The subscription to remove.</param>
    public void Unsubscribe(EventSubscription subscription);
    /// <summary>
    /// Remove an event subscription of a provided callback handler.
    /// </summary>
    /// <param name="name">The name of the event.</param>
    /// <param name="handler">Callback handler of the subscription.</param>
    /// <typeparam name="TArgs">The event argument's type.</typeparam>
    public void Unsubscribe<TArgs>(string name, AsyncEventHandler<TArgs> handler) where TArgs : EventArgs;
    /// <summary>
    /// Dispatch an event with event args.
    /// </summary>
    /// <param name="name">Name of the event to dispatch.</param>
    /// <param name="args">Event arguments.</param>
    /// <param name="sender">The entity that triggered the event.</param>
    /// <returns></returns>
    public Task Raise(string name, EventArgs args, object? sender);
    /// <summary>
    /// Dispatch an event with event args.
    /// </summary>
    /// <param name="name">Name of the event to dispatch.</param>
    /// <param name="args">Event arguments.</param>
    /// <returns></returns>
    public Task Raise(string name, EventArgs args) => Raise(name, args, null);
    /// <summary>
    /// Dispatch an event with event args.
    /// </summary>
    /// <param name="name">Name of the event to dispatch.</param>
    /// <param name="args">Event arguments.</param>
    /// <param name="sender">The entity that triggered the event.</param>
    /// <returns></returns>
    public Task Raise(Enum name, EventArgs args, object? sender) =>
        Raise(name.GetEventIdentifier(), args, sender);
    /// <summary>
    /// Dispatch an event with event args.
    /// </summary>
    /// <param name="name">Name of the event to dispatch.</param>
    /// <param name="args">Event arguments.</param>
    /// <returns></returns>
    public Task Raise(Enum name, EventArgs args) =>
        Raise(name, args, null);
}
