using System.Threading.Tasks;

namespace EvoSC.Core.Plugins.Abstractions;

public interface IPluginService
{
    public Task LoadPlugin(string directory);
    public Task UnloadPlugin();
}
