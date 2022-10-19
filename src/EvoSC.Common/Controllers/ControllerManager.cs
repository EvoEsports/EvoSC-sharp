using System.Reflection;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Util;
using GbxRemoteNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Controllers;

public class ControllerManager : IControllerManager
{
    private readonly ILogger<ControllerManager> _logger;
    private readonly IServiceProvider _services;

    private Dictionary<Type, ControllerInfo> _controllers = new();
    private Dictionary<Type, List<IController>> _instances = new();
    private List<IControllerActionRegistry> _registries = new();

    public IEnumerable<ControllerInfo> Controllers => _controllers.Values;
    
    public ControllerManager(ILogger<ControllerManager> logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
    }

    public void AddController(Type controllerType, Guid moduleId)
    {
        var controllerInfo = GetControllerInfo(controllerType);

        foreach (var registry in _registries)
        {
            registry.RegisterForController(controllerType);
        }
        
        _controllers.Add(controllerType, new ControllerInfo(controllerType, moduleId));
    }

    private ControllerAttribute GetControllerInfo(Type controllerType)
    {
        if (!controllerType.IsControllerClass())
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

    public void AddController<TController>(Guid moduleId) where TController : IController
        => AddController(typeof(TController), moduleId);

    public void AddControllerActionRegistry(IControllerActionRegistry registry)
    {
        _registries.Add(registry);
    }

    public void RemoveController(Type controllerType)
    {
        if (!_controllers.ContainsKey(controllerType))
        {
            throw new ControllerException("Controller not found.");
        }

        _controllers.Remove(controllerType);

        if (_instances.ContainsKey(controllerType))
        {
            DisposeControllerInstances(controllerType);
        }
    }

    private void DisposeControllerInstances(Type controllerType)
    {
        foreach (var instance in _instances[controllerType])
        {
            try
            {
                instance.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to dispose controller instance of type '{Type}'", controllerType);
            }
        }

        _instances.Remove(controllerType);

        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    public void RemoveModuleControllers(Guid moduleId)
    {
        foreach (var (controllerType, controllerInfo) in _controllers)
        {
            if (controllerInfo.ModuleId == moduleId)
            {
                RemoveController(controllerType);
            }
        }
    }

    public ControllerInfo GetInfo(Type controllerType)
    {
        if (!_controllers.ContainsKey(controllerType))
        {
            throw new ControllerException("Controller not found.");
        }

        return _controllers[controllerType];
    }
    
    public (IController, IControllerContext) CreateInstance(Type controllerType)
    {
        var controllerInfo = GetInfo(controllerType);
        var scope = _services.CreateScope();
        var instance = ActivatorUtilities.CreateInstance(scope.ServiceProvider, controllerType) as IController;

        if (instance == null)
        {
            throw new ControllerException($"Failed to instantiate controller of type '{controllerType}'.");
        }

        var context = CreateContext(scope);
        return (instance, context);
    }

    private IControllerContext CreateContext(IServiceScope scope)
    {
        IControllerContext context = new GenericControllerContext(scope);
        context.SetScope(scope);

        return context;
    }
}
