using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.OpenPlanetModule.Themes;

[Theme(Name = "OpenPlanet", Description = "Default theme for the OpenPlanet module.")]
public class DefaultOpenPlanetTheme : Theme<DefaultOpenPlanetTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("OpenPlanetModule.WarningWindow.TextWarning").To(theme.Warning);
        Set("OpenPlanetModule.WarningWindow.TextHighlight").To("99ddff");
        Set("OpenPlanetModule.WarningWindow.Border").To(theme.White);
        Set("OpenPlanetModule.WarningWindow.BgSecondary").To(ColorUtils.Lighten(theme.UI_BgHighlight));
        
        return Task.CompletedTask;
    }
}
