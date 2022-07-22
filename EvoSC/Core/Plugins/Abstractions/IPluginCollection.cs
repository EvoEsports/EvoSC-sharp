using System.Collections.Generic;

namespace EvoSC.Core.Plugins.Abstractions;

public interface IPluginCollection
{
    /// <summary>
    /// A collection of plugins mapped to their unique name.
    /// </summary>
    public Dictionary<string, IPluginMetaInfo> Plugins { get; }
    
    /// <summary>
    /// Add a new plugin to the collection.
    /// </summary>
    /// <param name="pluginMeta"></param>
    public void Add(IPluginMetaInfo pluginMeta);
}
