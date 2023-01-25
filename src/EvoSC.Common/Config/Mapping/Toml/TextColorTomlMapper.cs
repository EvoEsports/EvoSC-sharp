using EvoSC.Common.Interfaces.Config.Mapping;
using EvoSC.Common.Util.TextFormatting;
using Tomlet.Exceptions;
using Tomlet.Models;

namespace EvoSC.Common.Config.Mapping.Toml;

public class TextColorTomlMapper : ITomlTypeMapper<TextColor>
{
    public TomlValue Serialize(TextColor? typeValue) =>
        new TomlString(typeValue.ToString()[1..]);

    public TextColor Deserialize(TomlValue tomlValue)
    {
        if (!(tomlValue is TomlString tomlString))
        {
            // Expected type, actual type, context (type being deserialized)
            throw new TomlTypeMismatchException(typeof(TomlString), tomlValue.GetType(), typeof(TextColor));   
        }

        return new TextColor(tomlString.Value);
    }
}
