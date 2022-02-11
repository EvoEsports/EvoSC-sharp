using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using EvoSC.Core.Services;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace EvoSC.Core.Plugins;

public class PluginFactory
{
    #region Singleton
    private static PluginFactory _instance;

    public static PluginFactory Instance => _instance ??= new PluginFactory();
    #endregion

    private readonly Dictionary<Guid, PluginWrapper> _cache = new();
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    private PluginFactory()
    {
    }

    /// <summary>Scans for available plugins at the specified path.</summary>
    /// <param name="path">The path to the plugins location.</param>
    /// <returns>
    ///   <para>Returns true if plugins have been loaded successfuly, otherwise returns false.<br /></para>
    /// </returns>
    public bool LoadPlugins(IServiceCollection services, string path = "plugins")
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
                            isUnloadable: true,
                            sharedTypes: new[] { typeof(IPlugin), typeof(ISampleService) },
                            config => config.EnableHotReload = true);

                    Type[] serviceTypes = LoadServiceDescriptor(dir);

                    loaders.Add(loader);

                    LoadAndExecutePlugin(loader, serviceTypes, services);

                    _logger.Debug($"Added plugin loader for '{pluginFileName}'.");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Could not create PluginLoader instance for file '{pluginFileName}'!");
                }
            }
        }

        return loaders.Count > 0;
    }

    /// <summary>Loads the and execute plugin contained by the PluginLoader instance.</summary>
    /// <param name="loader">The plugin loader.</param>
    /// <returns>Returns true if the plugin execution was successful, otherwise returns false.<br /></returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private bool LoadAndExecutePlugin(PluginLoader loader, Type[] serviceTypes, IServiceCollection services)
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
                    instance.Load(services);
                    instance.Execute();

                    PluginWrapper wrapper = new PluginWrapper(instance, loader, serviceTypes);

                    _cache.Add(wrapper.Id, wrapper);

                    _logger.Trace($"Created new instance of '{pluginType}' and added it to the plugin cache.");

                    return true;
                }
                else
                {
                    _logger.Error($"Could not instantiate '{pluginType}'!");

                    return false;
                }
            }
            else
            {
                _logger.Warn($"Could not find suitable type in '{asm.FullName}' that implements the 'IEvoScPlugin' interface.");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error occured while trying to instantiate plugin!");

            return false;
        }
    }

    /// <summary>Unloads the plugin contained by the PluginLoader instance.</summary>
    /// <param name="loader">The plugin loader.</param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void UnloadPlugin(Guid id)
    {
        if (_cache.TryGetValue(id, out PluginWrapper wrapper))
        {
            WeakReference weakRef = new WeakReference(wrapper.Plugin.GetType().Assembly, true);

            wrapper.Plugin.Unload();
            wrapper.Loader.Dispose();

            _cache.Remove(id);
            
            wrapper = null;

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

            _logger.Trace($"Unloaded plugin '{wrapper.Plugin.GetType()}'.");
        }
        else
        {
            _logger.Warn("Could not find plugin instance matching plugin loader!");
        }
    }

    private Type[] LoadServiceDescriptor(string pluginDir)
    {
        List<Type> types = new();

        if (Directory.Exists(pluginDir))
        {
            try
            {
                string servicesFileName = Directory.GetFiles(pluginDir, "*.Services.json")[0];

                JObject servicesObject;

                using (StreamReader file = File.OpenText(servicesFileName))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        servicesObject = (JObject)JToken.ReadFrom(reader);
                    }
                }

                if (servicesObject != null)
                {
                    foreach (JToken serviceInfo in servicesObject.Children())
                    {
                        //item.First
                    }
                }
                else
                {
                    _logger.Warn("Couldn't load information from services descriptor!");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Couldn't load services descriptor!");
            }
        }

        return types.ToArray();
    }
}
