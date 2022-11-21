using System.Text.RegularExpressions;

namespace EvoSC.Common.Util;

public static class FormattingUtils
{
    private static readonly Regex TmTextFormatPattern = new Regex(@"\$((L|H)\[.+\]|[\da-f]{3}|[\w\$\<\>]{1})",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static string CleanTmFormatting(string text) =>
        TmTextFormatPattern.Replace(text, "");
}
