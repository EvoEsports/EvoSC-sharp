using Config.Net;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Config.Stores;
using EvoSC.Common.Config.Mapping;
using EvoSC.Common.Config.Mapping.Toml;

namespace EvoSC.Common.Config;

public static class Configuration
{
    private const string MainConfigFile = "config/main.toml";
    
    /// <summary>
    /// Set up and parse EvoSC's base configuration.
    /// </summary>
    /// <param name="configFile">The TOML file containing the base config.</param>
    /// <returns></returns>
    public static IEvoScBaseConfig GetBaseConfig(string configFile, Dictionary<string, string> cliOptions)
    {
        ConfigMapper.SetupDefaultMappers();

        var baseConfig = new ConfigurationBuilder<IEvoScBaseConfig>()
            .UseEvoScConfig(MainConfigFile, cliOptions)
            .UseTypeParser(new TextColorTypeParser())
            .Build();
        
        return baseConfig;
    }

    /// <summary>
    /// Set up and parse EvoSC's base configuration.
    /// </summary>
    /// <returns></returns>
    public static IEvoScBaseConfig GetBaseConfig(Dictionary<string, string> cliOptions) => GetBaseConfig(MainConfigFile, cliOptions);
}
