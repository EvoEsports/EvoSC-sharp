using System.Diagnostics;
using EvoSC.CLI;
using EvoSC.Commands;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Application;
using EvoSC.Common.Config;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace EvoSC;

public sealed class Application : IEvoSCApplication, IDisposable
{
    private readonly bool _isDebug;
    private ILogger<Application> _logger;

    private readonly IEvoScBaseConfig _config;

    private readonly CancellationTokenSource _runningToken = new();

    public IStartupPipeline StartupPipeline { get; }

    public CancellationToken MainCancellationToken => _runningToken.Token;
    public Container Services => StartupPipeline.ServiceContainer;

    public Application(IEvoScBaseConfig config)
    {
        _config = config;
        _isDebug = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development";
        StartupPipeline = new StartupPipeline(_config);

        StartupPipeline.Services("Application", s => s.RegisterInstance<IEvoSCApplication>(this));
        StartupPipeline.Services("Config", s => s.RegisterInstance(_config));

        ConfigureServiceContainer();
    }

    private void ConfigureServiceContainer()
    {
        StartupPipeline.ServiceContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        StartupPipeline.ServiceContainer.Options.EnableAutoVerification = false;
        StartupPipeline.ServiceContainer.Options.ResolveUnregisteredConcreteTypes = true;
        StartupPipeline.ServiceContainer.Options.SuppressLifestyleMismatchVerification = true;
        StartupPipeline.ServiceContainer.Options.UseStrictLifestyleMismatchBehavior = false;
    }

    public async Task RunAsync()
    {
        // await SetupApplicationAsync();
        
        StartupPipeline.SetupPipeline(_config);
        await StartupPipeline.ExecuteAllAsync();

        // wait indefinitely
        WaitHandle.WaitAll(new[] {_runningToken.Token.WaitHandle});
    }
    
    public async Task ShutdownAsync()
    {
        var serverClient = Services.GetInstance<IServerClient>();
        await serverClient.StopAsync(_runningToken.Token);
        
        // cancel the token to stop the application itself
        _runningToken.Cancel();
    }

    /* public async Task ShutdownAsync()
    {
        var serverClient = _services.GetInstance<IServerClient>();
        await serverClient.StopAsync(_runningToken.Token);
        
        // cancel the token to stop the application itself
        _runningToken.Cancel();
    }

    private async Task SetupApplicationAsync()
    {
        var sw = new Stopwatch();
        sw.Start();

        SetupServices();
        MigrateDatabase();
        SetupControllerManager();
        await SetupModulesAsync();
        await StartBackgroundServicesAsync();
        await EnableModulesAsync();
        await InitializeTemplatesAsync();
        
        sw.Stop();
        
        _logger.LogDebug("Startup time: {Time}ms", sw.ElapsedMilliseconds);
    }

    private void SetupServices()
    {
        _services.AddEvoScLogging(_config.Logging);
        
        _services.AddEvoScMigrations();
        _services.AddEvoScDatabase(_config.Database);
        
        _services.AddGbxRemoteClient();
        _services.AddEvoScEvents();
        _services.AddEvoScModules();
        _services.AddEvoScControllers();
        _services.AddEvoScCommonServices();
        _services.AddEvoScChatCommands();
        _services.AddEvoScMiddlewarePipelines();
        _services.AddEvoScPermissions();
        _services.AddEvoScManialinks();

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
        controllers.AddControllerActionRegistry(_services.GetInstance<IManialinkActionManager>());
        
        var pipelineManager = _services.GetInstance<IActionPipelineManager>();
        pipelineManager.UseEvoScCommands(_services);
        pipelineManager.UseEvoScManialinks(_services);
    }

    private async Task SetupModulesAsync()
    {
        var modules = _services.GetInstance<IModuleManager>();
        var config = _services.GetInstance<IEvoScBaseConfig>();

        await modules.LoadInternalModulesAsync();

        var dirs = config.Modules.ModuleDirectories;
        var externalModules = new SortedModuleCollection<IExternalModuleInfo>();
        foreach (var dir in dirs)
        {
            if (!Directory.Exists(dir))
            {
                continue;
            }

            ModuleDirectoryUtils.FindModulesFromDirectory(dir, externalModules);
        }

        externalModules.SetIgnoredDependencies(modules.LoadedModules.Select(m => m.ModuleInfo.Name));
        await modules.LoadAsync(externalModules);
    }

    private async Task StartBackgroundServicesAsync()
    {
        _logger.LogDebug("Starting background services");

        // initialize event manager before anything else
        _services.GetInstance<IEventManager>();

        // initialize player cache
        _services.GetInstance<IPlayerCacheService>();
        
        // initialize manialink handler
        _services.GetInstance<IManialinkInteractionHandler>();

        // connect to the dedicated server and setup callbacks and chat router
        var serverClient = _services.GetInstance<IServerClient>();
        _services.GetInstance<IServerCallbackHandler>();
        _services.GetInstance<IRemoteChatRouter>();
        await serverClient.StartAsync(_runningToken.Token);
    }
    
    private async Task EnableModulesAsync()
    {
        await _services.GetRequiredService<IManialinkManager>().AddDefaultTemplatesAsync();
        await _services.GetRequiredService<IModuleManager>().EnableModulesAsync();
    }
    
    private async Task InitializeTemplatesAsync()
    {
        var maniaLinks = _services.GetRequiredService<IManialinkManager>();
        await maniaLinks.PreprocessAllAsync();
    } */

    public void Dispose()
    {
        var moduleManager = Services.GetInstance<IModuleManager>();

        foreach (var module in moduleManager.LoadedModules)
        {
            moduleManager.UnloadAsync(module.LoadId).GetAwaiter().GetResult();
        }
        
        Services.Dispose();
        _runningToken.Dispose();
    }
}
