using System;
using System.Threading.Tasks;

namespace EvoSC.Core.Plugins.Abstractions;

public interface IPluginService
{
    /// <summary>
    /// Load a plugin.
    /// </summary>
    /// <param name="dir">The directory which holds the plugin's files.</param>
    /// <returns></returns>
    public Task LoadPlugin(string dir);
    /// <summary>
    /// Unload a plugin.
    /// </summary>
    /// <param name="loadId"></param>
    /// <returns></returns>
    public Task UnloadPlugin(Guid loadId);
    /// <summary>
    /// Load plugins from a collection.
    /// </summary>
    /// <param name="collection">A sortable (by dependency) collection of plugins.</param>
    /// <returns></returns>
    public Task LoadCollection(ISortedPluginCollection collection);
}
