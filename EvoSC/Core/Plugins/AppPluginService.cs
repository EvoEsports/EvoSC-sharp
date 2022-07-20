using System.Threading;
using System.Threading.Tasks;
using EvoSC.Core.Helpers.Builders;
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
    
    public AppPluginService(IPluginService plugins, ILogger<AppPluginService> logger, IOptions<PluginsHostConfiguration> options)
    {
        _plugins = plugins;
        _logger = logger;
        _options = options.Value;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Loading plugins ...");
        
        // load internal plugins
        var internalPlugins = new InternalPluginCollection();

        internalPlugins.Add(
            PluginMetaInfoBuilder.NewInternal<Modules.Info.Info>()
                .WithName("info")
                .WithTitle("Information")
                .WithAuthor("snixtho")
                .WithVersion("1.0.0")
                .WithSummary("Information and help on the controller.")
                .Build()
        );

        
        await _plugins.LoadCollection(internalPlugins);
        
        // load external plugins
        var externalPlugins = new ExternalPluginCollection();
        externalPlugins.AddRangeFromDirectory(_options.PluginsDir);

        await _plugins.LoadCollection(externalPlugins);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Unloading plugins ...");
        return Task.CompletedTask;
    }
}
