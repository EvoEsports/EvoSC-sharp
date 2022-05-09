using System;
using EvoSC.Core.Plugins;
using EvoSC.Core.Services;
using EvoSC.Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using EvoSC.Core;
using EvoSC.Core.Configuration;
using EvoSC.Core.Events.Callbacks;
using EvoSC.Core.Events.GbxEventHandlers;
using EvoSC.Core.Services.Chat;
using EvoSC.Core.Services.Player;
using EvoSC.Interfaces;
using EvoSC.Interfaces.Chat;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var builder = Host.CreateDefaultBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("Initializing EvoSC...");

// NLog: Setup NLog for Dependency injection
builder.ConfigureLogging((context, builder) =>
{
    builder.ClearProviders();
    builder.AddNLog("nlog.config")
        .SetMinimumLevel(LogLevel.Trace);
});

var serverConnectionConfig = Config.GetDedicatedConfig();
var theme = Config.GetTheme();
var dbConfig = Config.GetDatabaseConfig();
var connectionString = dbConfig.GetConnectionString();

builder.ConfigureServices(services =>
{
    services.AddDbContext<DatabaseContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

    // GbxClient
    services.AddSingleton(new GbxRemoteClient(serverConnectionConfig.Host, serverConnectionConfig.Port));

    // Event Handlers
    services.AddSingleton<IGbxEventHandler, PlayerGbxEventHandler>();
    services.AddSingleton<IGbxEventHandler, ChatGbxEventHandler>();
    services.AddSingleton<Theme>(theme);


    // Callbacks
    services.AddSingleton<IPlayerCallbacks, PlayerCallbacks>();

    // Services
    services.AddSingleton<IPlayerService, PlayerService>();
    services.AddSingleton<IChatService, ChatService>();

    // Load plugins
    PluginFactory.Instance.LoadPlugins(services);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.MigrateAsync();
}

logger.Info($"Connecting to server with IP {serverConnectionConfig.Host} and port {serverConnectionConfig.Port}");
var serverConnection = new ServerConnection(
    app.Services.GetRequiredService<GbxRemoteClient>(),
    app.Services.GetServices<IGbxEventHandler>(),
    app.Services.GetService<IPlayerService>()
);

serverConnection.InitializeEventHandlers();


await serverConnection.ConnectToServer(serverConnectionConfig);

logger.Info("Completed initialization");

//var sample = app.Services.GetRequiredService<ISampleService>();
//logger.Info(sample.GetName());
var module = app.Services.GetService<IPlugin>();
module?.HandleEvents(app.Services.GetRequiredService<IPlayerCallbacks>());
Subscribe(app.Services.GetRequiredService<IPlayerCallbacks>());

app.Run();

void Subscribe(IPlayerCallbacks playerCallbacks)
{
    playerCallbacks.PlayerConnect += PlayerCallbacks_PlayerConnect;
    playerCallbacks.PlayerDisconnect += PlayerCallbacks_PlayerDisconnect;
}

void PlayerCallbacks_PlayerConnect(object sender, EvoSC.Core.Events.Callbacks.Args.PlayerConnectEventArgs e)
{
    logger.Info("A player has connected");
}

void PlayerCallbacks_PlayerDisconnect(object sender, EvoSC.Core.Events.Callbacks.Args.PlayerDisconnectEventArgs e)
{
    logger.Info("A player has disconnected");
}
