using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace EvoSC.Common.Config;

public static class ConfigServiceExtensions
{
    public static IConfig AddEvoScConfig(this Container services)
    {
        var config = new EvoScConfig(EvoScConfig.ConfigDir);
        // services.Register<IConfig>(config, Lifestyle.Singleton);
        services.RegisterInstance(config);

        // register configs for easy access
        /* services.AddSingleton(config.Get<ServerConfig>(EvoScConfig.ServerConfigKey));
        services.AddSingleton(config.Get<LoggingConfig>(EvoScConfig.LoggingConfigKey));
        services.AddSingleton(config.Get<DatabaseConfig>(EvoScConfig.DatabaseConfigKey)); */
        
        services.RegisterInstance(config.Get<ServerConfig>(EvoScConfig.ServerConfigKey));
        services.RegisterInstance(config.Get<LoggingConfig>(EvoScConfig.LoggingConfigKey));
        services.RegisterInstance(config.Get<DatabaseConfig>(EvoScConfig.DatabaseConfigKey));

        return config;
    }
}
