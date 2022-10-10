using System.Reflection;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Util;
using GbxRemoteNet;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Controllers;

public class ControllerManager : IControllerManager
{
    private readonly ILogger<ControllerManager> _logger;
    private readonly IEventManager _events;

    private List<ControllerInfo> _controllers = new();

    public IEnumerable<ControllerInfo> Controllers => _controllers;
    
    public ControllerManager(ILogger<ControllerManager> logger, IEventManager events)
    {
        _logger = logger;
        _events = events;
    }

    public void Add(Type controllerType)
    {
        var controllerInfo = GetControllerInfo(controllerType);
        
        RegisterEvents(controllerType);
        
        _controllers.Add(new ControllerInfo
        {
            ControllerType = controllerType
        });
    }

    private void RegisterEvents(Type controllerType)
    {
        var methods = controllerType.GetMethods(ReflectionUtils.InstanceMethods);

        foreach (var method in methods)
        {
            var attr = method.GetCustomAttribute<Subscribe>();

            if (attr == null)
            {
                continue;
            }

            var subscription = new EventSubscription(attr.Name, controllerType, method, null, attr.Priority, attr.IsAsync);
            _events.Subscribe(subscription);
        }
    }
    
    private ControllerAttribute GetControllerInfo(Type controllerType)
    {
        if (!controllerType.IsAssignableTo(typeof(IController)))
        {
            throw new InvalidControllerClassException("The controller must implement IController.");
        }

        var attr = controllerType.GetCustomAttribute<ControllerAttribute>();

        if (attr == null)
        {
            throw new InvalidControllerClassException("The controller must annotate the Controller attribute.");
        }

        return attr;
    }

    public void Add<TController>() where TController : IController
        => Add(typeof(TController));

    public void Remove(Type controllerType)
    {
        
    }

    public void Remove<TController>() where TController : IController
    {
        
    }
}
