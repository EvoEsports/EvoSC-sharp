using System.Threading.Tasks;
using EvoSC.Core.Plugins;

namespace EvoSC.Interfaces.Plugins;

public interface IPluginsService
{
    public Task LoadPlugin(IPlugin plugin);
    public Task UnloadPlugin(IPlugin plugin);
    public Task LoadPlugins();
    public Task UnloadPlugins();
}
