using Config.Net;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Config.Stores;
using EvoSC.Common.Config.Mapping;
using EvoSC.Common.Config.Mapping.Toml;
using EvoSC.Common.Interfaces.Config.Mapping;
using SimpleInjector;

namespace EvoSC.Common.Config;

public static class ConfigServiceExtensions
{
    private const string MainConfigFile = "config/main.toml";
    
    /// <summary>
    /// Set up and parse EvoSC's base configuration. This method also adds the configuration
    /// to the service container.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IEvoScBaseConfig AddEvoScConfig(this Container services)
    {
        SetupTomlMapping(services);

        var baseConfig = new ConfigurationBuilder<IEvoScBaseConfig>()
            .UseTomlFile(MainConfigFile)
            .UseTypeParser(new TextColorTypeParser())
            .Build();

        services.RegisterInstance<IEvoScBaseConfig>(baseConfig);
        
        return baseConfig;
    }

    private static void SetupTomlMapping(Container services)
    {
        var mappingManager = new TomlMappingManager();
        mappingManager.SetupDefaultMappers();
        services.RegisterInstance<ITomlMappingManager>(mappingManager);
    }
}
