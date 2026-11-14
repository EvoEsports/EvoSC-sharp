using EvoSC.CLI.Interfaces;
using EvoSC.Common.Application;
using EvoSC.Common.Config.Models;
using System.Reflection;
using System.Runtime.InteropServices;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services;
using EvoSC.Modules.Interfaces;
using SimpleInjector;

namespace EvoSC;

public sealed class Application : IEvoSCApplication, IDisposable
{
    private readonly bool _isDebug;

    private readonly IEvoScBaseConfig _config;

    private readonly CancellationTokenSource _runningToken = new();

    private int _shuttingDown;

    public IStartupPipeline StartupPipeline { get; }

    public CancellationToken MainCancellationToken => _runningToken.Token;
    public Container Services => StartupPipeline.ServiceContainer;

    public Application(IEvoScBaseConfig config, ICliContext cliContext)
    {
        _config = config;
        _isDebug = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development";
        StartupPipeline = new StartupPipeline(_config);

        StartupPipeline.ServiceContainer.ConfigureServiceContainerForEvoSc();
        StartupPipeline.Services("CliContext", s => s.RegisterInstance(cliContext));
        StartupPipeline.Services("Application", s => s.RegisterInstance<IEvoSCApplication>(this));
    }

    public async Task RunAsync()
    {
        StartupPipeline.SetupPipeline(_config);
        await StartupPipeline.ExecuteAllAsync();

        using var sigterm = PosixSignalRegistration.Create(PosixSignal.SIGTERM, OnSigterm);

        // wait indefinitely
        WaitHandle.WaitAll(new[] {_runningToken.Token.WaitHandle});
    }

    /// <summary>
    /// The version of the running application, without build metadata.
    /// </summary>
    public static string GetVersion()
    {
        var version = Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        return version?.Split('+')[0] ?? "unknown";
    }

    private void OnSigterm(PosixSignalContext context)
    {
        // we handle the shutdown ourselves so the message can be sent before disconnecting
        context.Cancel = true;

        if (_runningToken.IsCancellationRequested || Interlocked.Exchange(ref _shuttingDown, 1) == 1)
        {
            return;
        }

        try
        {
            Services.GetInstance<IChatService>().WarningMessageAsync("EvoSC# is shutting down.")
                .GetAwaiter().GetResult();
        }
        catch (Exception)
        {
            // the server may already be unreachable, shut down regardless
        }

        ShutdownAsync().GetAwaiter().GetResult();
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
