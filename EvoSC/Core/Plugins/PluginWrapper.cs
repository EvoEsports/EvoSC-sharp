using System;
using McMaster.NETCore.Plugins;

namespace EvoSC.Core.Plugins;

internal class PluginWrapper
{
    public Guid Id { get; }

    public IPlugin Instance { get; }

    /// <summary>Gets the plugin loader.</summary>
    /// <value>The plugin loader.</value>
    public PluginLoader Loader { get; }

    /// <summary>Initializes a new instance of the <see cref="PluginWrapper" /> class.</summary>
    /// <param name="instance">The instance.</param>
    /// <param name="pluginLoader">The plugin loader.</param>
    public PluginWrapper(IPlugin instance, PluginLoader pluginLoader)
    {
        Id = Guid.NewGuid();
        Instance = instance;
        Loader = pluginLoader;
    }
}
