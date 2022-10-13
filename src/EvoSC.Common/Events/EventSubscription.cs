using System.Reflection;

namespace EvoSC.Common.Events;

public class EventSubscription : IEquatable<EventSubscription>
{
    public string Name { get; init; }
    public Type InstanceClass { get; init; }
    public object? Instance { get; init; }
    public MethodInfo HandlerMethod { get; init; }
    public EventPriority Priority { get; init; }
    public bool RunAsync { get; init; }
    public bool IsController { get; init; }

    public EventSubscription(string name, Type instanceClass, MethodInfo handlerMethod, object? instance=null, EventPriority priority=EventPriority.Medium, bool runAsync=false, bool isController=false)
    {
        Name = name;
        InstanceClass = instanceClass;
        Instance = instance;
        HandlerMethod = handlerMethod;
        Priority = priority;
        RunAsync = runAsync;
        IsController = isController;
    }

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
