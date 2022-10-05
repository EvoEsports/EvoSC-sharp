
using System.Runtime.InteropServices;
using EvoSC.Common.Config;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Logging;
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

    public Application(string[] args)
    {
        _args = args;
        _isDebug = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development";
    }
    
    public Task RunAsync()
    {
        SetupServices();

        _logger = _serviceProvider.GetRequiredService<ILogger<Application>>();
        _logger.LogInformation("Starting application");

        return Task.Delay(-1);
    }

    public Task ShutdownAsync()
    {
        throw new NotImplementedException();
    }

    private void SetupServices()
    {
        var config = _services.AddEvoScConfig();
        _services.AddEvoScLogging(config.Get<LoggingConfig>(EvoScConfig.LoggingConfigKey));

        _services.AddSingleton<IEvoSCApplication>(this);
        _serviceProvider = _services.BuildServiceProvider();
    }
}
