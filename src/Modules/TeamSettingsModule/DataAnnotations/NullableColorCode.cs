namespace EvoSC.Modules.Official.TeamSettingsModule.DataAnnotations;

public class NullableColorCode : ColorCode
{
    public override bool IsValid(object? value)
    {
        return value?.ToString()?.Length == 0 || base.IsValid(value);
    }
}
