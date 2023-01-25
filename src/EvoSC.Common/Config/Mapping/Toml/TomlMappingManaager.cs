using EvoSC.Common.Interfaces.Config.Mapping;
using Tomlet;

namespace EvoSC.Common.Config.Mapping.Toml;

public class TomlMappingManager : ITomlMappingManager
{
    public void AddMapper<T>(ITomlTypeMapper<T> mapper) =>
        TomletMain.RegisterMapper(mapper.Serialize, mapper.Deserialize);

    public void SetupDefaultMappers()
    {
        AddMapper(new TextColorTomlMapper());
    }
}
