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
using EvoSC.Common.Themes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Extensions;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Util;
using GBX.NET;
using GBX.NET.LZO;
using SimpleInjector;

namespace EvoSC;

public static class ApplicationSetup
{
    public static void SetupPipeline(this IStartupPipeline pipeline, IEvoScBaseConfig config)
    {
        pipeline
                // Setting up service container
            .Services(AppFeature.Config, s => s.RegisterInstance(config))

            .Services(AppFeature.Logging, s => s.AddEvoScLogging(config.Logging))

            .Services(AppFeature.DatabaseMigrations, s => s.AddEvoScMigrations())

            .Services(AppFeature.Database, s => s
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

            .Services(AppFeature.MatchSettings, s =>
            {
                s.Register<IMatchSettingsService, MatchSettingsService>(Lifestyle.Transient);
                s.RegisterSingleton<IMatchSettingsTrackerService, MatchSettingsTrackerService>();
                s.RegisterSingleton<IMatchSettingsMaplistUpdateService, MatchSettingsMaplistUpdateService>();
            })

            .Services(AppFeature.Auditing, s => s
                .Register<IAuditService, AuditService>(Lifestyle.Transient)
            )

            .Services(AppFeature.ServicesManager, s => s
                .RegisterSingleton<IServiceContainerManager, ServiceContainerManager>()
            )
                
            .Services(AppFeature.Chat, s => s
                .Register<IChatService, ChatService>(Lifestyle.Transient)
            )

            .Services(AppFeature.ChatCommands, s => s.AddEvoScChatCommands())

            .Services(AppFeature.ActionPipelines, s => s.AddEvoScMiddlewarePipelines())

            .Services(AppFeature.Permissions, s => s.AddEvoScPermissions())

            .Services(AppFeature.Manialinks, s => s.AddEvoScManialinks())
                
            .Services(AppFeature.Themes, s => s.AddEvoScThemes())

            .Action("ActionSetupLibraries", SetupLibraries)
                
                // initialize the application
            .Action("ActionMigrateDatabase", MigrateDatabase)

            .Action("ActionSetupControllerManager", SetupControllerManager)

            .AsyncAction("ActionSetupModules", SetupModulesAsync)

            .Action("ActionInitializeEventManager", s => s
                .GetInstance<IEventManager>()
            )

            .AsyncAction("InitializeCaches", InitializeCachesAndTrackersAsync)

            .Action("ActionInitializeManialinkInteractionHandler", s => s
                .GetInstance<IManialinkInteractionHandler>()
            )

            .AsyncAction("InitializeGbxRemoteConnection", SetupGbxRemoteConnectionAsync)

            .AsyncAction("ActionEnableModules", EnableModulesAsync)

            .AsyncAction("ActionInitializeTemplates", InitializeTemplatesAsync, "Manialinks");
    }

    public static void SetupLibraries(ServicesBuilder s)
    {
        Gbx.LZO = new Lzo();
    }
    
    /// <summary>
    /// Preprocesses manialinks.
    /// </summary>
    /// <param name="s"></param>
    private static async Task InitializeTemplatesAsync(ServicesBuilder s)
    {
        var maniaLinks = s.GetInstance<IManialinkManager>();
        await maniaLinks.PreprocessAllAsync();
    }

    /// <summary>
    /// Enables internal and external modules.
    /// </summary>
    /// <param name="s"></param>
    private static async Task EnableModulesAsync(ServicesBuilder s)
    {
        await s.GetInstance<IManialinkManager>().AddDefaultTemplatesAsync();
        await s.GetInstance<IModuleManager>().EnableModulesAsync();
    }

    /// <summary>
    /// Run database migrations.
    /// </summary>
    /// <param name="s"></param>
    private static void MigrateDatabase(ServicesBuilder s)
    {
        using var scope = new Scope(s);
        var manager = scope.GetInstance<IMigrationManager>();
    
        // main migrations
        manager.MigrateFromAssembly(typeof(MigrationManager).Assembly);
    
        // internal modules
        manager.RunInternalModuleMigrations();
    }

    /// <summary>
    /// Initialize controller registries and set up command and manialink controller managers.
    /// </summary>
    /// <param name="s"></param>
    private static void SetupControllerManager(ServicesBuilder s)
    {
        var controllers = s.GetInstance<IControllerManager>();
        controllers.AddControllerActionRegistry(s.GetInstance<IEventManager>());
        controllers.AddControllerActionRegistry(s.GetInstance<IChatCommandManager>());
        controllers.AddControllerActionRegistry(s.GetInstance<IManialinkActionManager>());
    
        var pipelineManager = s.GetInstance<IActionPipelineManager>();
        pipelineManager.UseEvoScCommands(s);
        pipelineManager.UseEvoScManialinks(s);
    }

    /// <summary>
    /// Load internal and external modules.
    /// </summary>
    /// <param name="s"></param>
    private static async Task SetupModulesAsync(ServicesBuilder s)
    {
        var modules = s.GetInstance<IModuleManager>();
        var config = s.GetInstance<IEvoScBaseConfig>();

        await modules.LoadInternalModulesAsync();

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

    /// <summary>
    /// Connect to XMLRPC and set up callback handler and chat router.
    /// </summary>
    /// <param name="s"></param>
    private static async Task SetupGbxRemoteConnectionAsync(ServicesBuilder s)
    {
        var serverClient = s.GetInstance<IServerClient>();
        s.GetInstance<IServerCallbackHandler>();
        s.GetInstance<IRemoteChatRouter>();
        await serverClient.StartAsync(CancellationToken.None);
    }
    
    /// <summary>
    /// Creates the singleton instances of caches and runs
    /// initialization methods to make them ready.
    /// </summary>
    /// <param name="s"></param>
    private static async Task InitializeCachesAndTrackersAsync(ServicesBuilder s)
    {
        var msTrackerService = s.GetInstance<IMatchSettingsTrackerService>();
        await msTrackerService.SetDefaultMatchSettingsAsync();
        
        s.GetInstance<IPlayerCacheService>();
        s.GetInstance<IMatchSettingsMaplistUpdateService>();
    }
}
