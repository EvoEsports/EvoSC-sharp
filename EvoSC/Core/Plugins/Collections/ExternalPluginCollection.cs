using System;
using System.Collections.Generic;
using System.IO;
using EvoSC.Core.Plugins.Abstractions;
using EvoSC.Core.Plugins.Info;
using Microsoft.Extensions.ObjectPool;

namespace EvoSC.Core.Plugins;

/// <summary>
/// A collection to keep track of external plugins.
/// </summary>
public class ExternalPluginCollection : ISortedPluginCollection
{
    private Dictionary<string, IPluginMetaInfo> _plugins = new();

    public override Dictionary<string, IPluginMetaInfo> Plugins => _plugins;
    
    public override void Add(IPluginMetaInfo pluginMeta) => _plugins.Add(pluginMeta.Name, pluginMeta);

    /// <summary>
    /// Add a single plugin from a directory.
    /// </summary>
    /// <param name="pluginDir"></param>
    public void AddFromDirectory(string pluginDir) => Add(PluginMetaInfo.FromDirectory(pluginDir));

    /// <summary>
    /// Add all plugins found in a directory.
    /// </summary>
    /// <param name="pluginsDir"></param>
    public void AddRangeFromDirectory(string pluginsDir)
    {
        foreach (var pluginDir in Directory.GetDirectories(Path.GetFullPath(pluginsDir)))
        {
            if (!PluginMetaInfo.MetaFileExists(pluginDir))
            {
                continue;
            }

            AddFromDirectory(pluginDir);
        }
    }
}
