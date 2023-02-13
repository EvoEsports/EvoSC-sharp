using System.Reflection;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace EvoSC.Common.Controllers;

public class ControllerManager : IControllerManager
{
    private readonly ILogger<ControllerManager> _logger;

    private readonly Dictionary<Type, ControllerInfo> _controllers = new();
    private readonly Dictionary<Type, List<IController>> _instances = new();
    private readonly List<IControllerActionRegistry> _registries = new();

    public IEnumerable<ControllerInfo> Controllers => _controllers.Values;
    
    public ControllerManager(ILogger<ControllerManager> logger)
    {
        _logger = logger;
    }

    public void AddController(Type controllerType, Guid moduleId, Container services)
    {
        ValidateController(controllerType);

        foreach (var registry in _registries)
        {
            registry.RegisterForController(controllerType);
        }

        _controllers.Add(controllerType,
            new ControllerInfo {ControllerType = controllerType, ModuleId = moduleId, Services = services});
    }

    private void ValidateController(Type controllerType)
    {
        if (!controllerType.IsControllerClass())
        {
            _logger.LogError("{Type} does not implement IController. Make sure to inherit the EvoScController base class", controllerType);
            throw new InvalidControllerClassException("The controller must implement IController.");
        }

        if (controllerType.GetCustomAttribute<ControllerAttribute>() == null)
        {
            _logger.LogError("{Type} does not annotate the [Controller] attribute", controllerType);
            throw new InvalidControllerClassException("The controller must annotate the Controller attribute.");
        }
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

        foreach (var registry in _registries)
        {
            registry.UnregisterForController(controllerType);
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
        var scope = AsyncScopedLifestyle.BeginScope(controllerInfo.Services);
        var instance = ActivatorUtilities.CreateInstance(scope, controllerType) as IController;

        if (instance == null)
        {
            throw new ControllerException($"Failed to instantiate controller of type '{controllerType}'.");
        }

        TrackControllerInstance(controllerType, instance);

        var context = scope
            .GetRequiredService<IContextService>()
            .CreateContext(scope, instance);

        return (instance, context);
    }

    private void TrackControllerInstance(Type controllerType, IController instance)
    {
        if (!_instances.ContainsKey(controllerType))
        {
            _instances[controllerType] = new List<IController>();
        }

        _instances[controllerType].Add(instance);
    }

    public Task InvokeActionAsync(IControllerContext context, MethodInfo method, params object[] args)
    {
        // todo: implement this
        // get pipeline
        // execute pipeline
        // cancel

        return Task.CompletedTask;
    }
}
