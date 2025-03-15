using System.Globalization;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Util;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Button Component", Description = "Default theme for the Button component.")]
public class DefaultButtonTheme(IThemeManager themeManager) : Theme<DefaultButtonTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.Button.Default.Bg").To(theme.UI_SurfaceBgSecondary);
        Set("UI.Button.Default.BgFocus").To(ColorUtils.Darken(theme.UI_SurfaceBgSecondary));
        Set("UI.Button.Default.Text").To(theme.UI_TextSecondary);
        
        Set("UI.Button.Secondary.Bg").To(theme.UI_SurfaceBgPrimary);
        Set("UI.Button.Secondary.BgFocus").To(ColorUtils.Lighten(theme.UI_SurfaceBgPrimary));
        Set("UI.Button.Secondary.Text").To(theme.UI_TextPrimary);
        
        Set("UI.Button.Accent.Bg").To(theme.UI_AccentPrimary);
        Set("UI.Button.Accent.BgFocus").To(ColorUtils.Lighten(theme.UI_AccentPrimary));
        Set("UI.Button.Accent.Text").To(new GlobalManialinkUtils(themeManager).TypeToColorText(theme.UI_AccentPrimary));

        Set("UI.Button.Size.Normal").To(5.0);
        Set("UI.Button.Size.Small").To(4.0);
        Set("UI.Button.Size.Big").To(6.0);
        
        Set("UI.Button.Disabled.Bg").To(ColorUtils.SetLightness(theme.UI_SurfaceBgSecondary, 50));
        Set("UI.Button.Disabled.Text").To(ColorUtils.SetLightness(theme.UI_TextSecondary, 50));
        
        return Task.CompletedTask;
    }

    public override Task ConfigureDynamicAsync(dynamic theme)
    {
        Set("UI.Button.Bg").To((string type) => type.ToLower(CultureInfo.InvariantCulture) switch
        {
            "primary" => theme.UI_Button_Default_Bg,
            "secondary" => theme.UI_Button_Secondary_Bg,
            "accent" => theme.UI_Button_Accent_Bg,
            _ => theme.UI_Button_Default_Bg,
        });
        
        Set("UI.Button.Size").To((string type, double custom) => type.ToLower(CultureInfo.InvariantCulture) switch
        {
            "normal" => theme.UI_Button_Size_Normal,
            "small" => theme.UI_Button_Size_Small,
            "big" => theme.UI_Button_Size_Big,
            "custom" => custom,
            _ => theme.UI_Button_Size_Normal,
        });
        
        return Task.CompletedTask;
    }
}
