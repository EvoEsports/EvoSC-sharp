using System.Threading;
using System.Threading.Tasks;
using EvoSC.Core.Plugins.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;

namespace EvoSC.Core.Plugins;

public class AppPluginService : IHostedService
{
    private readonly IPluginService _plugins;
    private readonly ILogger<AppPluginService> _logger;
    
    public AppPluginService(IPluginService plugins, ILogger<AppPluginService> logger)
    {
        _plugins = plugins;
        _logger = logger;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Loading plugins ...");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Unloading plugins ...");
        return Task.CompletedTask;
    }
}
