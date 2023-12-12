using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.CurrentMapModule.Themes;

[Theme(Name = "Current Map", Description = "Default theme for the Current Map Module.")]
public class DefaultCurrentMapTheme : Theme<DefaultCurrentMapTheme>
{
    private readonly dynamic _theme;
    
    public DefaultCurrentMapTheme(IThemeManager theme)
    {
        _theme = theme.Theme;
    }
    
    public override Task ConfigureAsync()
    {
        Set("CurrentMapModule.CurrentMapWidget.Default.BgHeaderGrad1").To(_theme.UI_BgPrimary);
        Set("CurrentMapModule.CurrentMapWidget.Default.BgHeaderGrad2").To(ColorUtils.Darken(_theme.UI_BgPrimary));
        Set("CurrentMapModule.CurrentMapWidget.Default.BgContent").To(_theme.UI_BgSecondary);
        Set("CurrentMapModule.CurrentMapWidget.Default.BgRow").To(ColorUtils.Lighten(_theme.UI_BgSecondary));
        Set("CurrentMapModule.CurrentMapWidget.Default.Logo").To(_theme.UI_LogoLight);
        Set("CurrentMapModule.CurrentMapWidget.Default.Text").To(_theme.UI_TextPrimary);
        
        return Task.CompletedTask;
    }
}
