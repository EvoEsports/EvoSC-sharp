using System.ComponentModel.DataAnnotations;

namespace EvoSC.Modules.Official.TeamSettingsModule.DataAnnotations;

public class NullableUrl : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        string url = value?.ToString()?.Trim() ?? "";

        return url.Length == 0 || Uri.IsWellFormedUriString(url, UriKind.Absolute);
    }
}
