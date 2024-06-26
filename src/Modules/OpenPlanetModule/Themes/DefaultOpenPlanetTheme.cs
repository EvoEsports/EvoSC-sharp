﻿using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.OpenPlanetModule.Themes;

[Theme(Name = "OpenPlanet", Description = "Default theme for the OpenPlanet module.")]
public class DefaultOpenPlanetTheme(IThemeManager theme) : Theme<DefaultOpenPlanetTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("OpenPlanetModule.WarningWindow.TextWarning").To(_theme.Warning);
        Set("OpenPlanetModule.WarningWindow.TextHighlight").To("99ddff");
        Set("OpenPlanetModule.WarningWindow.Border").To(_theme.White);
        Set("OpenPlanetModule.WarningWindow.BgSecondary").To(ColorUtils.Lighten(_theme.UI_BgHighlight));
        
        return Task.CompletedTask;
    }
}
