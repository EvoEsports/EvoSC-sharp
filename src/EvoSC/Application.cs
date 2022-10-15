
using System.Diagnostics;
using System.Runtime.InteropServices;
using EvoSC.Common.Config;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Controllers;
using EvoSC.Common.Database;
using EvoSC.Common.Events;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Logging;
using EvoSC.Common.Remote;
using EvoSC.Modules;
using EvoSC.Modules.Extensions;
using EvoSC.Modules.Official.Player;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EvoSC;

public class Application : IEvoSCApplication
{
    private readonly string[] _args;
    private IServiceCollection _services = new ServiceCollection();
    private IServiceProvider _serviceProvider;
    private bool _isDebug;
    private ILogger<Application> _logger;

    private readonly CancellationTokenSource _runningToken = new();
    
    public CancellationToken MainCancellationToken => _runningToken.Token;
    
    public Application(string[] args)
    {
        _args = args;
        _isDebug = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development";
    }

    public async Task RunAsync()
    {
        var sw = new Stopwatch();
        sw.Start();

        SetupServices();
        MigrateDatabase();
        SetupControllerManager();
        await SetupModules();
        await StartBackgroundServices();
        
        sw.Stop();
        
        _logger.LogDebug("Startup time: {Time}ms", sw.ElapsedMilliseconds);

        // wait indefinitely
        WaitHandle.WaitAll(new[] {_runningToken.Token.WaitHandle});
    }

    public async Task ShutdownAsync()
    {
        var serverClient = _serviceProvider.GetRequiredService<IServerClient>();
        await serverClient.StopAsync(_runningToken.Token);
        
        // cancel the token to stop the application itself
        _runningToken.Cancel();
    }

    private void SetupServices()
    {
        var config = _services.AddEvoScConfig();

        var dbConfig = config.Get<DatabaseConfig>(EvoScConfig.DatabaseConfigKey);
        _services.AddEvoScLogging(config.Get<LoggingConfig>(EvoScConfig.LoggingConfigKey));
        
        _services.AddEvoScMigrations();
        _services.AddEvoScDatabase(dbConfig);
        
        _services.AddGbxRemoteClient();
        _services.AddEvoScEvents();
        _services.AddEvoScModules();
        _services.AddEvoScControllers();

        _services.AddSingleton<IEvoSCApplication>(this);
        _serviceProvider = _services.BuildServiceProvider();
        
        _logger = _serviceProvider.GetRequiredService<ILogger<Application>>();
    }

    private void MigrateDatabase()
    {
        using var scope = _serviceProvider.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<IMigrationManager>();
        
        // main migrations
        manager.MigrateFromAssembly(typeof(MigrationManager).Assembly);
        
        // internal modules
        manager.RunInternalModuleMigrations();
    }
    
    private void SetupControllerManager()
    {
        var controllers = _serviceProvider.GetRequiredService<IControllerManager>();
        
        controllers.AddControllerActionRegistry(_serviceProvider.GetRequiredService<IEventManager>());
    }
    
    private async Task SetupModules()
    {
        var modules = _serviceProvider.GetRequiredService<IModuleManager>();

        modules.LoadInternalModules();
    }

    private async Task StartBackgroundServices()
    {
        _logger.LogDebug("Starting background services");
        
        // initialize event manager before anything else
        _serviceProvider.GetRequiredService<IEventManager>();
        
        // connect to the dedicated server and setup callbacks
        var serverClient = _serviceProvider.GetRequiredService<IServerClient>();
        var serverCallbacks = _serviceProvider.GetRequiredService<IServerCallbackHandler>();
        await serverClient.StartAsync(_runningToken.Token);
    }
}
