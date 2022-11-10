using EvoSC.Common.Util;

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

    public SubscribeAttribute(string name)
    {
        Name = name;
        Priority = EventPriority.Medium;
        IsAsync = false;
    }
    
    public SubscribeAttribute(string name, EventPriority priority)
    {
        Name = name;
        Priority = priority;
        IsAsync = false;
    }
    
    public SubscribeAttribute(string name, EventPriority priority, bool isAsync)
    {
        Name = name;
        Priority = priority;
        IsAsync = isAsync;
    }

    public SubscribeAttribute(object name) : this((name as Enum).GetEventIdentifier()) {}
    public SubscribeAttribute(object name, EventPriority priority) : this((name as Enum).GetEventIdentifier(), priority) {}
    public SubscribeAttribute(object name, EventPriority priority, bool isAsync) : this((name as Enum).GetEventIdentifier(), priority, isAsync) {}
}
