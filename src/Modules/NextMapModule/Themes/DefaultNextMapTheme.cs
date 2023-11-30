using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.NextMapModule.Themes;

[Theme(Name = "Next Map", Description = "Default theme for the Next Map module.")]
public class DefaultNextMapTheme : Theme<DefaultNextMapTheme>
{
    private readonly dynamic _theme;

    public DefaultNextMapTheme(IThemeManager theme) => _theme = theme.Theme;
    
    public override Task ConfigureAsync()
    {
        Set("NextMapModule.NextMap.Default.BgHeaderGrad1").To(_theme.UI_BgPrimary);
        Set("NextMapModule.NextMap.Default.BgHeaderGrad2").To(ColorUtils.Darken(_theme.UI_BgPrimary));
        Set("NextMapModule.NextMap.Default.BgContent").To(_theme.UI_BgSecondary);
        Set("NextMapModule.NextMap.Default.BgRow").To(ColorUtils.Lighten(_theme.UI_BgSecondary));
        Set("NextMapModule.NextMap.Default.Logo").To(_theme.UI_LogoLight);
        
        return Task.CompletedTask;
    }
}
