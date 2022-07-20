using System.Threading;
using System.Threading.Tasks;
using EvoSC.Core.Helpers.Builders;
using EvoSC.Core.Modules;
using EvoSC.Core.Plugins.Abstractions;
using EvoSC.Core.Plugins.Info;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;

namespace EvoSC.Core.Plugins;

public class AppPluginService : IHostedService
{
    private readonly IPluginService _plugins;
    private readonly ILogger<AppPluginService> _logger;
    private PluginsHostConfiguration _options;

    private readonly ExternalPluginCollection _externalPlugins;
    private readonly InternalPluginCollection _internalPlugins;
    
    public AppPluginService(IPluginService plugins, ILogger<AppPluginService> logger, IOptions<PluginsHostConfiguration> options)
    {
        _plugins = plugins;
        _logger = logger;
        _options = options.Value;

        _internalPlugins = InternalPlugins.GetPlugins();
        _externalPlugins = new();
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Loading plugins ...");
        
        // load internal plugins
        await _plugins.LoadCollection(_internalPlugins);
        
        // load external plugins
        _externalPlugins.AddRangeFromDirectory(_options.PluginsDir);

        await _plugins.LoadCollection(_externalPlugins);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Unloading plugins ...");
        
        await _plugins.UnloadAll();
    }
}
