using System.Reflection;

namespace EvoSC.Common.Events;

public class EventSubscriptionBuilder
{
    private string _name;
    private Type _instanceClass;
    private object? _instance;
    private MethodInfo _handlerMethod;
    private EventPriority _priority = EventPriority.Medium;
    private bool _runAsync = false;
    private bool _isController = false;

    public EventSubscriptionBuilder WithEvent(string name)
    {
        _name = name;
        return this;
    }

    public EventSubscriptionBuilder WithInstanceClass(Type classType)
    {
        _instanceClass = classType;
        return this;
    }

    public EventSubscriptionBuilder WithInstanceClass<TClass>()
    {
        WithInstanceClass(typeof(TClass));
        return this;
    }

    public EventSubscriptionBuilder WithInstance(object instanceObject)
    {
        _instance = instanceObject;
        return this;
    }

    public EventSubscriptionBuilder WithHandlerMethod(MethodInfo method)
    {
        _handlerMethod = method;
        return this;
    }

    public EventSubscriptionBuilder WithHandlerMethod<TArgs>(AsyncEventHandler<TArgs> method) where TArgs : EventArgs
    {
        WithHandlerMethod(method.Method);
        return this;
    }

    public EventSubscriptionBuilder WithPriority(EventPriority priority)
    {
        _priority = priority;
        return this;
    }

    public EventSubscriptionBuilder WithLowPriority()
    {
        WithPriority(EventPriority.Low);
        return this;
    }

    public EventSubscriptionBuilder WithMediumPriority()
    {
        WithPriority(EventPriority.Medium);
        return this;
    }

    public EventSubscriptionBuilder WithHighPriority()
    {
        WithPriority(EventPriority.High);
        return this;
    }

    public EventSubscriptionBuilder AsAsync(bool runAsync)
    {
        _runAsync = runAsync;
        return this;
    }

    public EventSubscriptionBuilder AsAsync() => AsAsync(true);

    public EventSubscriptionBuilder AsController(bool isController)
    {
        _isController = isController;
        return this;
    }

    public EventSubscriptionBuilder AsController() => AsController(true);
    
    public EventSubscription Build()
    {
        if (_name == null)
        {
            throw new InvalidOperationException("Event name must be set.");
        }
        
        if (_instanceClass == null)
        {
            throw new InvalidOperationException("Instance class must be set for event.");
        }
        
        if (_handlerMethod == null)
        {
            throw new InvalidOperationException("Handler method must be set for event.");
        }

        return new EventSubscription(
            _name,
            _instanceClass,
            _handlerMethod,
            _instance,
            _priority,
            _runAsync,
            _isController
        );
    }
}
