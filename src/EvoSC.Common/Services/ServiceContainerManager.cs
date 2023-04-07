using System.Reflection;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Exceptions;
using EvoSC.Common.Services.Models;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace EvoSC.Common.Services;

public class ServiceContainerManager : IServiceContainerManager
{
    private readonly IEvoSCApplication _app;
    private readonly ILogger<ServiceContainerManager> _logger;

    private readonly Dictionary<Guid, Container> _containers = new();
    private readonly Dictionary<Guid, List<Guid>> _dependencyServices = new();

    public ServiceContainerManager(IEvoSCApplication app, ILogger<ServiceContainerManager> logger)
    {
        _app = app;
        _logger = logger;
    }

    public void AddContainer(Guid containerId, Container container)
    {
        container.ResolveUnregisteredType += (_, args) =>
        {
            ResolveCoreService(args, containerId);
        };
        
        if (_containers.ContainsKey(containerId))
        {
            throw new ServicesException($"A container is already registered for ID: {containerId}");
        }
        
        _containers.Add(containerId, container);
        
        _logger.LogDebug("Added service container with ID: {ContainerId}", containerId);
    }

    public Container NewContainer(Guid containerId, IEnumerable<Assembly> assemblies, List<Guid> loadedDependencies)
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
                        throw new ServicesException($"Service {type} must implement a custom interface.");
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
                            throw new ServicesException($"Unsupported lifetime type for service: {type}");
                    }
                }
            }
        }

        AddContainer(containerId, container);
        
        foreach (var dependency in loadedDependencies)
        {
            RegisterDependency(containerId, dependency);
        }
        
        return container;
    }

    public void RemoveContainer(Guid containerId)
    {
        if (!_containers.ContainsKey(containerId))
        {
            throw new ServicesException($"No container for {containerId} was found.");
        }

        var container = _containers[containerId];
        container.Dispose();
        _containers.Remove(containerId);
        
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        _logger.LogDebug("Removed service container for container: {ContainerId}", containerId);
    }

    public void RegisterDependency(Guid containerId, Guid dependencyId)
    {
        if (!_containers.ContainsKey(containerId))
        {
            throw new InvalidOperationException($"Container '{containerId}' was not found to have a container.");
        }

        if (!_containers.ContainsKey(dependencyId))
        {
            throw new InvalidOperationException($"Dependency container '{containerId}' was not found to have a container.");
        }

        if (!_dependencyServices.ContainsKey(containerId))
        {
            _dependencyServices[containerId] = new List<Guid>();
        }
        
        _dependencyServices[containerId].Add(dependencyId);
        _logger.LogDebug("Registered dependency '{DepId}' for '{ContainerId}'", dependencyId, containerId);
    }
    
    private void ResolveCoreService(UnregisteredTypeEventArgs e, Guid containerId)
    {
        try
        {
            e.Register(() =>
            {
                _logger.LogTrace("Will attempt to resolve service '{Service}' for {Container}", 
                    e.UnregisteredServiceType,
                    containerId);
                
                if (_dependencyServices.ContainsKey(containerId))
                {
                    foreach (var dependencyId in _dependencyServices[containerId])
                    {
                        try
                        {
                            return _containers[dependencyId].GetInstance(e.UnregisteredServiceType);
                        }
                        catch (ActivationException ex)
                        {
                            _logger.LogTrace(ex,
                                "Did not find service {Service} for container {Container} in dependency {Dependency}",
                                e.UnregisteredServiceType,
                                containerId,
                                dependencyId);
                        }
                    }
                }
                
                try
                {
                    _logger.LogTrace(
                        "Dependencies does not have service '{Service}' for {Container}. Will try core services",
                        e.UnregisteredServiceType,
                        containerId);
                    
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
