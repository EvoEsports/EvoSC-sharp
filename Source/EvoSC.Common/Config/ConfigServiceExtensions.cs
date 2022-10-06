using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Config;

public static class ConfigServiceExtensions
{
    public static IConfig AddEvoScConfig(this IServiceCollection services)
    {
        var config = new EvoScConfig(EvoScConfig.ConfigDir);
        services.AddSingleton<IConfig>(config);

        // register configs for easy access
        services.AddSingleton(config.Get<ServerConfig>(EvoScConfig.ServerConfigKey));
        services.AddSingleton(config.Get<LoggingConfig>(EvoScConfig.LoggingConfigKey));
        services.AddSingleton(config.Get<DatabaseConfig>(EvoScConfig.DatabaseConfigKey));

        return config;
    }
}
