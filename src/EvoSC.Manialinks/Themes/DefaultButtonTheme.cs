using System.Globalization;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Util;

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
        
        Set("UI.Button.Accent.Bg").To(_theme.UI_AccentPrimary);
        Set("UI.Button.Accent.BgFocus").To(ColorUtils.Lighten(_theme.UI_AccentPrimary));
        Set("UI.Button.Accent.Text").To(new GlobalManialinkUtils(theme).TypeToColorText(_theme.UI_AccentPrimary));

        Set("UI.Button.Bg").To((string type) => type.ToLower(CultureInfo.InvariantCulture) switch
        {
            "primary" => ThemeOptions["UI.Button.Default.Bg"].ToString(),
            "secondary" => ThemeOptions["UI.Button.Secondary.Bg"].ToString(),
            "accent" => ThemeOptions["UI.Button.Accent.Bg"].ToString(),
            _ => ThemeOptions["UI.Button.Default.Bg"].ToString(),
        });

        Set("UI.Button.Size.Normal").To(5.0);
        Set("UI.Button.Size.Small").To(4.0);
        Set("UI.Button.Size.Big").To(6.0);
        
        Set("UI.Button.Size").To((string type, double custom) => type.ToLower(CultureInfo.InvariantCulture) switch
        {
            "normal" => (double)ThemeOptions["UI.Button.Size.Normal"],
            "small" => (double)ThemeOptions["UI.Button.Size.Small"],
            "big" => (double)ThemeOptions["UI.Button.Size.Big"],
            "custom" => custom,
            _ => (double)ThemeOptions["UI.Button.Size.Normal"],
        });
        
        Set("UI.Button.Disabled.Bg").To(ColorUtils.SetLightness(_theme.UI_SurfaceBgPrimary, 50));
        Set("UI.Button.Disabled.Text").To(ColorUtils.SetLightness(_theme.UI_TextPrimary, 50));
        
        return Task.CompletedTask;
    }
}
