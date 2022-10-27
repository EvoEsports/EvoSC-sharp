using Config.Net;

namespace EvoSC.Common.Config.Stores;

public static class StoreExtensions
{
    public static ConfigurationBuilder<TInterface> UseTomlFile<TInterface>(
        this ConfigurationBuilder<TInterface> builder, string path) where TInterface : class
    {
        builder.UseConfigStore(new TomlConfigStore<TInterface>(path));
        return builder;
    }
}
