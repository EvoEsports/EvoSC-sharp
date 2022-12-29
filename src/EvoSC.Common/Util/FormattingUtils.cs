using System.Text.RegularExpressions;

namespace EvoSC.Common.Util;

public static class FormattingUtils
{
    /// <summary>
    /// Pattern to match TrackMania's formatting syntax.
    /// </summary>
    private static readonly Regex TmTextFormatPattern = new Regex(@"\$((L|H)\[.+\]|[\da-f]{3}|[\w\$\<\>]{1})",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>
    /// Removes all TrackMania formatting from a string.
    /// </summary>
    /// <param name="text">The text to clean up.</param>
    /// <returns></returns>
    public static string CleanTmFormatting(string text) =>
        TmTextFormatPattern.Replace(text, "");

    public static string FormatTime(int milliseconds)
    {
        var ms = milliseconds % 1000;
        var s = milliseconds / 1000 % 60;
        var m = milliseconds / 1000 / 60;
        
        return $"{(m > 0 ? m + ":" : "")}{s:00}.{ms:000}";
    }
}
