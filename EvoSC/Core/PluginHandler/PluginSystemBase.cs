using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EvoSC.Contracts;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace EvoSC.Core.PluginHandler;

public static class PluginSystemBase
{
    public static IServiceCollection AddPluginLoaders(this IServiceCollection services)
    {
        var loaders = PluginLoaders();
        // Create an instance of plugin types
        foreach (var loader in loaders)
        {
            foreach (var pluginType in loader
                         .LoadDefaultAssembly()
                         .GetTypes()
                         .Where(t => typeof(IPluginFactory).IsAssignableFrom(t) && !t.IsAbstract))
            {
                // This assumes the implementation of IPluginFactory has a parameterless constructor
                var plugin = Activator.CreateInstance(pluginType) as IPluginFactory;

                plugin?.Configure(services);
            }
        }
        
        return services;
    }

    private static List<PluginLoader> PluginLoaders()
    {
        var loaders = new List<PluginLoader>();

        var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");
        foreach (var dir in Directory.GetDirectories(pluginsDir))
        {
            var dirName = Path.GetFileName(dir);
            var pluginDll = Path.Combine(dir, dirName + ".dll");
            if (!File.Exists(pluginDll))
            {
                continue;
            }

            var loader = PluginLoader.CreateFromAssemblyFile(
                pluginDll,
                sharedTypes: new[]
                {
                    typeof(IPluginFactory), typeof(IServiceCollection), typeof(ILogger)
                },
                config => config.EnableHotReload = true
            );

            loaders.Add(loader);
        }

        return loaders;
    }
}
