using EvoSC.Common.Interfaces.Config.Mapping;
using Tomlet;

namespace EvoSC.Common.Config.Mapping.Toml;

public static class ConfigMapper
{
    public static void AddMapper<T>(ITomlTypeMapper<T> mapper) =>
        TomletMain.RegisterMapper(mapper.Serialize, mapper.Deserialize);

    public static void SetupDefaultMappers()
    {
        AddMapper(new TextColorTomlMapper());
    }
}
