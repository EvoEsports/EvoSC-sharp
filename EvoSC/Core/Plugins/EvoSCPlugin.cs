using EvoSC.Core.Plugins.Abstractions;

namespace EvoSC.Core.Plugins;

public abstract class EvoSCPlugin : IPlugin
{
    protected IPluginMetaInfo Info { get; private set; }

    void IPlugin.SetInfo(IPluginMetaInfo metaInfo)
    {
        Info = metaInfo;
    }
}
