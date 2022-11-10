using EvoSC.Common.Interfaces;
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
                    throw ex;
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
