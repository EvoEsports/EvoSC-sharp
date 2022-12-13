using System.CommandLine;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EvoSC.Commands;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Config;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Controllers;
using EvoSC.Common.Database;
using EvoSC.Common.Events;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Logging;
using EvoSC.Common.Middleware;
using EvoSC.Common.Permissions;
using EvoSC.Common.Remote;
using EvoSC.Common.Services;
using EvoSC.Modules;
using EvoSC.Modules.Extensions;
using EvoSC.Modules.Interfaces;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace EvoSC;

public class Application : IEvoSCApplication
{
    private readonly string[] _args;
    private Container _services;
    private readonly bool _isDebug;
    private ILogger<Application> _logger;

    private readonly CancellationTokenSource _runningToken = new();

    public CancellationToken MainCancellationToken => _runningToken.Token;
    public Container Services => _services;

    public Application(string[] args)
    {
        _args = args;
        _isDebug = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development";

        ConfigureServiceContainer();
    }

    private void ConfigureServiceContainer()
    {
        _services = new Container();
        _services.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        _services.Options.EnableAutoVerification = false;
        _services.Options.ResolveUnregisteredConcreteTypes = true;
        _services.Options.SuppressLifestyleMismatchVerification = true;
        _services.Options.UseStrictLifestyleMismatchBehavior = false;
    }

    public async Task RunAsync()
    {
        await SetupApplication();

        // wait indefinitely
        WaitHandle.WaitAll(new[] {_runningToken.Token.WaitHandle});
    }

    public async Task ShutdownAsync()
    {
        var serverClient = _services.GetInstance<IServerClient>();
        await serverClient.StopAsync(_runningToken.Token);
        
        // cancel the token to stop the application itself
        _runningToken.Cancel();
    }

    public async Task SetupApplication()
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
    }

    private void SetupServices()
    {
        var config = _services.AddEvoScConfig();

        _services.AddEvoScLogging(config.Logging);
        
        _services.AddEvoScMigrations();
        _services.AddEvoScDatabase(config.Database);
        
        _services.AddGbxRemoteClient();
        _services.AddEvoScEvents();
        _services.AddEvoScModules();
        _services.AddEvoScControllers();
        _services.AddEvoScCommonServices();
        _services.AddEvoScChatCommands();
        _services.AddEvoScMiddlewarePipelines();
        _services.AddEvoScPermissions();

        _services.RegisterInstance<IEvoSCApplication>(this);
        
        _logger = _services.GetInstance<ILogger<Application>>();
    }

    private void MigrateDatabase()
    {
        using var scope = new Scope(_services);
        var manager = scope.GetInstance<IMigrationManager>();
        
        // main migrations
        manager.MigrateFromAssembly(typeof(MigrationManager).Assembly);
        
        // internal modules
        manager.RunInternalModuleMigrations();
    }
    
    private void SetupControllerManager()
    {
        var controllers = _services.GetInstance<IControllerManager>();
        controllers.AddControllerActionRegistry(_services.GetInstance<IEventManager>());
        controllers.AddControllerActionRegistry(_services.GetInstance<IChatCommandManager>());
        
        var pipelineManager = _services.GetInstance<IActionPipelineManager>();
        pipelineManager.UseEvoScCommands(_services);
    }

    private async Task SetupModules()
    {
        var modules = _services.GetInstance<IModuleManager>();

        await modules.LoadInternalModules();
    }

    private async Task StartBackgroundServices()
    {
        _logger.LogDebug("Starting background services");

        // initialize event manager before anything else
        _services.GetInstance<IEventManager>();

        // connect to the dedicated server and setup callbacks and chat router
        var serverClient = _services.GetInstance<IServerClient>();
        _services.GetInstance<IServerCallbackHandler>();
        _services.GetInstance<IRemoteChatRouter>();
        await serverClient.StartAsync(_runningToken.Token);

        // setup command handler
        _services.GetInstance<ICommandInteractionHandler>();
    }
}
