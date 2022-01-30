using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using McMaster.NETCore.Plugins;

namespace EvoSC.Core.Plugins;

public class PluginManager
{
    private readonly List<PluginLoader> _loaders = new();
    private readonly Dictionary<Guid, WeakReference> _cache = new();

    public PluginLoader[] Loaders => _loaders.ToArray();

    /// <summary>Scans for available plugins at the specified path.</summary>
    /// <param name="path">The path.</param>
    /// <returns>
    ///   <para>Returns true if plugins have been loaded successfuly, otherwise returns false.<br /></para>
    /// </returns>
    public bool ScanForPlugins(string path = "plugins")
    {
        var blnRet = true;

        var pluginsDir = Path.Combine(AppContext.BaseDirectory, path);

        if (!Directory.Exists(pluginsDir))
            Directory.CreateDirectory(pluginsDir);

        foreach (var dir in Directory.GetDirectories(pluginsDir))
        {
            var dirName = Path.GetFileName(dir);
            var pluginFileName = Path.Combine(dir, dirName + ".dll");
            if (File.Exists(pluginFileName))
            {
                try
                {
                    var loader = PluginLoader.CreateFromAssemblyFile(
                            pluginFileName,
                            sharedTypes: new[] { typeof(IEvoScPlugin) },
                            config => config.EnableHotReload = true);

                    _loaders.Add(loader);

                    blnRet &= true;
                }
                catch (Exception ex)
                {
                    blnRet = false;

                    //TODO: Implement proper logging
                    Console.WriteLine(ex.Message);
                }
            }
        }

        return blnRet;
    }

    /// <summary>Loads the and execute plugin contained by the PluginLoader instance.</summary>
    /// <param name="loader">The plugin loader.</param>
    /// <returns>Returns true if the plugin execution was successful, otherwise returns false.<br /></returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public bool LoadAndExecutePlugin(PluginLoader loader)
    {
        var blnRet = false;

        try
        {
            var pluginType = loader.LoadDefaultAssembly().GetExportedTypes().Where(t => t.GetInterface("IEvoScPlugin") != null).FirstOrDefault();
            if (pluginType != null)
            {
                var instance = Activator.CreateInstance(pluginType);

                if (instance != null)
                {
                    pluginType.GetMethod("Load")!.Invoke(instance, null);
                    pluginType.GetMethod("Execute")!.Invoke(instance, null);

                    var id = (Guid)pluginType!.GetProperty("Id")!.GetValue(instance, null)!;

                    _cache.Add(id, new WeakReference(instance!));

                    Console.WriteLine($"Created new instance of '{pluginType}' and added it to the plugin cache");
                }
                else
                {
                    //TODO: Add proper logging
                    Console.WriteLine($"Couldn't instantiate '{pluginType}'!");
                }
            }
        }
        catch (Exception ex)
        {
            //TODO: Implement proper logging
            Console.WriteLine(ex.Message);
        }

        return blnRet;
    }

    /// <summary>Unloads the plugin contained by the PluginLoader instance.</summary>
    /// <param name="pluginLoader">The plugin loader.</param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void UnloadPlugin(PluginLoader pluginLoader)
    {
        var pluginType = pluginLoader.LoadDefaultAssembly().GetExportedTypes().Where(t => t.GetInterface("IEvoScPlugin") != null).FirstOrDefault();
        if (pluginType != null)
        {
            foreach (var item in _cache)
            {
                if (pluginType.Equals(item.Value.Target!.GetType()))
                {
                    pluginType.GetMethod("Unload")!.Invoke(item.Value.Target, null);

                    pluginLoader.Dispose();

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    break;
                }
            }
        }
    }
}
