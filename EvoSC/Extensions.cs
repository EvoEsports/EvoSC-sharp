using EvoSC.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddPluginFactory(this IServiceCollection services, string pluginsPath = "plugins")
    {
        services.AddSingleton<PluginFactory>();

        services.BuildServiceProvider().GetService<PluginFactory>()
            .ScanForPlugins(pluginsPath);

        return services;
    }
}