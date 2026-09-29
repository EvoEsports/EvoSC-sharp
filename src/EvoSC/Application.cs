using EvoSC.CLI.Interfaces;
using EvoSC.Common.Application;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Services;
using EvoSC.Modules.Interfaces;
using SimpleInjector;

namespace EvoSC;

public sealed class Application : IEvoSCApplication, IDisposable
{
    private readonly IEvoScBaseConfig _config;

    private readonly CancellationTokenSource _runningToken = new();

    public IStartupPipeline StartupPipeline { get; }

    public CancellationToken MainCancellationToken => _runningToken.Token;
    public Container Services => StartupPipeline.ServiceContainer;

    public Application(IEvoScBaseConfig config, ICliContext cliContext)
    {
        _config = config;
        StartupPipeline = new StartupPipeline(_config);

        StartupPipeline.ServiceContainer.ConfigureServiceContainerForEvoSc();
        StartupPipeline.Services("CliContext", s => s.RegisterInstance(cliContext));
        StartupPipeline.Services("Application", s => s.RegisterInstance<IEvoSCApplication>(this));
    }

    public async Task RunAsync()
    {
        StartupPipeline.SetupPipeline(_config);
        await StartupPipeline.ExecuteAllAsync(_runningToken.Token);

        // wait indefinitely
        WaitHandle.WaitAll(new[] {_runningToken.Token.WaitHandle});
    }
    
    public async Task ShutdownAsync()
    {
        var moduleManager = Services.GetInstance<IModuleManager>();

        // Disable everything first so feature registration is torn down in a predictable
        // state, then unload the external modules. The modules that ship with EvoSC stay
        // loaded: they are part of the host for as long as it runs.
        foreach (var module in moduleManager.GetLoadedModules().Where(m => m.IsEnabled))
        {
            await moduleManager.DisableAsync(module.LoadId);
        }

        foreach (var module in moduleManager.GetLoadedModules().Where(m => !m.ModuleInfo.IsInternal))
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
