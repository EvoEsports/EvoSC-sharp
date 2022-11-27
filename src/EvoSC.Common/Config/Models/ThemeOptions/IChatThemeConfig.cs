using System.ComponentModel;
using Config.Net;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Config.Models.ThemeOptions;

public interface IChatThemeConfig
{
    [Description("The primary text color to use.")]
    [Option(Alias = "primaryTextColor", DefaultValue = "fff")]
    public TextColor PrimaryColor { get; }
    
    [Description("The secondary text color to use.")]
    [Option(Alias = "secondaryTextColor", DefaultValue = "eee")]
    public TextColor SecondaryColor { get; }
    
    [Description("The color to use for info messages.")]
    [Option(Alias = "infoTextColor", DefaultValue = "29b")]
    public TextColor InfoColor { get; }
    
    [Description("The color to use for error messages.")]
    [Option(Alias = "errorTextColor", DefaultValue = "c44")]
    public TextColor ErrorColor { get; }
    
    [Description("The color to use for warning messages.")]
    [Option(Alias = "warningTextColor", DefaultValue = "e83")]
    public TextColor WarningColor { get; }
    
    [Description("The color to use for success messages.")]
    [Option(Alias = "successTextColor", DefaultValue = "5b6")]
    public TextColor SuccessColor { get; }
}
