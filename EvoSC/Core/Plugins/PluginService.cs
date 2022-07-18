using System.Threading.Tasks;
using EvoSC.Core.Plugins.Abstractions;

namespace EvoSC.Core.Plugins;

public class PluginService : IPluginService
{
    public Task LoadPlugin(string directory)
    {
        throw new System.NotImplementedException();
    }

    public Task UnloadPlugin()
    {
        throw new System.NotImplementedException();
    }
}
