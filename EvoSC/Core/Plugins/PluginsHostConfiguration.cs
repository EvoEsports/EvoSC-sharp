using EvoSC.Core.Plugins.Abstractions;

namespace EvoSC.Core.Plugins;

public class PluginsHostConfiguration
{
    /// <summary>
    /// The path to the directory where plugins can be found.
    /// </summary>
    public string PluginsDir { get; set; } = "plugins";
    /// <summary>
    /// The name of the file which plugin's meta info are in.
    /// </summary>
    public string MetaFile { get; set; } = "info.json";
}
