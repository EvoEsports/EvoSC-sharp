using System.Reflection;
using EvoSC.Common.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Exceptions.ModuleServices;
using EvoSC.Modules.Interfaces;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace EvoSC.Modules;

public class ModuleServicesManager : IModuleServicesManager
{
    private readonly IEvoSCApplication _app;
    private readonly ILogger<ModuleServicesManager> _logger;

    private readonly Dictionary<Guid, Container> _moduleContainers = new();

    public ModuleServicesManager(IEvoSCApplication app, ILogger<ModuleServicesManager> logger)
    {
        _app = app;
        _logger = logger;
    }

    public void AddContainer(Guid moduleId, Container container)
    {
        container.ResolveUnregisteredType += ResolveCoreService;
        
        if (_moduleContainers.ContainsKey(moduleId))
        {
            throw new ModuleServicesException($"A container is already registered for module: {moduleId}");
        }
        
        _moduleContainers.Add(moduleId, container);
    }

    public Container NewContainer(Guid moduleId, IEnumerable<Assembly> assemblies)
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.Options.SuppressLifestyleMismatchVerification = true;
        container.Options.UseStrictLifestyleMismatchBehavior = false;

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
                        default:
                            throw new ModuleServicesException($"Unsupported lifetime type for module service: {type}");
                    }
                }
            }
        }

        AddContainer(moduleId, container);
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
    }

    private void ResolveCoreService(object? sender, UnregisteredTypeEventArgs e)
    {
        try
        {
            e.Register(() =>
            {
                try
                {
                    return _app.Services.GetInstance(e.UnregisteredServiceType);
                }
                catch (ActivationException ex)
                {
                    _logger.LogError("Failed to get EvoSC core service: {Msg} | Stacktrace: {St}", ex.Message, ex.StackTrace);
                    throw;
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "An unknown error occured while trying to resolve a core service: {Msg} | Stacktrace: {St}", ex.Message,
                ex.StackTrace);
        }
    }
}
