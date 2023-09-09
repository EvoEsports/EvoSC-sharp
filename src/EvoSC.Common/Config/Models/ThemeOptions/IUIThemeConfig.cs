using System.ComponentModel;
using Config.Net;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Config.Models.ThemeOptions;

public interface IUIThemeConfig
{
    [Description("Primary color used for accents/highlights in UI.")]
    [Option(Alias = "primaryColor", DefaultValue = "1253a3")]
    public string PrimaryColor { get; }
    
    [Description("Default background color for UI.")]
    [Option(Alias = "backgroundColor", DefaultValue = "24262f")]
    public string BackgroundColor { get; }
    
    [Description("Background color for widget headers.")]
    [Option(Alias = "headerBackgroundColor", DefaultValue = "d41e67")]
    public string HeaderBackgroundColor { get; }
    
    [Description("Colored logo to be displayed on UI elements like headers.")]
    [Option(Alias = "logoUrl", DefaultValue = "https://liquipedia.net/commons/images/thumb/c/cb/XPEvo_allmode.png/600px-XPEvo_allmode.png")]
    public string LogoUrl { get; }
    
    [Description("White logo to be displayed on UI elements like headers.")]
    [Option(Alias = "logoWhiteUrl", DefaultValue = "https://cdn.discordapp.com/attachments/764035004048080916/1150113831331307540/XPEvoW100px.png")]
    public string LogoWhiteUrl { get; }
    
    public IUIScoreboardThemeConfig Scoreboard { get; }
}
