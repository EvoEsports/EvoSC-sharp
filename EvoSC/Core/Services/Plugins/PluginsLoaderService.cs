using System.Threading;
using System.Threading.Tasks;
using EvoSC.Interfaces.Plugins;
using Microsoft.Extensions.Hosting;

namespace EvoSC.Core.Services.Plugins;

public class PluginsLoaderService : IHostedService
{
    private readonly IPluginsService _plugins;
    
    public PluginsLoaderService(IPluginsService plugins)
    {
        _plugins = plugins;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _plugins.LoadPlugins();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _plugins.UnloadPlugins();
    }
}
