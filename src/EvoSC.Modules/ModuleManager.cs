using System.ComponentModel;
using System.Data.Common;
using System.Reflection;
using Config.Net;
using EvoSC.Common.Config.Stores;
using EvoSC.Common.Controllers.Attributes;
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
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Container = SimpleInjector.Container;

namespace EvoSC.Modules;

public class ModuleManager : IModuleManager
{
    private readonly ILogger<ModuleManager> _logger;
    private readonly IControllerManager _controllers;
    private readonly IModuleServicesManager _servicesManager;
    private readonly DbConnection _db;
    private readonly IActionPipelineManager _pipelineManager;
    private readonly IPermissionManager _permissions;

    private readonly Dictionary<Guid, IModuleLoadContext> _loadedModules = new();

    public ModuleManager(ILogger<ModuleManager> logger, IControllerManager controllers,
        IModuleServicesManager servicesManager, IActionPipelineManager pipelineManager, DbConnection db,
        IPermissionManager permissions)
    {
        _logger = logger;
        _controllers = controllers;
        _servicesManager = servicesManager;
        _db = db;
        _pipelineManager = pipelineManager;
        _permissions = permissions;
    }

    private async Task<Guid> LoadInternalModule(Type moduleClass, ModuleAttribute moduleAttr)
    {
        var loadId = Guid.NewGuid();
        var moduleServices = CreateServiceContainer(moduleClass.Assembly);
        _servicesManager.AddContainer(loadId, moduleServices);

        var moduleInfo = CreateModuleInfo(moduleClass.Assembly, true);

        await RegisterModuleConfig(moduleClass.Assembly, moduleServices, moduleInfo);
        
        var loadContext = CreateLoadContext(moduleClass, moduleInfo, loadId, moduleServices);
        
        _loadedModules.Add(loadId, loadContext);

        await RegisterMiddlewares(loadContext);
        await RegisterPermissions(loadContext);

        _logger.LogDebug("Module '{Name}' loaded with ID: {LoadId}", moduleInfo.Name, loadId);
        
        return loadId;
    }

    private void ThrowModuleInfoValueError(string name)
    {
        throw new InvalidOperationException(
            $"The module is missing assembly information. The '{name}' cannot be retrieved.");
    }
    
    private IModuleInfo CreateModuleInfo(Assembly assembly, bool isInternal)
    {
        var name = assembly.GetCustomAttribute<ModuleIdentifierAttribute>()?.Name;
        var title = assembly.GetCustomAttribute<ModuleTitleAttribute>()?.Title;
        var summary = assembly.GetCustomAttribute<ModuleSummaryAttribute>()?.Summary;
        var version = assembly.GetCustomAttribute<ModuleVersionAttribute>()?.Version;
        var author = assembly.GetCustomAttribute<ModuleAuthorAttribute>()?.Author;
        var dependencies = Array.Empty<IModuleInfo>();

        if (name == null) ThrowModuleInfoValueError("name");
        if (title == null) ThrowModuleInfoValueError("title");
        if (summary == null) ThrowModuleInfoValueError("summary");
        if (version == null) ThrowModuleInfoValueError("version");
        if (author == null) ThrowModuleInfoValueError("author");

        if (isInternal)
        {
            return new InternalModuleInfo
            {
                Name = name!,
                Title = title!,
                Summary = summary!,
                Version = version!,
                Author = author!,
                Dependencies = dependencies
            };
        }

        return new ExternalModuleInfo
        {
            Name = name!,
            Title = title!,
            Summary = summary!,
            Version = version!,
            Author = author!,
            Dependencies = dependencies,
            Directory = new DirectoryInfo(Path.GetDirectoryName(assembly.Location) ?? string.Empty)
        };
    }

    private async Task<Guid> LoadModule(Type moduleType, ModuleAttribute moduleAttr)
    {
        var loadId = Guid.Empty;

        if (moduleAttr.IsInternal)
        {
            loadId = await LoadInternalModule(moduleType, moduleAttr);
        }
        
        _logger.LogDebug("Module {Type}({Module}) was loaded", moduleType, loadId);
        
        return loadId;
    }
    
    private async Task InstallModule(Guid loadId)
    {
        var moduleContext = GetModuleById(loadId);

        await InstallPermissions(moduleContext);
        await TryCallModuleInstall(moduleContext);
        
        _logger.LogDebug("Module {Type}({Module}) was enabled", moduleContext.ModuleClass, loadId);
    }
    
    public async Task EnableModule(Guid loadId)
    {
        var moduleContext = GetModuleById(loadId);

        await EnableControllers(moduleContext);
        await EnableMiddlewares(moduleContext);
        await TryCallModuleEnable(moduleContext);
        
        _logger.LogDebug("Module {Type}({Module}) was installed", moduleContext.ModuleClass, loadId);
    }

    private static ModuleLoadContext CreateLoadContext(Type moduleClass, IModuleInfo moduleInfo, Guid loadId, Container moduleServices)
    {
        var instance = (IEvoScModule)ActivatorUtilities.CreateInstance(moduleServices, moduleClass);
        
        return new ModuleLoadContext
        {
            Instance = instance,
            LoadContext = null,
            LoadId = loadId,
            ModuleClass = moduleClass,
            ModuleInfo = moduleInfo,
            Assembly = moduleClass.Assembly,
            Services = moduleServices,
            Pipelines = new Dictionary<PipelineType, IActionPipeline>
            {
                {PipelineType.ChatRouter, new ActionPipeline()},
                {PipelineType.ControllerAction, new ActionPipeline()}
            },
            Permissions = new List<IPermission>()
        };
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
    
    private Task RegisterPermissions(ModuleLoadContext loadContext)
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

        return Task.CompletedTask;
    }

    private async Task InstallPermissions(IModuleLoadContext moduleContext)
    {
        var identifiedPermissions = new List<IPermission>();
        
        foreach (var permission in moduleContext.Permissions)
        {
            var existingPermission = await _permissions.GetPermission(permission.Name);

            if (existingPermission != null)
            {
                _logger.LogDebug("Wont install permission '{Name}' as it already exists", permission.Name);
                identifiedPermissions.Add(existingPermission);
                continue;
            }

            _logger.LogDebug("Installing permission: {Name}", permission.Name);
            await _permissions.AddPermission(permission);
            var identifiedPermission = await _permissions.GetPermission(permission.Name);

            if (identifiedPermission == null)
            {
                _logger.LogError(
                    "Could not identify permission '{Name}' after installing it. Was it not added to the database?",
                    permission.Name);
                continue;
            }
            
            identifiedPermissions.Add(identifiedPermission);
        }

        moduleContext.Permissions = identifiedPermissions;
    }

    private Task EnableMiddlewares(IModuleLoadContext moduleContext)
    {
        _pipelineManager.AddPipeline(PipelineType.ChatRouter, moduleContext.LoadId,
            moduleContext.Pipelines[PipelineType.ChatRouter]);
        _pipelineManager.AddPipeline(PipelineType.ControllerAction, moduleContext.LoadId,
            moduleContext.Pipelines[PipelineType.ControllerAction]);

        return Task.CompletedTask;
    }

    private Task RegisterMiddlewares(IModuleLoadContext moduleContext)
    {
        foreach (var middlewareType in moduleContext.Assembly.AssemblyTypesWithAttribute<MiddlewareAttribute>())
        {
            var attr = middlewareType.GetCustomAttribute<MiddlewareAttribute>();
            moduleContext.Pipelines[attr!.For].AddComponent(middlewareType, moduleContext.Services);
        }

        return Task.CompletedTask;
    }

    private Task TryCallModuleEnable(IModuleLoadContext moduleContext)
    {
        if (moduleContext.Instance is IToggleable instance)
        {
            return instance.Enable();
        }

        return Task.CompletedTask;
    }

    private Task TryCallModuleInstall(IModuleLoadContext moduleContext)
    {
        if (moduleContext.Instance is IInstallable instance)
        {
            return instance.Install();
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
                        container.Register(intf, type);
                        break;
                    default:
                        throw new ModuleServicesException($"Unsupported lifetime type for module service: {type}");
                }
            }
        }

        return container;
    }

    private async Task RegisterModuleConfig(Assembly assembly, Container container, IModuleInfo moduleInfo)
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

                var store = await CreateModuleConfigStore(moduleInfo.Name, type);
                var config = CreateConfigInstance(type, store);

                if (config == null)
                {
                    throw new InvalidOperationException("Failed to create module config instance.");
                }
                
                container.RegisterInstance(type, config);
            }
        }
    }

    private async Task<IConfigStore> CreateModuleConfigStore(string name, Type configInterface)
    {
        var dbStore = new DatabaseStore(name, configInterface, _db);
        await dbStore.SetupDefaultSettingsAsync();

        return dbStore;
    }
    
    private object? CreateConfigInstance(Type configInterface, IConfigStore store)
    {
        var builder = ReflectionUtils.CreateGenericInstance(typeof(ConfigurationBuilder<>), configInterface);

        if (builder == null)
        {
            throw new InvalidOperationException("Failed to create module config builder.");
        }
        
        ReflectionUtils.CallMethod(builder, "UseConfigStore", store);
        return ReflectionUtils.CallMethod(builder, "Build");
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
                
                var loadId = await LoadModule(moduleType, moduleAttr!);
                await InstallModule(loadId);
                await EnableModule(loadId);

                // only load one module per assembly to avoid conflicts
                return;
            }
        }
    }
}
