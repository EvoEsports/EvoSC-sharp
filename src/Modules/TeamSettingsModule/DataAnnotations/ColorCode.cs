using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EvoSC.Modules.Official.TeamSettingsModule.DataAnnotations;

public partial class ColorCode : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var colorCode = value?.ToString() ?? "";

        return ValidColorCodeRegex().IsMatch(colorCode);
    }

    [GeneratedRegex("^(?:[0-9a-fA-F]{3}){1,2}$")]
    private static partial Regex ValidColorCodeRegex();
}
