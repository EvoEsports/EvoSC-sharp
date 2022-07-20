using System;
using EvoSC.Core.Plugins;
using EvoSC.Core.Plugins.Abstractions;

namespace EvoSC.Core.Modules.Info;

public class Info : EvoSCPlugin
{
    public Info(IPluginMetaInfo info)
    {
        Console.WriteLine($"hello from {info.Name}");
    }
}
