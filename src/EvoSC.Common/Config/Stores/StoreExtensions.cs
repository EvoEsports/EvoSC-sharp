using Config.Net;

namespace EvoSC.Common.Config.Stores;

public static class StoreExtensions
{
    /// <summary>
    /// Use the TOML file store for this config.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="path">The path to the config file.</param>
    /// <typeparam name="TInterface">The interface which defines the config options.</typeparam>
    /// <returns></returns>
    public static ConfigurationBuilder<TInterface> UseEvoScConfig<TInterface>(
        this ConfigurationBuilder<TInterface> builder, string path, Dictionary<string, string> cliOptions) where TInterface : class
    {
        builder.UseConfigStore(new EvoScBaseConfigStore(path, cliOptions));
        return builder;
    }
}
