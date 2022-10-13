using EvoSC.Core.Plugins;
using EvoSC.Core.Plugins.Abstractions;
using EvoSC.Core.Services;
using ExamplePlugin2;
using Microsoft.Extensions.DependencyInjection;

namespace ExamplePlugin;

public class ExamplePlugin : EvoSCPlugin
{
    public ExamplePlugin(IChatCommandsService commands, IPluginMetaInfo info)
    {
        commands.RegisterCommands<Commands>(info);
    }
    
    public static void Setup(IServiceCollection services)
    {
        services.AddTransient<ExampleService>();
    }
}