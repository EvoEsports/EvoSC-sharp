using System;
using EvoSC.Contracts;
using EvoSC.Core.PluginHandler;
using EvoSC.Migrations;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var builder = WebApplication.CreateBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("Initializing EvoSC...");
Console.WriteLine("Initializing EvoSC...");

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

// FluentMigrator setup
builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddMySql5()
        .WithGlobalConnectionString(Environment.GetEnvironmentVariable("DOTNET_CONNECTION_STRING"))
        .ScanIn(typeof(CreateDatabase).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

// Plugin loading setup
builder.Services.AddPluginLoaders();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    UpdateDatabase(scope.ServiceProvider);
}

logger.Info("Completed initialization");
Console.WriteLine("Completed initialization");

var sample = app.Services.GetRequiredService<ISampleService>();
logger.Info(sample.GetName());

app.Run();

static void UpdateDatabase(IServiceProvider serviceProvider)
{
    var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

    runner.MigrateUp();
}
