using Config.Net;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Config.Mapping;

public class TextColorTypeParser : ITypeParser
{
    public bool TryParse(string? value, Type t, out object? result)
    {
        if (value == null || value.Length != 3)
        {
            result = null;
            return false;
        }

        try
        {
            var r = Convert.FromHexString("0" + value[0]).First();
            var g = Convert.FromHexString("0" + value[1]).First();
            var b = Convert.FromHexString("0" + value[2]).First();

            result = new TextColor(r, g, b);

            return true;
        }
        catch (FormatException ex)
        {
            result = null;
            return false;
        }
    }

    public string? ToRawString(object? value)
    {
        return value?.ToString()?[1..] ?? "";
    }

    public IEnumerable<Type> SupportedTypes => new[] {typeof(TextColor)};
}
