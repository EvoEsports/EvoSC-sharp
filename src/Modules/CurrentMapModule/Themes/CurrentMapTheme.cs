using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.CurrentMapModule.Themes;

[Theme(Name = "Current Map", Description = "Default theme for the Current Map Module.")]
public class CurrentMapTheme : Theme<CurrentMapTheme>
{
    private readonly dynamic _theme;
    
    public CurrentMapTheme(IThemeManager theme)
    {
        _theme = theme.Theme;
    }
    
    public override Task ConfigureAsync()
    {
        Set("CurrentMapModule.CurrentMapWidget.Default.BgHeaderGrad1").To(_theme.UI_BgPrimary);
        Set("CurrentMapModule.CurrentMapWidget.Default.BgHeaderGrad2").To(ColorUtils.Darken(_theme.UI_BgPrimary));
        Set("CurrentMapModule.CurrentMapWidget.Default.BgContent").To(_theme.UI_BgSecondary);
        Set("CurrentMapModule.CurrentMapWidget.Default.Logo").To(_theme.UI_LogoLight);
        
        return Task.CompletedTask;
    }
}
