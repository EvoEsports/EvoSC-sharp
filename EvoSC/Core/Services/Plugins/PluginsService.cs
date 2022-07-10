using System;
using System.Threading.Tasks;
using EvoSC.Core.Helpers;
using EvoSC.Core.Plugins;
using EvoSC.Interfaces.Players;
using EvoSC.Interfaces.Plugins;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace EvoSC.Core.Services.Plugins;

public class PluginsService : IPluginsService
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    static PluginFactory Plugins => PluginFactory.Instance;

    private readonly IServiceProvider _services;

    public PluginsService(IServiceProvider services)
    {
        _services = services;
    }

    public async Task LoadPlugin(IPlugin plugin)
    {
        try
        {
            var method = ReflectionUtils.GetInstanceMethod(plugin, "Load");

            if (method == null)
            {
                return;
            }

            using var scope = _services.CreateScope();
            await (Task)ReflectionUtils.ExecuteMethod(plugin, method, scope.ServiceProvider);
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to load plugin: {Msg} | Stacktrace: {St}",
                ex.Message, ex.StackTrace);
        }
    }

    public async Task UnloadPlugin(IPlugin plugin)
    {
        try
        {
            var method = ReflectionUtils.GetInstanceMethod(plugin, "Unload");

            if (method == null)
            {
                return;
            }

            using var scope = _services.CreateScope();
            await (Task)ReflectionUtils.ExecuteMethod(plugin, method, scope.ServiceProvider);
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to unload plugin: {Msg} | Stacktrace: {St}",
                ex.Message, ex.StackTrace);
        }
    }

    public async Task LoadPlugins()
    {
        foreach (var pluginId in Plugins.PluginIds)
        {
            var plugin = Plugins.GetPlugin(pluginId);
            await LoadPlugin(plugin.Instance);
        }
    }

    public async Task UnloadPlugins()
    {
        foreach (var pluginId in Plugins.PluginIds)
        {
            var plugin = Plugins.GetPlugin(pluginId);
            await UnloadPlugin(plugin.Instance);
        }
    }
}
