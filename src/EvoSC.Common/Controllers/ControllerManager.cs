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
using SimpleInjector;

namespace EvoSC.Common.Controllers;

public class ControllerManager : IControllerManager
{
    private readonly ILogger<ControllerManager> _logger;

    private Dictionary<Type, ControllerInfo> _controllers = new();
    private Dictionary<Type, List<IController>> _instances = new();
    private List<IControllerActionRegistry> _registries = new();

    public IEnumerable<ControllerInfo> Controllers => _controllers.Values;
    
    public ControllerManager(ILogger<ControllerManager> logger)
    {
        _logger = logger;
    }

    public void AddController(Type controllerType, Guid moduleId, Container services)
    {
        var controllerInfo = GetControllerInfo(controllerType);

        foreach (var registry in _registries)
        {
            registry.RegisterForController(controllerType);
        }
        
        _controllers.Add(controllerType, new ControllerInfo(controllerType, moduleId, services));
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

    public void AddController<TController>(Guid moduleId, Container services) where TController : IController
        => AddController(typeof(TController), moduleId, services);

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
        var scope = new Scope(controllerInfo.Services);
        var instance = ActivatorUtilities.CreateInstance(scope.Container, controllerType) as IController;

        if (instance == null)
        {
            throw new ControllerException($"Failed to instantiate controller of type '{controllerType}'.");
        }

        var context = CreateContext(scope);
        return (instance, context);
    }

    private IControllerContext CreateContext(Scope scope)
    {
        IControllerContext context = new GenericControllerContext(scope);
        context.SetScope(scope);

        return context;
    }
}
