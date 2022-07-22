using EvoSC.Core.Plugins.Abstractions;

namespace EvoSC.Core.Plugins.Extensions;

public static class PluginServiceExtensions
{
    /// <summary>
    /// Get the load context of a plugin by it's name.
    /// </summary>
    /// <param name="pluginService"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static IPluginLoadContext FindPluginByName(this IPluginService pluginService, string name)
    {
        foreach (var plugin in pluginService.LoadedPlugins)
        {
            if (plugin.MetaInfo.Name == name)
            {
                return plugin;
            }
        }

        return null;
    }
}
