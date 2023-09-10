using System.ComponentModel;
using Config.Net;

namespace EvoSC.Common.Config.Models.ThemeOptions;

public interface IUIScoreboardThemeConfig
{
    [Description("Background color of the position box.")]
    [Option(Alias = "positionBackgroundColor", DefaultValue = "383b4a")]
    public string PositionBackgroundColor { get; }
    
    [Description("Player row background color on hover.")]
    [Option(Alias = "playerRowHighlightColor", DefaultValue = "767987")]
    public string PlayerRowHighlightColor { get; }
}
