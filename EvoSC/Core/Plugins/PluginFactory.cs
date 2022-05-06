using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using EvoSC.Core.Services;
using EvoSC.Interfaces.Players;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace EvoSC.Core.Plugins;

internal class PluginFactory
{
    #region Instance
    private static PluginFactory s_instance;

    public static PluginFactory Instance => s_instance ??= new PluginFactory();
    #endregion

    private readonly Dictionary<Guid, PluginWrapper> _cache = new();
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public Guid[] PluginIds => _cache.Keys.ToArray();

    private PluginFactory()
    {
    }

    /// <summary>Scans for available plugins at the specified path and loads them.</summary>
    /// <param name="path">The path to the plugins location. (Default: "\plugins\")</param>
    /// <returns>
    ///   <para>Returns the plugins ids of successfully loaded plugins.<br /></para>
    /// </returns>
    public Guid[] LoadPlugins(IServiceCollection services, string path = "plugins")
    {
        string pluginsDir = Path.Combine(AppContext.BaseDirectory, path);

        if (!Directory.Exists(pluginsDir))
            Directory.CreateDirectory(pluginsDir);

        foreach (var dir in Directory.GetDirectories(pluginsDir))
        {
            string dirName = Path.GetRelativePath(pluginsDir, dir);
            string pluginFileName = Path.Combine(dir, dirName + ".dll");

            if (File.Exists(pluginFileName))
            {
                try
                {
                    PluginLoader loader = PluginLoader.CreateFromAssemblyFile(
                            assemblyFile: pluginFileName,
                            isUnloadable: true,
                            sharedTypes: new[] { typeof(IPlugin), typeof(ISampleService) },
                            config => config.EnableHotReload = true);

                    IPlugin loadedPlugin = InitializePlugin(loader, services);

                    if (loadedPlugin != null)
                    {
                        PluginWrapper wrapper = new(loadedPlugin, loader);
                        _cache.Add(wrapper.Id, wrapper);

                        _logger.Debug($"Instantiated new plugin ('{pluginFileName}') with ID {wrapper.Id}");
                    }
                    else
                    {
                        _logger.Warn($"Could not instantiate plugin! ('{pluginFileName}')");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Could not create PluginLoader instance! ('{pluginFileName}')");
                }
            }
        }

        return _cache.Keys.ToArray();
    }

    /// <summary>Loads the and execute plugin contained by the PluginLoader instance.</summary>
    /// <param name="loader">The plugin loader.</param>
    /// <returns>Returns true if the plugin execution was successful, otherwise returns false.<br /></returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private IPlugin InitializePlugin(PluginLoader loader, IServiceCollection services)
    {
        try
        {
            Assembly asm = loader.LoadDefaultAssembly();
            Type pluginType = asm.GetExportedTypes()
                .Where(t => t.GetInterface("IPlugin") != null)
                .FirstOrDefault();

            if (pluginType != null)
            {
                if (Activator.CreateInstance(pluginType) is IPlugin instance)
                {
                    instance.Load(services);
                    instance.Execute();

                    _logger.Trace($"Created new instance of type '{pluginType}'.");

                    return instance;
                }
                else
                {
                    _logger.Error($"Could not instantiate instance of type '{pluginType}'!");

                    return null;
                }
            }
            else
            {
                _logger.Warn($"Could not find suitable type in '{asm.FullName}' that implements the 'IPlugin' interface.");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error occured while trying to instantiate plugin!");

            return null;
        }
    }

    /// <summary>Unloads the plugin contained by the PluginLoader instance.</summary>
    /// <param name="loader">The plugin loader.</param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void UnloadPlugin(Guid id, IServiceCollection services)
    {
        if (!_cache.ContainsKey(id))
        {
            _logger.Warn("Could not find plugin instance matching plugin loader!");
            return;
        }

        string logMessage;

        WeakReference weakRef;
        {
            var wrapper = _cache[id];

            logMessage = $"Unloaded plugin. ('{wrapper.Instance.GetType()}')";

            weakRef = new WeakReference(wrapper.Instance.GetType().Assembly, true);
            wrapper.Instance.Unload(services);
            wrapper.Loader.Dispose();
            _cache.Remove(id);
            wrapper = null;
        }

        var tries = 10;
        while (weakRef.IsAlive && tries-- > 0)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        if (weakRef.IsAlive)
            throw new InvalidOperationException("There are still some references to the plugin assembly!");
        else if (!string.IsNullOrEmpty(logMessage))
            _logger.Trace(logMessage);
    }
}
