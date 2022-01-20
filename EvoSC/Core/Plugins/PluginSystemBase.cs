using System;
using System.Collections.Generic;
using System.IO;
using EvoSC.Core.Contracts;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Core.Plugins;

public class PluginSystemBase
{
    public static List<PluginLoader> GetPluginLoaders()
    {
        var loaders = new List<PluginLoader>();

        var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");
        foreach (var dir in Directory.GetDirectories(pluginsDir))
        {
            var dirName = Path.GetFileName(dir);
            var pluginDll = Path.Combine(dir, dirName + ".dll");
            if (File.Exists(pluginDll))
            {
                var loader = PluginLoader.CreateFromAssemblyFile(
                    pluginDll,
                    sharedTypes: new[]
                    {
                        typeof(IPluginFactory), typeof(IServiceCollection)
                    }
                );
                
                loaders.Add(loader);
            }
        }

        return loaders;
    }
}
