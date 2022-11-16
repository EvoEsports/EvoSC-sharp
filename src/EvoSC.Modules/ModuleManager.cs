using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using Config.Net;
using EvoSC.Common.Config.Stores;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Middleware;
using EvoSC.Common.Middleware.Attributes;
using EvoSC.Common.Permissions.Attributes;
using EvoSC.Common.Permissions.Models;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Exceptions;
using EvoSC.Modules.Exceptions.ModuleServices;
using EvoSC.Modules.Extensions;
using EvoSC.Modules.Info;
using EvoSC.Modules.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector.Lifestyles;
using Container = SimpleInjector.Container;

namespace EvoSC.Modules;

public class ModuleManager : IModuleManager
{
    private readonly ILogger<ModuleManager> _logger;
    private readonly Container _services;
    private readonly IControllerManager _controllers;
    private readonly IModuleServicesManager _servicesManager;
    private readonly DbConnection _db;
    private readonly IActionPipelineManager _pipelineManager;
    private readonly IPermissionManager _permissions;

    private readonly Dictionary<Guid, IModuleLoadContext> _loadedModules = new();

    public ModuleManager(ILogger<ModuleManager> logger, Container services, IControllerManager controllers,
        IModuleServicesManager servicesManager, IActionPipelineManager pipelineManager, DbConnection db, IPermissionManager permissions)
    {
        _logger = logger;
        _services = services;
        _controllers = controllers;
        _servicesManager = servicesManager;
        _db = db;
        _pipelineManager = pipelineManager;
        _permissions = permissions;
    }

    private async Task<Guid> LoadInternalModule(Type moduleClass, ModuleAttribute moduleInfo)
    {
        var loadId = Guid.NewGuid();
        var moduleServices = CreateServiceContainer(moduleClass.Assembly);
        _servicesManager.AddContainer(loadId, moduleServices);

        await AddModuleConfig(moduleClass.Assembly, moduleServices, moduleInfo);
        
        var instance = (IEvoScModule)ActivatorUtilities.CreateInstance(moduleServices, moduleClass);

        var loadContext = new ModuleLoadContext
        {
            Instance = instance,
            LoadContext = null,
            LoadId = loadId,
            ModuleClass = moduleClass,
            ModuleInfo = moduleInfo,
            Assembly = moduleClass.Assembly,
            Services = moduleServices,
            ActionPipeline = new ActionPipeline(),
            Permissions = new List<IPermission>()
        };
        
        _loadedModules.Add(loadId, loadContext);

        await AddMiddlewares(loadContext);
        await AddPermissions(loadContext);

        return loadId;
    }

    private IModuleLoadContext GetModuleById(Guid loadId)
    {
        if (loadId == Guid.Empty || !_loadedModules.ContainsKey(loadId))
        {
            throw new EvoScModuleException($"Module with Id {loadId} does not exist.");
        }

        var moduleContext = _loadedModules[loadId];
        return moduleContext;
    }
    
    private async Task AddPermissions(ModuleLoadContext loadContext)
    {
        foreach (var permGroup in loadContext.Assembly.AssemblyTypesWithAttribute<PermissionGroupAttribute>())
        {
            var rootName = permGroup.Name;

            var idAttr = permGroup.GetCustomAttribute<IdentifierAttribute>();
            if (idAttr != null)
            {
                rootName = idAttr.Name;
            }

            foreach (var f in permGroup.GetFields())
            {
                if (f.FieldType != permGroup)
                {
                    continue;
                }

                var actualName = f.GetCustomAttribute<IdentifierAttribute>()?.Name ?? f.Name;
                
                loadContext.Permissions.Add(new Permission
                {
                    Name = $"{rootName}.{actualName}",
                    Description = f.GetCustomAttribute<DescriptionAttribute>()?.Description ?? ""
                });
            }
        }
    }

    private async Task InstallModule(Guid loadId)
    {
        var moduleContext = GetModuleById(loadId);

        await InstallPermissions(moduleContext);
    }

    private async Task InstallPermissions(IModuleLoadContext moduleContext)
    {
        foreach (var permission in moduleContext.Permissions)
        {
            var existingPermission = await _permissions.GetPermission(permission.Name);

            if (existingPermission != null)
            {
                _logger.LogDebug("Wont install permission '{Name}' as it already exists", permission.Name);
                continue;
            }

            _logger.LogDebug("Installing permission: {Name}", permission.Name);
            await _permissions.AddPermission(permission);
        }
    }

    public async Task EnableModule(Guid loadId)
    {
        var moduleContext = GetModuleById(loadId);

        await EnableControllers(moduleContext);
        await EnableMiddlewares(moduleContext);
        await TryCallModuleEnable(moduleContext);
    }

    private async Task EnableMiddlewares(IModuleLoadContext moduleContext)
    {
        _pipelineManager.AddPipeline(moduleContext.LoadId, moduleContext.ActionPipeline);
    }

    private async Task AddMiddlewares(IModuleLoadContext moduleContext)
    {
        foreach (var middlewareType in moduleContext.Assembly.AssemblyTypesWithAttribute<MiddlewareAttribute>())
        {
            moduleContext.ActionPipeline.AddComponent(middlewareType, moduleContext.Services);
        }
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
        foreach (var module in moduleContext.Assembly.Modules)
        {
            foreach (var type in module.GetTypes())
            {
                var controllerAttr = type.GetCustomAttribute<ControllerAttribute>();

                if (controllerAttr == null || !type.IsControllerClass())
                {
                    continue;
                }
                
                _controllers.AddController(type, moduleContext.LoadId, moduleContext.Services);
            }
        }

        return Task.CompletedTask;
    }

    private Container CreateServiceContainer(Assembly assembly)
    {
        var container = new Container();
        container.Options.EnableAutoVerification = false;
        container.Options.SuppressLifestyleMismatchVerification = true;
        container.Options.UseStrictLifestyleMismatchBehavior = false;

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
                        container.Register(type);
                        break;
                    default:
                        throw new ModuleServicesException($"Unsupported lifetime type for module service: {type}");
                }
            }
        }

        return container;
    }

    private async Task AddModuleConfig(Assembly assembly, Container container, ModuleAttribute moduleInfo)
    {
        foreach (var module in assembly.Modules)
        {
            foreach (var type in module.GetTypes())
            {
                var configAttr = type.GetCustomAttribute<SettingsAttribute>();

                if (configAttr == null)
                {
                    continue;
                }

                if (!type.IsInterface)
                {
                    throw new ModuleServicesException($"Settings type {type} must be an interface.");
                }

                var builder = ReflectionUtils.CreateGenericInstance(typeof(ConfigurationBuilder<>), type);
                var dbStore = new DatabaseStore(moduleInfo.Name, type, _db);

                await dbStore.SetupDefaultSettingsAsync();

                ReflectionUtils.CallMethod(builder, "UseConfigStore", dbStore);
                var config = ReflectionUtils.CallMethod(builder, "Build");
                
                container.RegisterInstance(type, config);
            }
        }
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

                var moduleAttr = moduleType.GetEvoScModuleAttribute();
                var loadId = await LoadModule(moduleType, moduleAttr);

                await InstallModule(loadId);
                await EnableModule(loadId);
            }
        }
    }

    private async Task<Guid> LoadModule(Type moduleType, ModuleAttribute moduleAttr)
    {
        var loadId = Guid.Empty;

        if (moduleAttr.IsInternal)
        {
            loadId = await LoadInternalModule(moduleType, moduleAttr);
        }
        
        _logger.LogDebug("Loaded module: {Type}", moduleType);
        
        return loadId;
    }
}
