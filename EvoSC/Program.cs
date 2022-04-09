using System;
using System.Security.Authentication;
using EvoSC.Contracts;
using EvoSC.Core;
using EvoSC.Core.Configuration;
using EvoSC.Core.Events.GbxEventHandlers;
using EvoSC.Core.PluginHandler;
using EvoSC.Core.Services.Chat;
using EvoSC.Core.Services.Player;
using EvoSC.Interfaces;
using EvoSC.Interfaces.Chat;
using EvoSC.Interfaces.Player;
using EvoSC.Migrations;
using FluentMigrator.Runner;
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
Console.WriteLine("Initializing EvoSC...");

// NLog: Setup NLog for Dependency injection
builder.ConfigureLogging((context, builder) =>
{
    builder.SetMinimumLevel(LogLevel.Trace);
    builder.AddNLog("appsettings.json");
});

var serverConnectionConfig = ConfigurationLoader.LoadServerConnectionConfig();

builder.ConfigureServices(services =>
{
    // FluentMigrator setup
    services.AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddMySql5()
            .WithGlobalConnectionString(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING"))
            .ScanIn(typeof(CreateDatabase).Assembly).For.Migrations())
        .AddLogging(lb => lb.AddFluentMigratorConsole());

    // GbxClient
    services.AddSingleton(new GbxRemoteClient(serverConnectionConfig.Host, serverConnectionConfig.Port));

    // Event Handlers
    services.AddSingleton<IGbxEventHandler, PlayerGbxEventHandler>();
    services.AddSingleton<IGbxEventHandler, ChatGbxEventHandler>();

    // Services
    services.AddSingleton<IPlayerService, PlayerService>();
    services.AddSingleton<IChatService, ChatService>();

    // Plugin loading setup
    services.AddPluginLoaders();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    UpdateDatabase(scope.ServiceProvider);
}

logger.Info($"Connecting to server with IP {serverConnectionConfig.Host} and port {serverConnectionConfig.Port}");
var serverConnection = new ServerConnection(
    app.Services.GetRequiredService<GbxRemoteClient>(), 
    app.Services.GetServices<IGbxEventHandler>()
);
var loggedIn = await serverConnection.Authenticate(serverConnectionConfig);

if (!loggedIn)
{
    throw new AuthenticationException("Could not authenticate to server - login or password is incorrect!");
}

serverConnection.InitializeEventHandlers();

logger.Info("Completed initialization");

var sample = app.Services.GetRequiredService<ISampleService>();
logger.Info(sample.GetName());

app.Run();

static void UpdateDatabase(IServiceProvider serviceProvider)
{
    var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

    runner.MigrateUp();
}
