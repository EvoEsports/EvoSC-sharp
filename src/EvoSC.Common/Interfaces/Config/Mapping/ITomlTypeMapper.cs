using Tomlet.Models;

namespace EvoSC.Common.Interfaces.Config.Mapping;

public interface ITomlTypeMapper<T>
{
    /// <summary>
    /// Serialize a type's value to a TomlValue.
    /// </summary>
    /// <param name="typeValue">The value to convert.</param>
    /// <returns></returns>
    public TomlValue Serialize(T? typeValue);
    
    /// <summary>
    /// Convert a TomlValue to the CLR type's value.
    /// </summary>
    /// <param name="tomlValue">The value to convert.</param>
    /// <returns></returns>
    public T Deserialize(TomlValue tomlValue);
}
