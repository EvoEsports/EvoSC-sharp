using System.Reflection;

namespace EvoSC.Common.Events;

public class EventSubscription : IEquatable<EventSubscription>
{
    /// <summary>
    /// Name of the event that the subscription is subscribed to.
    /// </summary>
    public required string Name { get; init; }
    /// <summary>
    /// The class type that contains the callback method.
    /// </summary>
    public required Type InstanceClass { get; init; }
    /// <summary>
    /// The class instance to call the callback upon.
    /// </summary>
    public object? Instance { get; init; }
    /// <summary>
    /// The callback method used to handle this event subscription.
    /// </summary>
    public required MethodInfo HandlerMethod { get; init; }
    /// <summary>
    /// Callback priority. Higher priority callbacks are called first.
    /// </summary>
    public EventPriority Priority { get; init; } = EventPriority.Medium;
    /// <summary>
    /// Whether to run the handler async in its own thread or not.
    /// </summary>
    public bool RunAsync { get; init; }
    /// <summary>
    /// Whether the instance class is a controller or not.
    /// </summary>
    public bool IsController { get; init; }

    public bool Equals(EventSubscription? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return HandlerMethod.Equals(other.HandlerMethod);
    }

    public override int GetHashCode()
    {
        return HandlerMethod.GetHashCode();
    }
}
