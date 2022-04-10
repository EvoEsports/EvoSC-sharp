using System;
using EvoSC.Core.Plugins;
using EvoSC.Core.Services;
using EvoSC.Domain;
using Microsoft.EntityFrameworkCore;
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
    services.AddDbContext<DatabaseContext>(options => options.UseMySql(Environment.GetEnvironmentVariable("DOTNET_CONNECTION_STRING"), ServerVersion.AutoDetect(Environment.GetEnvironmentVariable("DOTNET_CONNECTION_STRING"))));
    // Load plugins
    PluginFactory.Instance.LoadPlugins(services);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.MigrateAsync();
}

logger.Info("Completed initialization");
Console.WriteLine("Completed initialization");

var sample = app.Services.GetRequiredService<ISampleService>();
logger.Info(sample.GetName());

app.Run();
