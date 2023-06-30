using System.Reflection.PortableExecutable;
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

public static class CliStartup
{
    public static void SetupBasePipeline(this IStartupPipeline pipeline, IEvoScBaseConfig config)
    {
        pipeline
            .Services(AppFeature.Config, s => s.RegisterInstance(config))

            .Services(AppFeature.Logging, s => s.AddEvoScLogging(config.Logging))

            .Services(AppFeature.DatabaseMigrations, s => s.AddEvoScMigrations())

            .Services(AppFeature.Database, s => s
                    .AddEvoScDatabase(config.Database)
                , "DatabaseMigrations")

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

            .Action("ActionMigrateDatabase", MigrateDatabase, "DatabaseMigrations")

            .Action("ActionInitializeEventManager", s => s
                    .GetInstance<IEventManager>()
                , "Events")

            .Action("ActionInitializePlayerCache", s => s
                    .GetInstance<IPlayerCacheService>()
                , "PlayerCache")

            .Action("ActionInitializeManialinkInteractionHandler", s => s
                    .GetInstance<IManialinkInteractionHandler>()
                , "ActionInitializeEventManager", "ActionInitializePlayerCache")

            .ActionAsync("InitializeGbxRemoteConnection", SetupGbxRemoteConnection,
                "ActionInitializeEventManager",
                "ActionInitializePlayerCache",
                "ActionInitializeManialinkInteractionHandler",
                "GbxRemoteClient")

            .ActionAsync("ActionInitializeTemplates", InitializeTemplates, "Manialinks");
    }
    
    private static async Task InitializeTemplates(ServicesBuilder s)
    {
        var maniaLinks = s.GetInstance<IManialinkManager>();
        await maniaLinks.PreprocessAllAsync();
    }

    private static void MigrateDatabase(ServicesBuilder s)
    {
        using var scope = new Scope(s);
        var manager = scope.GetInstance<IMigrationManager>();
    
        // main migrations
        manager.MigrateFromAssembly(typeof(MigrationManager).Assembly);
    }

    private static async Task SetupGbxRemoteConnection(ServicesBuilder s)
    {
        var serverClient = s.GetInstance<IServerClient>();
        s.GetInstance<IServerCallbackHandler>();
        s.GetInstance<IRemoteChatRouter>();
        await serverClient.StartAsync(CancellationToken.None);
    }
}
