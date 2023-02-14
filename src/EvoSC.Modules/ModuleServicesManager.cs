using System.Reflection;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Services;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Exceptions.ModuleServices;
using EvoSC.Modules.Interfaces;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace EvoSC.Modules;

public class ModuleServicesManager : IModuleServicesManager
{
    private readonly IEvoSCApplication _app;
    private readonly ILogger<ModuleServicesManager> _logger;

    private readonly Dictionary<Guid, Container> _moduleContainers = new();
    private readonly Dictionary<Guid, List<Guid>> _dependencyServices = new();

    public ModuleServicesManager(IEvoSCApplication app, ILogger<ModuleServicesManager> logger)
    {
        _app = app;
        _logger = logger;
    }

    public void AddContainer(Guid moduleId, Container container)
    {
        container.ResolveUnregisteredType += (_, args) =>
        {
            ResolveCoreService(args, moduleId);
        };
        
        if (_moduleContainers.ContainsKey(moduleId))
        {
            throw new ModuleServicesException($"A container is already registered for module: {moduleId}");
        }
        
        _moduleContainers.Add(moduleId, container);
        
        _logger.LogDebug("Added service container with ID: {ModuleId}", moduleId);
    }

    public void RegisterDependency(Guid moduleId, Guid dependencyId)
    {
        if (!_moduleContainers.ContainsKey(moduleId))
        {
            throw new InvalidOperationException($"Module '{moduleId}' was not found to have a container.");
        }

        if (!_moduleContainers.ContainsKey(dependencyId))
        {
            throw new InvalidOperationException($"Dependency module '{moduleId}' was not found to have a container.");
        }

        if (!_dependencyServices.ContainsKey(moduleId))
        {
            _dependencyServices[moduleId] = new List<Guid>();
        }
        
        _dependencyServices[moduleId].Add(dependencyId);
        _logger.LogDebug("Registered dependency '{DepId}' for '{ModuleId}'", dependencyId, moduleId);
    }

    public Container NewContainer(Guid moduleId, IEnumerable<Assembly> assemblies, List<Guid> loadedDependencies)
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.Options.SuppressLifestyleMismatchVerification = true;
        container.Options.UseStrictLifestyleMismatchBehavior = false;
        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

        container.AddEvoScCommonScopedServices();

        foreach (var assembly in assemblies)
        {
            foreach (var module in assembly.Modules)
            {
                foreach (var type in module.GetTypes())
                {
                    var serviceAttr = type.GetCustomAttribute<ServiceAttribute>();

                    if (serviceAttr == null)
                    {
                        continue;
                    }

                    var intf = type.GetInterfaces().FirstOrDefault();

                    if (intf == null)
                    {
                        throw new ModuleServicesException($"Service {type} must implement a custom interface.");
                    }

                    switch (serviceAttr.LifeStyle)
                    {
                        case ServiceLifeStyle.Singleton:
                            container.RegisterSingleton(intf, type);
                            break;
                        case ServiceLifeStyle.Transient:
                            container.Register(intf, type);
                            break;
                        case ServiceLifeStyle.Scoped:
                            container.Register(intf, type, Lifestyle.Scoped);
                            break;
                        default:
                            throw new ModuleServicesException($"Unsupported lifetime type for module service: {type}");
                    }
                }
            }
        }

        AddContainer(moduleId, container);
        
        foreach (var dependency in loadedDependencies)
        {
            RegisterDependency(moduleId, dependency);
        }
        
        return container;
    }

    public void RemoveContainer(Guid moduleId)
    {
        if (!_moduleContainers.ContainsKey(moduleId))
        {
            throw new ModuleServicesException( $"No container for {moduleId} was found.");
        }

        var container = _moduleContainers[moduleId];
        container.Dispose();
        _moduleContainers.Remove(moduleId);
        
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        _logger.LogDebug("Removed service container for module: {ModuleId}", moduleId);
    }

    private void ResolveCoreService(UnregisteredTypeEventArgs e, Guid moduleId)
    {
        try
        {
            e.Register(() =>
            {
                _logger.LogTrace("Will attempt to resolve service '{Service}' for {Module}", 
                    e.UnregisteredServiceType,
                    moduleId);
                
                if (_dependencyServices.ContainsKey(moduleId))
                {
                    foreach (var dependencyId in _dependencyServices[moduleId])
                    {
                        try
                        {
                            return _moduleContainers[dependencyId].GetInstance(e.UnregisteredServiceType);
                        }
                        catch (ActivationException ex)
                        {
                            _logger.LogTrace(ex,
                                "Did not find service {Service} for module {Module} in dependency {Dependency}",
                                e.UnregisteredServiceType,
                                moduleId,
                                dependencyId);
                        }
                    }
                }
                
                try
                {
                    _logger.LogTrace(
                        "Dependencies does not have service '{Service}' for {Module}. Will try core services",
                        e.UnregisteredServiceType,
                        moduleId);
                    
                    return _app.Services.GetInstance(e.UnregisteredServiceType);
                }
                catch (ActivationException ex)
                {
                    _logger.LogError(ex, "Failed to get EvoSC core service");
                    throw;
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unknown error occured while trying to resolve a core service");
        }
    }
}
