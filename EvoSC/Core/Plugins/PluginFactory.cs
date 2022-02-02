using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EvoSC.Core.Plugins;

public class PluginFactory
{
    private readonly Dictionary<PluginLoader, IPlugin> _cache = new();
    private readonly IServiceCollection _services;
    private readonly ILogger _logger;

    public PluginFactory(IServiceCollection services, ILogger<PluginFactory> logger)
    {
        _services = services;
        _logger = logger;
    }

    /// <summary>Scans for available plugins at the specified path.</summary>
    /// <param name="path">The path to the plugins location.</param>
    /// <returns>
    ///   <para>Returns true if plugins have been loaded successfuly, otherwise returns false.<br /></para>
    /// </returns>
    public PluginLoader[] ScanForPlugins(string path = "plugins")
    {
        List<PluginLoader> loaders = new();

        string pluginsDir = Path.Combine(AppContext.BaseDirectory, path);

        if (!Directory.Exists(pluginsDir))
            Directory.CreateDirectory(pluginsDir);

        foreach (var dir in Directory.GetDirectories(pluginsDir))
        {
            string dirName = Path.GetFileName(dir);
            string pluginFileName = Path.Combine(dir, dirName + ".dll");

            if (File.Exists(pluginFileName))
            {
                try
                {
                    PluginLoader loader = PluginLoader.CreateFromAssemblyFile(
                            pluginFileName,
                            sharedTypes: new[] { typeof(IPlugin), typeof(IServiceCollection) },
                            config => config.EnableHotReload = true);

                    loaders.Add(loader);

                    _logger.LogDebug($"Added plugin loader for '{pluginFileName}'.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Could not create PluginLoader instance for file '{pluginFileName}'!");
                }
            }
        }

        return loaders.ToArray();
    }

    /// <summary>Loads the and execute plugin contained by the PluginLoader instance.</summary>
    /// <param name="loader">The plugin loader.</param>
    /// <returns>Returns true if the plugin execution was successful, otherwise returns false.<br /></returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public bool LoadAndExecutePlugin(PluginLoader loader)
    {
        try
        {
            Assembly asm = loader.LoadDefaultAssembly();
            Type pluginType = asm.GetExportedTypes()
                .Where(t => t.GetInterface("IPlugin") != null)
                .FirstOrDefault();

            if (pluginType != null)
            {
                IPlugin instance = Activator.CreateInstance(pluginType) as IPlugin;

                if (instance != null)
                {
                    instance.Load(_services);
                    instance.Execute();

                    _cache.Add(loader, instance);

                    _logger.LogTrace($"Created new instance of '{pluginType}' and added it to the plugin cache.");

                    return true;
                }
                else
                {
                    _logger.LogError($"Could not instantiate '{pluginType}'!");

                    return false;
                }
            }
            else
            {
                _logger.LogWarning($"Could not find suitable type in '{asm.FullName}' that implements the 'IEvoScPlugin' interface.");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occured while trying to instantiate plugin!");

            return false;
        }
    }

    /// <summary>Unloads the plugin contained by the PluginLoader instance.</summary>
    /// <param name="loader">The plugin loader.</param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void UnloadPlugin(PluginLoader loader)
    {
        if (_cache.TryGetValue(loader, out IPlugin plugin))
        {
            WeakReference weakRef = new WeakReference(plugin.GetType().Assembly, true);

            plugin.Unload();
            loader.Dispose();

            _cache.Remove(loader);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            var tries = 10;
            while (weakRef.IsAlive && tries-- > 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            
            if (weakRef.IsAlive)
                throw new InvalidOperationException("There are still some references to the plugin assembly!");

            _logger.LogTrace($"Unloaded plugin '{plugin.GetType()}'.");
        }
        else
        {
            _logger.LogWarning("Could not find plugin instance matching plugin loader!");
        }
    }
}
