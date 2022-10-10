using System.Reflection;

namespace EvoSC.Common.Events;

public class EventSubscription
{
    public string Name { get; init; }
    public Type InstanceClass { get; init; }
    public object? Instance { get; init; }
    public MethodInfo HandlerMethod { get; init; }
    public EventPriority Priority { get; init; }
    public bool RunAsync { get; init; }

    public EventSubscription(string name, Type instanceClass, MethodInfo handlerMethod, object? instance=null, EventPriority priority=EventPriority.Medium, bool runAsync=false)
    {
        Name = name;
        InstanceClass = instanceClass;
        Instance = instance;
        HandlerMethod = handlerMethod;
        Priority = priority;
        RunAsync = runAsync;
    }
}
