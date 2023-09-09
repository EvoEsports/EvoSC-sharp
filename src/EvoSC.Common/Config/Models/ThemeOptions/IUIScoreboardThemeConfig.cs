using System.ComponentModel;
using Config.Net;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Config.Models.ThemeOptions;

public interface IUIScoreboardThemeConfig
{
    [Description("Background color of the position box.")]
    [Option(Alias = "positionBackgroundColor", DefaultValue = "383b4a")]
    public string PositionBackgroundColor { get; }
    
    [Description("Player row background color on hover")]
    [Option(Alias = "playerRowHighlightColor", DefaultValue = "54596e")]
    public string PlayerRowHighlightColor { get; }
}
