using EvoSC.Commands;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Application;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Controllers;
using EvoSC.Common.Database;
using EvoSC.Common.Database.Extensions;
using EvoSC.Common.Events;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Logging;
using EvoSC.Common.Middleware;
using EvoSC.Common.Permissions;
using EvoSC.Common.Remote;
using EvoSC.Common.Services;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Extensions;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Util;
using SimpleInjector;

namespace EvoSC.CLI;

public static class BaseStartup
{
    public static void SetupBasePipeline(this IStartupPipeline pipeline, IEvoScBaseConfig config)
    {
        pipeline
            .Services(AppFeature.Logging, s => s.AddEvoScLogging(config.Logging))

            .Services(AppFeature.DatabaseMigrations, s => s.AddEvoScMigrations())

            .Services(AppFeature.Database, s => s
                .DependsOn("DatabaseMigrations")
                .AddEvoScDatabase(config.Database)
            )

            .Services(AppFeature.Events, s => s.AddEvoScEvents())

            .Services(AppFeature.GbxRemoteClient, s => s.AddGbxRemoteClient())

            .Services(AppFeature.Modules, s => s.AddEvoScModules())

            .Services(AppFeature.ControllerManager, s => s.AddEvoScControllers())

            .Services(AppFeature.PlayerManager, s => s
                .Register<IPlayerManagerService, PlayerManagerService>(Lifestyle.Transient)
            )

            .Services(AppFeature.MapsManager, s => s
                .Register<IMapService, MapService>(Lifestyle.Transient)
            )

            .Services(AppFeature.PlayerCache, s => s
                .RegisterSingleton<IPlayerCacheService, PlayerCacheService>()
            )

            .Services(AppFeature.MatchSettings, s => s
                .Register<IMatchSettingsService, MatchSettingsService>(Lifestyle.Transient)
            )

            .Services(AppFeature.Auditing, s => s
                .Register<IAuditService, AuditService>(Lifestyle.Transient)
            )

            .Services(AppFeature.ServicesManager, s => s
                .RegisterSingleton<IServiceContainerManager, ServiceContainerManager>()
            )

            .Services(AppFeature.ChatCommands, s => s.AddEvoScChatCommands())

            .Services(AppFeature.ActionPipelines, s => s.AddEvoScMiddlewarePipelines())

            .Services(AppFeature.Permissions, s => s.AddEvoScPermissions())

            .Services(AppFeature.Manialinks, s => s.AddEvoScManialinks())

            .Action("ActionMigrateDatabase", MigrateDatabase)

            .Action("ActionSetupControllerManager", SetupControllerManager)

            .ActionAsync("ActionSetupModules", SetupModules)

            .Action("ActionInitializeEventManager", s => s
                .DependsOn("Events")
                .GetInstance<IEventManager>()
            )

            .Action("ActionInitializePlayerCache", s => s
                .DependsOn("PlayerCache")
                .GetInstance<IPlayerCacheService>()
            )

            .Action("ActionInitializeManialinkInteractionHandler", s => s
                .DependsOn("ActionInitializeEventManager", "ActionInitializePlayerCache")
            )
            
            .ActionAsync("InitializeGbxRemoteConnection", SetupGbxRemoteConnection)
            
            .ActionAsync("ActionEnableModules", EnableModules)
            
            .ActionAsync("ActionInitializeTemplates", InitializeTemplates);
    }
    
    private static async Task InitializeTemplates(ServicesBuilder s)
    {
        s.DependsOn("Manialinks");
    
        var maniaLinks = s.GetInstance<IManialinkManager>();
        await maniaLinks.PreprocessAllAsync();
    }

    private static async Task EnableModules(ServicesBuilder s)
    {
        await s.GetInstance<IManialinkManager>().AddDefaultTemplatesAsync();
        await s.GetInstance<IModuleManager>().EnableModulesAsync();
    }

    private static void MigrateDatabase(ServicesBuilder s)
    {
        s.DependsOn("DatabaseMigrations");
        using var scope = new Scope(s);
        var manager = scope.GetInstance<IMigrationManager>();
    
        // main migrations
        manager.MigrateFromAssembly(typeof(MigrationManager).Assembly);
    
        // internal modules
        // manager.RunInternalModuleMigrations();
    }

    private static void SetupControllerManager(ServicesBuilder s)
    {
        s.DependsOn("ControllerManager", "ActionPipelines");

        var controllers = s.GetInstance<IControllerManager>();
        controllers.AddControllerActionRegistry(s.GetInstance<IEventManager>());
        controllers.AddControllerActionRegistry(s.GetInstance<IChatCommandManager>());
        controllers.AddControllerActionRegistry(s.GetInstance<IManialinkActionManager>());
    
        var pipelineManager = s.GetInstance<IActionPipelineManager>();
        pipelineManager.UseEvoScCommands(s);
        pipelineManager.UseEvoScManialinks(s);
    }

    private static async Task SetupModules(ServicesBuilder s)
    {
        s.DependsOn("Modules", "Config");

        var modules = s.GetInstance<IModuleManager>();
        var config = s.GetInstance<IEvoScBaseConfig>();

        // await modules.LoadInternalModulesAsync();

        var dirs = config.Modules.ModuleDirectories;
        var externalModules = new SortedModuleCollection<IExternalModuleInfo>();
        foreach (var dir in dirs)
        {
            if (!Directory.Exists(dir))
            {
                continue;
            }

            ModuleDirectoryUtils.FindModulesFromDirectory(dir, externalModules);
        }

        externalModules.SetIgnoredDependencies(modules.LoadedModules.Select(m => m.ModuleInfo.Name));
        await modules.LoadAsync(externalModules);
    }

    private static async Task SetupGbxRemoteConnection(ServicesBuilder s)
    {
        s.DependsOn(
            "ActionInitializeEventManager",
            "ActionInitializePlayerCache",
            "ActionInitializeManialinkInteractionHandler",
            "GbxRemoteClient"
        );

        var serverClient = s.GetInstance<IServerClient>();
        s.GetInstance<IServerCallbackHandler>();
        s.GetInstance<IRemoteChatRouter>();
        await serverClient.StartAsync(CancellationToken.None);
    }
}
