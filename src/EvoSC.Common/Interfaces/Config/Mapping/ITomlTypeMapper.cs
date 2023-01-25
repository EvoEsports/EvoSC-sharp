using Tomlet.Models;

namespace EvoSC.Common.Interfaces.Config.Mapping;

public interface ITomlTypeMapper<T>
{
    public TomlValue Serialize(T? typeValue);
    public T Deserialize(TomlValue tomlValue);
}
