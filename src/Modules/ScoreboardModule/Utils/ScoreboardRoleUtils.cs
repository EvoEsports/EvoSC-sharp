using EvoSC.Modules.Official.GroupsModule.Models;

namespace EvoSC.Modules.Official.ScoreboardModule.Utils;

public static class ScoreboardRoleUtils
{
    /// <summary>
    /// Formats a role as the colored text that is shown on the scoreboard.
    /// </summary>
    public static string FormatRole(PlayerRole role)
    {
        var icon = string.IsNullOrWhiteSpace(role.Icon) ? "" : $"{role.Icon} ";
        return $"${role.Color}{icon}{role.Title}";
    }

    /// <summary>
    /// Converts a map of login to role text to a ManiaScript array.
    /// </summary>
    public static string ToManiaScriptArray(IReadOnlyDictionary<string, string> rolesByLogin)
    {
        var entries = rolesByLogin.Select(r => $"\"{EscapeText(r.Key)}\" => \"{EscapeText(r.Value)}\"");
        return $"[{string.Join(",\n", entries)}]";
    }

    /// <summary>
    /// Escapes text to be safely inserted as a ManiaScript string inside a XML comment.
    /// </summary>
    public static string EscapeText(string text) => text
        .Replace("\\", "\\\\", StringComparison.Ordinal)
        .Replace("\"", "\\\"", StringComparison.Ordinal)
        .Replace("\r", "", StringComparison.Ordinal)
        .Replace("\n", " ", StringComparison.Ordinal)
        .Replace("-->", "-→", StringComparison.Ordinal);
}
