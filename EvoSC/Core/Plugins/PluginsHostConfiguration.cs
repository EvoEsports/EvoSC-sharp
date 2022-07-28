using EvoSC.Core.Plugins.Abstractions;

namespace EvoSC.Core.Plugins;

public class PluginsHostConfiguration
{
    /// <summary>
    /// The path to the directory where plugins can be found.
    /// </summary>
    public string PluginsDir { get; set; } = "plugins";
}
