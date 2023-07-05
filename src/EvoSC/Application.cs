using EvoSC.Common.Application;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Modules.Interfaces;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace EvoSC;

public sealed class Application : IEvoSCApplication, IDisposable
{
    private readonly bool _isDebug;

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
        StartupPipeline.SetupPipeline(_config);
        await StartupPipeline.ExecuteAllAsync();

        // wait indefinitely
        WaitHandle.WaitAll(new[] {_runningToken.Token.WaitHandle});
    }
    
    public async Task ShutdownAsync()
    {
        var moduleManager = Services.GetInstance<IModuleManager>();

        foreach (var module in moduleManager.LoadedModules)
        {
            await moduleManager.UnloadAsync(module.LoadId);
        }
        
        var serverClient = Services.GetInstance<IServerClient>();
        await serverClient.StopAsync(_runningToken.Token);
        
        // cancel the token to stop the application itself
        _runningToken.Cancel();
    }

    public void Dispose()
    {
        Services.Dispose();
        _runningToken.Dispose();
    }
}
