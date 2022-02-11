using System;
using McMaster.NETCore.Plugins;

namespace EvoSC.Core.Plugins;

public class PluginWrapper
{
    public Guid Id { get; }
    public IPlugin Plugin { get; }
    public PluginLoader Loader { get; }
    public Type[] ServiceTypes { get; }

    public PluginWrapper(IPlugin plugin, PluginLoader pluginLoader, Type[] serviceTypes)
    {
        Id = Guid.NewGuid();
        Plugin = plugin;
        Loader = pluginLoader;
        ServiceTypes = serviceTypes;
    }
}