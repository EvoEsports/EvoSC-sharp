﻿using System;
using System.IO;
using System.Threading.Tasks;
using EvoSC.Core;
using EvoSC.Core.Configuration;
using EvoSC.Core.Events.Callbacks;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Core.Events.GbxEventHandlers;
using EvoSC.Core.Plugins;
using EvoSC.Core.Plugins.Abstractions;
using EvoSC.Core.Services.Chat;
using EvoSC.Core.Services.Commands;
using EvoSC.Core.Services.Players;
using EvoSC.Core.Services.UI;
using EvoSC.Domain;
using EvoSC.Interfaces;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Commands;
using EvoSC.Interfaces.Players;
using EvoSC.Interfaces.UI;
using GbxRemoteNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

var builder = Host.CreateDefaultBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("Initializing EvoSC...");

// NLog: Setup NLog for Dependency injection
builder.ConfigureLogging((context, builder) =>
{
    builder.ClearProviders();
    builder.AddNLog("nlog.config");
    builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
});

var serverConnectionConfig = Config.GetDedicatedConfig();
var theme = Config.GetTheme();
var dbConfig = Config.GetDatabaseConfig();
var connectionString = dbConfig.GetConnectionString();

builder.ConfigureServices((builder, services) =>
{
    services.AddDbContext<DatabaseContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

    // GbxClient
    services.AddSingleton(new GbxRemoteClient(serverConnectionConfig.Host, serverConnectionConfig.Port));

    // Event Handlers
    services.AddSingleton<IGbxEventHandler, PlayerGbxEventHandler>();
    services.AddSingleton<IGbxEventHandler, ChatGbxEventHandler>();
    services.AddSingleton<IGbxEventHandler, ManialinkPageGbxEventHandler>();
    services.AddSingleton(theme);

    // Callbacks
    services.AddSingleton<IPlayerCallbacks, PlayerCallbacks>();
    services.AddSingleton<IChatCallbacks, ChatCallbacks>();
    services.AddSingleton<IManialinkPageCallbacks, ManialinkPageCallbacks>();

    // Services
    services.AddSingleton<IPlayerService, PlayerService>();
    services.AddSingleton<IChatService, ChatService>();
    services.AddSingleton<IUiService, UiService>();
    services.AddSingleton<IChatCommandsService, ChatCommandsService>();
    services.AddScoped<IPermissionsService, PermissionsService>();
});

// plugins
builder.UsePlugins(config =>
{
    config.PluginsDir = Path.GetFullPath(Environment.CurrentDirectory + "/ext_plugins");
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
    app.Services.GetService<IPlayerService>());

serverConnection.InitializeEventHandlers();

await serverConnection.ConnectToServer(serverConnectionConfig);

logger.Info("Completed initialization");

Subscribe(app.Services.GetRequiredService<IPlayerCallbacks>());

Task.Run(async () =>
{
    await Task.Delay(2000);
    await app.StopAsync();
});

app.Run();

void Subscribe(IPlayerCallbacks playerCallbacks)
{
    playerCallbacks.PlayerConnect += PlayerCallbacks_PlayerConnect;
    playerCallbacks.PlayerDisconnect += PlayerCallbacks_PlayerDisconnect;
}

void PlayerCallbacks_PlayerConnect(object sender, PlayerConnectEventArgs e)
{
    logger.Info("A player has connected");
}

void PlayerCallbacks_PlayerDisconnect(object sender, PlayerDisconnectEventArgs e)
{
    logger.Info("A player has disconnected");
}
