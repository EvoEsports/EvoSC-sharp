using System;
using EvoSC.Core.Plugins.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EvoSC.Core.Plugins;

public static class PluginsHostBuilderExtensions
{
    public static IHostBuilder UsePlugins(this IHostBuilder builder, Action<PluginsHostConfiguration>? config = null)
    {
        return InternalUsePlugins(builder, (context, services) => config(services));
    }
    
    public static IHostBuilder UsePlugins(this IHostBuilder builder, Action<HostBuilderContext, PluginsHostConfiguration>? config = null)
    {
        return InternalUsePlugins(builder, config);
    }

    private static IHostBuilder InternalUsePlugins(IHostBuilder builder, Action<HostBuilderContext, PluginsHostConfiguration>? config=null)
    {
        return builder.ConfigureServices((context, services) =>
        {
            services.AddOptions<PluginsHostConfiguration>();

            if (config != null)
            {
                services.Configure<PluginsHostConfiguration>(x => config(context, x));
            }

            services.AddSingleton<IPluginService, PluginService>();
            services.AddHostedService<AppPluginService>();
        });
    }
}
