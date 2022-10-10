using System.Diagnostics;
using System.Reflection;
using EvoSC.Common.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Exceptions;
using EvoSC.Modules.Extensions;
using EvoSC.Modules.Info;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules;

public class ModuleManager : IModuleManager
{
    private readonly ILogger<ModuleManager> _logger;
    private readonly IServiceProvider _services;
    private readonly IControllerManager _controllers;

    private Dictionary<Guid, IModuleLoadContext> _loadedModules = new();

    public ModuleManager(ILogger<ModuleManager> logger, IServiceProvider services, IControllerManager controllers)
    {
        _logger = logger;
        _services = services;
        _controllers = controllers;
    }

    private async Task<Guid> LoadInternalModule(Type moduleClass, ModuleAttribute moduleInfo)
    {
        var loadId = Guid.NewGuid();
        var instance = (IEvoScModule)ActivatorUtilities.CreateInstance(_services, moduleClass);

        var loadContext = new ModuleLoadContext
        {
            Instance = instance,
            LoadContext = null,
            LoadId = loadId,
            ModuleClass = moduleClass,
            ModuleInfo = moduleInfo
        };
        
        _loadedModules.Add(loadId, loadContext);

        return loadId;
    }
    
    public async Task EnableModule(Guid loadId)
    {
        if (loadId == Guid.Empty || !_loadedModules.ContainsKey(loadId))
        {
            throw new EvoScModuleException($"Module with Id {loadId} does not exist.");
        }

        var moduleContext = _loadedModules[loadId];

        await EnableControllers(moduleContext);
        await TryCallModuleEnable(moduleContext);
    }

    private Task TryCallModuleEnable(IModuleLoadContext moduleContext)
    {
        if (moduleContext.ModuleClass.IsAssignableTo(typeof(IToggleable)))
        {
            return ((IToggleable)moduleContext.Instance).Enable();
        }

        return Task.CompletedTask;
    }
    
    private Task EnableControllers(IModuleLoadContext moduleContext)
    {
        if (moduleContext.ModuleInfo.IsInternal)
        {
            foreach (var controller in moduleContext.Instance.Controllers)
            {
                _controllers.Add(controller);
            }
        }

        return Task.CompletedTask;
    }
    
    public async Task LoadModulesFromAssembly(Assembly assembly)
    {
        foreach (var asmModule in assembly.Modules)
        {
            foreach (var moduleType in asmModule.GetTypes())
            {
                if (!moduleType.IsEvoScModuleType())
                {
                    continue;
                }

                var moduleAttr = moduleType.GetModuleAttribute();
                var loadId = Guid.Empty;

                if (moduleAttr.IsInternal)
                {
                    loadId = await LoadInternalModule(moduleType, moduleAttr);
                }

                await EnableModule(loadId);
            }
        }
    }
}
