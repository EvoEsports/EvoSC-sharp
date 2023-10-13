using System.Text.RegularExpressions;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Util;

public static class FormattingUtils
{
    /// <summary>
    /// Pattern to match TrackMania's formatting syntax.
    /// </summary>
    private static readonly Regex TmTextFormatPattern = new Regex(@"\$((L|H)\[.+\]|[\da-f]{3}|[\w\$\<\>]{1})",
        RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));

    /// <summary>
    /// Removes all TrackMania formatting from a string.
    /// </summary>
    /// <param name="text">The text to clean up.</param>
    /// <returns></returns>
    public static string CleanTmFormatting(string text) =>
        TmTextFormatPattern.Replace(text, "");

    /// <summary>
    /// Formats the given milliseconds into human readable time.
    /// </summary>
    /// <param name="milliseconds">The time in milliseconds.</param>
    /// <returns></returns>
    public static string FormatTime(int milliseconds)
    {
        var ms = milliseconds % 1000;
        var s = milliseconds / 1000 % 60;
        var m = milliseconds / 1000 / 60;
        
        return $"{(m > 0 ? m + ":" : "")}{s:00}.{ms:000}";
    }

    /// <summary>
    /// Formats the given milliseconds into human readable time difference.
    /// </summary>
    /// <param name="milliseconds">The time in milliseconds.</param>
    /// <returns></returns>
    public static string FormatTimeAsDelta(int milliseconds)
    {
        var ms = milliseconds % 1000;
        var s = milliseconds / 1000 % 60;
        return $"+ {s:0}.{ms:000}";
    }

    public static TextFormatter FormatPlayerChatMessage(string nickname, string message)
    {
        var formattedMessage = new TextFormatter()
            .AddText("[")
            .AddText(text => text.AsIsolated().AddText(nickname))
            .AddText("] ")
            .AddText(text => text.AsIsolated().AddText(message));

        return formattedMessage;
    }
}
