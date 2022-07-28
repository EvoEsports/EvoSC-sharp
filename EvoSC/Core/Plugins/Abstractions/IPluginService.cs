using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoSC.Core.Plugins.Abstractions;

public interface IPluginService
{
    /// <summary>
    /// List of loaded plugins.
    /// </summary>
    public IReadOnlyList<IPluginLoadContext> LoadedPlugins { get; }
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
    /// <param name="throwIfNotUnloaded">Throw an exception if the GC cannot collect all references.</param>
    /// <returns></returns>
    public Task UnloadPlugin(Guid loadId, bool throwIfNotUnloaded=false);
    /// <summary>
    /// Load plugins from a collection.
    /// </summary>
    /// <param name="collection">A sortable (by dependency) collection of plugins.</param>
    /// <returns></returns>
    public Task LoadCollection(ISortedPluginCollection collection);
    /// <summary>
    /// Unload all plugins.
    /// </summary>
    /// <returns></returns>
    public Task UnloadAll();
}
