using System.ComponentModel.DataAnnotations;

namespace EvoSC.Modules.Official.TeamSettingsModule.DataAnnotations;

public class ColorCode : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var colorCode = value?.ToString() ?? "";
        
        //TODO: check for valid color code

        return colorCode.Length is 3 or 6;
    }
}
