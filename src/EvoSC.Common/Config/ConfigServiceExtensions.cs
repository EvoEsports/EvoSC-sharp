using Config.Net;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Config.Stores;
using EvoSC.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace EvoSC.Common.Config;

public static class ConfigServiceExtensions
{
    private const string MainConfigFile = "config/main.toml";
    
    public static IEvoScBaseConfig AddEvoScConfig(this Container services)
    {
        var baseConfig = new ConfigurationBuilder<IEvoScBaseConfig>()
            .UseTomlFile(MainConfigFile)
            .Build();

        services.RegisterInstance<IEvoScBaseConfig>(baseConfig);
        
        return baseConfig;
    }
}
