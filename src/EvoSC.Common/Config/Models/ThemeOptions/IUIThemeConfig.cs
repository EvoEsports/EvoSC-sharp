﻿using System.ComponentModel;
using Config.Net;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Config.Models.ThemeOptions;

public interface IUIThemeConfig
{
    [Description("Primary color used for accents/highlights in UI.")]
    [Option(Alias = "primaryColor", DefaultValue = "1253a3")]
    public string PrimaryColor { get; }
    
    [Description("Default background color for UI.")]
    [Option(Alias = "backgroundColor", DefaultValue = "111111")]
    public string BackgroundColor { get; }
    
    [Description("Background color for widget headers.")]
    [Option(Alias = "headerBackgroundColor", DefaultValue = "d41e67")]
    public string HeaderBackgroundColor { get; }
    
    [Description("Logo to be displayed on UI elements like headers.")]
    [Option(Alias = "logoUrl", DefaultValue = "https://maptesting.evotm.com/images/xpevo_logo.png")]
    public string LogoUrl { get; }
    
    public IUIScoreboardThemeConfig Scoreboard { get; }
}
