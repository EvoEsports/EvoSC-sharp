
using System.Diagnostics;
using System.Runtime.InteropServices;
using EvoSC.Common.Config;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Logging;
using EvoSC.Common.Remote;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EvoSC;

public class Application : IEvoSCApplication
{
    private readonly string[] _args;
    private IServiceCollection _services = new ServiceCollection();
    private IServiceProvider _serviceProvider;
    private bool _isDebug;
    private ILogger<Application> _logger;

    private readonly CancellationTokenSource _runningToken = new();

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
        
        _services.AddEvoScLogging(config.Get<LoggingConfig>(EvoScConfig.LoggingConfigKey));
        _services.AddGbxRemoteClient();

        _services.AddSingleton<IEvoSCApplication>(this);
        _serviceProvider = _services.BuildServiceProvider();
        
        _logger = _serviceProvider.GetRequiredService<ILogger<Application>>();
    }

    private async Task StartBackgroundServices()
    {
        var serverClient = _serviceProvider.GetRequiredService<IServerClient>();
        await serverClient.StartAsync(_runningToken.Token);
        
        _logger.LogDebug("Starting background services");
    }
}
