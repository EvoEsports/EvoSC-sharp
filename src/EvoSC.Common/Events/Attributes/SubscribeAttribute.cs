using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Events.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class SubscribeAttribute : Attribute
{
    /// <summary>
    /// The name of the event to subscribe to.
    /// </summary>
    public string Name { get; init; }
    /// <summary>
    /// Whether to run the callback asynchronous. Enabling this will essentially have the callback run in
    /// it's own thread.
    /// </summary>
    public bool IsAsync { get; init; }
    /// <summary>
    /// The callback priority. Subscriptions with a higher priority will be called first.
    /// </summary>
    public EventPriority Priority { get; init; }

    /// <summary>
    /// Create a new event subscription.
    /// </summary>
    /// <param name="name">Name of the event</param>
    public SubscribeAttribute(string name)
    {
        Name = name;
        Priority = EventPriority.Medium;
        IsAsync = false;
    }
    
    /// <summary>
    /// Create a new event subscription.
    /// </summary>
    /// <param name="name">Name of the event</param>
    /// <param name="priority">Callback priority of the event</param>
    public SubscribeAttribute(string name, EventPriority priority)
    {
        Name = name;
        Priority = priority;
        IsAsync = false;
    }
    
    /// <summary>
    /// Create a new event subscription.
    /// </summary>
    /// <param name="name">Name of the event</param>
    /// <param name="priority">Callback priority of the event</param>
    /// <param name="isAsync">If true, the callback is run in it's own thread and non-blocking.</param>
    public SubscribeAttribute(string name, EventPriority priority, bool isAsync)
    {
        Name = name;
        Priority = priority;
        IsAsync = isAsync;
    }

    /// <summary>
    /// Create a new event subscription.
    /// </summary>
    /// <param name="name">Must be an enumeration. Name of the event.</param>
    public SubscribeAttribute(object name) : this(name.AsEnum().GetIdentifier()) {}
    
    /// <summary>
    /// Create a new event subscription.
    /// </summary>
    /// <param name="name">Must be an enumeration. Name of the event.</param>
    /// <param name="priority">Callback priority of the event</param>
    public SubscribeAttribute(object name, EventPriority priority) : this(name.AsEnum().GetIdentifier(), priority) {}
    
    /// <summary>
    /// Create a new event subscription.
    /// </summary>
    /// <param name="name">Must be an enumeration. Name of the event.</param>
    /// <param name="priority">Callback priority of the event</param>
    /// <param name="isAsync">If true, the callback is run in it's own thread and non-blocking.</param>
    public SubscribeAttribute(object name, EventPriority priority, bool isAsync) : this(name.AsEnum().GetIdentifier(), priority, isAsync) {}
}
