using System;
using EvoSC.Core.Plugins;
using EvoSC.Core.Services;
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
    builder.AddNLog("appsettings.json")
        .SetMinimumLevel(LogLevel.Trace);
});

builder.ConfigureServices(services =>
{
    // FluentMigrator setup
    /*services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddMySql5()
        .WithGlobalConnectionString(Environment.GetEnvironmentVariable("DOTNET_CONNECTION_STRING"))
        .ScanIn(typeof(CreateDatabase).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());*/

    // Initialize plugin factory
    PluginFactory.Instance.LoadPlugins(services);
});

var app = builder.Build();

/*using (var scope = app.Services.CreateScope())
{
    UpdateDatabase(scope.ServiceProvider);
}*/

logger.Info("Completed initialization");
Console.WriteLine("Completed initialization");

var sample = app.Services.GetRequiredService<ISampleService>();
logger.Info(sample.GetName());

app.Run();

/*static void UpdateDatabase(IServiceProvider serviceProvider)
{
    var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

    runner.MigrateUp();
}*/
