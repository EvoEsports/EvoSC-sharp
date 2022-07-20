﻿using EvoSC.Core.Helpers.Builders;
using EvoSC.Core.Plugins;

namespace EvoSC.Core.Modules;

public static class InternalPlugins
{
    public static InternalPluginCollection GetPlugins()
    {
        var plugins = new InternalPluginCollection();

        plugins.Add(
            PluginMetaInfoBuilder.NewInternal<Info.Info>()
                .WithName("info")
                .WithTitle("Information")
                .WithAuthor("snixtho")
                .WithVersion("1.0.0")
                .WithSummary("Information and help on the controller.")
                .Build()
        );
        
        return plugins;
    }
}
