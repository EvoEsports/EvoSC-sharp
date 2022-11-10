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
    public static ConfigurationBuilder<TInterface> UseTomlFile<TInterface>(
        this ConfigurationBuilder<TInterface> builder, string path) where TInterface : class
    {
        builder.UseConfigStore(new TomlConfigStore<TInterface>(path));
        return builder;
    }
}
