namespace EvoSC.Common.Util.TextFormatting;

/// <summary>
/// TrackMania formatting syntax.
/// </summary>
public static class TextStyle
{
    /// <summary>
    /// Bold text style.
    /// </summary>
    public static string Bold = "$o";
    
    /// <summary>
    /// Italic text style.
    /// </summary>
    public static string Italic = "$i";
    
    /// <summary>
    /// Wide text style.
    /// </summary>
    public static string Wide = "$w";
    
    /// <summary>
    /// Narrow text style.
    /// </summary>
    public static string Narrow = "$n";
    
    /// <summary>
    /// Force uppercase.
    /// </summary>
    public static string Uppercase = "$t";
    
    /// <summary>
    /// Drop shadow on the text.
    /// </summary>
    public static string DropShadow = "$s";
    
    /// <summary>
    /// Make the text a clickable link.
    /// </summary>
    public static string Link = "$l";
    
    /// <summary>
    /// Reset previous colors to default.
    /// </summary>
    public static string ColorReset = "$g";
    
    /// <summary>
    /// Reset all formatting to default.
    /// </summary>
    public static string StyleReset = "$z";
    
    /// <summary>
    /// Start of isolated formatting.
    /// </summary>
    public static string IsolationStart = "$<";
    
    /// <summary>
    /// End of isolated formatting.
    /// </summary>
    public static string IsolationEnd = "$>";
}
