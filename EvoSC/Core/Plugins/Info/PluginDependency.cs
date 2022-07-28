using System;
using System.IO;
using EvoSC.Core.Helpers;
using EvoSC.Core.Plugins.Abstractions;
using Tomlet;

namespace EvoSC.Core.Plugins.Info;

public class PluginDependency : IPluginDependency
{
    public string Name { get; init; }
    public Version Version { get; init; }
    public string? ResolvedPath { get; private set; }
    
    public bool ResolvePath(string pluginsDir)
    {
        foreach (var pluginDir in Directory.GetDirectories(Path.GetFullPath(pluginsDir)))
        {
            var metaFile = $"{pluginDir}/{PluginMetaInfo.MetaFileName}";

            if (!File.Exists(metaFile))
            {
                continue;
            }

            var document = TomlParser.ParseFile(metaFile);
            var name = document.ValidateEntry<string>("info.name");

            if (Name == name) 
            {
                ResolvedPath = Path.GetFullPath(pluginDir);
                return true;
            }
        }
        
        return false;
    }
}
