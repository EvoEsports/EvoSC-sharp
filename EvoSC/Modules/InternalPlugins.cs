using EvoSC.Core.Helpers.Builders;
using EvoSC.Core.Plugins;

namespace EvoSC.Modules;

public static class InternalPlugins
{
    public static InternalPluginCollection GetPlugins()
    {
        var plugins = new InternalPluginCollection();

        plugins.Add(
            PluginMetaInfoBuilder.NewInternal<Info.Info>()
                .WithName("Info")
                .WithTitle("Information")
                .WithAuthor("snixtho")
                .WithVersion("1.0.0")
                .WithSummary("Information and help on the controller.")
                .Build()
        );
        
        return plugins;
    }
}
