using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Button Component", Description = "Default theme for the Button component.")]
public class DefaultButtonTheme(IThemeManager theme) : Theme<DefaultButtonTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("UI.Button.Default.Bg").To(_theme.UI_SurfaceBgSecondary);
        Set("UI.Button.Default.BgFocus").To(ColorUtils.Darken(_theme.UI_SurfaceBgSecondary));
        Set("UI.Button.Default.Text").To(_theme.UI_TextSecondary);
        
        Set("UI.Button.Secondary.Bg").To(_theme.UI_SurfaceBgPrimary);
        Set("UI.Button.Secondary.BgFocus").To(ColorUtils.Lighten(_theme.UI_SurfaceBgPrimary));
        Set("UI.Button.Secondary.Text").To(_theme.UI_TextPrimary);
        
        Set("UI.Button.Disabled.Bg").To(ColorUtils.SetLightness(_theme.UI_SurfaceBgPrimary, 50));
        Set("UI.Button.Disabled.Text").To(ColorUtils.SetLightness(_theme.UI_TextPrimary, 50));
        
        return Task.CompletedTask;
    }
}
