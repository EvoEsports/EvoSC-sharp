using EvoSC.Commands;
using EvoSC.Common.Application;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Controllers;
using EvoSC.Common.Database;
using EvoSC.Common.Database.Extensions;
using EvoSC.Common.Events;
using EvoSC.Common.Interfaces;
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
using GBX.NET;
using GBX.NET.LZO;
using SimpleInjector;

namespace EvoSC.CLI;

public static class CliStartup
{
    public static void SetupBasePipeline(this IStartupPipeline pipeline, IEvoScBaseConfig config)
    {
        pipeline
            // Set up service container
            .Services(AppFeature.Config, s =>
                    s.RegisterInstance(config)
                , "CliContext")

            .Services(AppFeature.Logging, s => s.AddEvoScLogging(config.Logging))

            .Services(AppFeature.DatabaseMigrations, s => s
                    .AddEvoScMigrations()
                , "Config")

            .Services(AppFeature.Database, s => s
                    .AddEvoScDatabase(config.Database)
                , "DatabaseMigrations", "Logging", "ActionMigrateDatabase")

            .Services(AppFeature.Events, s => s
                    .AddEvoScEvents()
                , "Logging", "ControllerManager", "ActionInitializeEventManager", "Application")

            .Services(AppFeature.GbxRemoteClient, s => s
                    .AddGbxRemoteClient()
                , "Logging", "Config", "Events", "ActionPipelines", "InitializeGbxRemoteConnection", "Themes")

            .Services(AppFeature.Modules, s => s
                    .AddEvoScModules()
                , "Logging", "Config", "ControllerManager", "ServicesManager", "ActionPipelines", "Permissions",
                "Database", "Manialinks")

            .Services(AppFeature.ControllerManager, s => s
                    .AddEvoScControllers()
                , "Logging")

            .Services(AppFeature.PlayerManager, s => s
                    .Register<IPlayerManagerService, PlayerManagerService>(Lifestyle.Transient)
                , "Logging", "Database", "PlayerCache", "GbxRemoteClient")

            .Services(AppFeature.MapsManager, s => s
                    .Register<IMapService, MapService>(Lifestyle.Transient)
                , "Logging", "Config", "Database", "PlayerManager", "GbxRemoteClient")

            .Services(AppFeature.PlayerCache, s => s
                    .RegisterSingleton<IPlayerCacheService, PlayerCacheService>()
                , "Logging", "Events", "Database", "GbxRemoteClient", "ActionInitializePlayerCache")
            
            .Services(AppFeature.MatchSettings, s =>
            {
                s.Register<IMatchSettingsService, MatchSettingsService>(Lifestyle.Transient);
                s.RegisterSingleton<IMatchSettingsTrackerService, MatchSettingsTrackerService>();
                s.RegisterSingleton<IMatchSettingsMaplistUpdateService, MatchSettingsMaplistUpdateService>();
            }, "Logging", "GbxRemoteClient", "Config", "Events")

            .Services(AppFeature.Auditing, s => s
                    .Register<IAuditService, AuditService>(Lifestyle.Transient)
                , "Logging", "Database")

            .Services(AppFeature.ServicesManager, s => s
                    .RegisterSingleton<IServiceContainerManager, ServiceContainerManager>()
                , "Logging")

            .Services(AppFeature.Chat, s => s
                .Register<IChatService, ChatService>(Lifestyle.Transient)
                , "GbxRemoteClient", "Themes")
            
            .Services(AppFeature.ChatCommands, s => s
                    .AddEvoScChatCommands()
                , "Logging", "PlayerManager")

            .Services(AppFeature.ActionPipelines, s => s
                    .AddEvoScMiddlewarePipelines()
                , "Logging")

            .Services(AppFeature.Permissions, s => s
                    .AddEvoScPermissions()
                , "Database")

            .Services(AppFeature.Manialinks, s => s
                    .AddEvoScManialinks()
                , "Logging", "Events", "PlayerManager", "ControllerManager", "ActionPipelines", "GbxRemoteClient",
                "ActionInitializeTemplates")

            .Services(AppFeature.Themes, s => s.AddEvoScThemes())

            // initialization of features
            .Action("ActionSetupLibraries", SetupLibraries)
            
            .Action("ActionMigrateDatabase", MigrateDatabase)

            .Action("ActionInitializeEventManager", s => s
                .GetInstance<IEventManager>()
            )

            .AsyncAction("InitializeCaches", InitializeCachesAsync)

            .Action("ActionInitializeManialinkInteractionHandler", s => s
                    .GetInstance<IManialinkInteractionHandler>()
                , "ActionInitializeEventManager", "ActionInitializePlayerCache")

            .AsyncAction("InitializeGbxRemoteConnection", SetupGbxRemoteConnectionAsync
                , "ActionInitializeEventManager", "ActionInitializePlayerCache",
                "ActionInitializeManialinkInteractionHandler")

            .AsyncAction("ActionInitializeTemplates", InitializeTemplatesAsync);
    }
    
    public static void SetupLibraries(ServicesBuilder s)
    {
        Gbx.LZO = new Lzo();
    }
    
    /// <summary>
    /// Initialize and preprocess all registered templates.
    /// </summary>
    /// <param name="s"></param>
    private static async Task InitializeTemplatesAsync(ServicesBuilder s)
    {
        var maniaLinks = s.GetInstance<IManialinkManager>();
        await maniaLinks.PreprocessAllAsync();
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
    }
    
    /// <summary>
    /// Creates the singleton instances of caches and runs
    /// initialization methods to make them ready.
    /// </summary>
    /// <param name="s"></param>
    private static async Task InitializeCachesAsync(ServicesBuilder s)
    {
        var msTrackerService = s.GetInstance<IMatchSettingsTrackerService>();
        await msTrackerService.SetDefaultMatchSettingsAsync();
        
        s.GetInstance<IPlayerCacheService>();
    }

    /// <summary>
    /// Connect to XMLRPC and initialize server callback and chat router.
    /// </summary>
    /// <param name="s"></param>
    private static async Task SetupGbxRemoteConnectionAsync(ServicesBuilder s)
    {
        var serverClient = s.GetInstance<IServerClient>();
        s.GetInstance<IServerCallbackHandler>();
        s.GetInstance<IRemoteChatRouter>();
        await serverClient.StartAsync(CancellationToken.None);
    }
}
