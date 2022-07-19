using System.Collections.Generic;
using EvoSC.Core.Plugins.Abstractions;

namespace EvoSC.Core.Plugins;

public class InternalSortedPluginCollection : ISortedPluginCollection
{
    private Dictionary<string, IPluginMetaInfo> _plugins = new();

    public Dictionary<string, IPluginMetaInfo> Plugins => _plugins;

    public override void Add(IPluginMetaInfo pluginMeta) => _plugins.Add(pluginMeta.Name, pluginMeta);
}
