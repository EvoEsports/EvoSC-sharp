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
        Set("UI.Button.Default.Bg").To(_theme.UI_BgPrimary);
        Set("UI.Button.Default.BgFocus").To(ColorUtils.Lighten(_theme.UI_BgPrimary));
        Set("UI.Button.Default.Text").To(_theme.UI_TextPrimary);
        Set("UI.Button.Default.DisabledBg").To(ColorUtils.GrayScale(_theme.UI_BgPrimary));
        Set("UI.Button.Default.DisabledText").To(ColorUtils.GrayScale(_theme.UI_TextPrimary));
        
        Set("UI.Button.Secondary.Bg").To(_theme.UI_BgSecondary);
        Set("UI.Button.Secondary.BgFocus").To(ColorUtils.Lighten(_theme.UI_BgSecondary));
        Set("UI.Button.Secondary.Text").To(_theme.UI_TextSecondary);
        Set("UI.Button.Secondary.DisabledBg").To(ColorUtils.GrayScale(_theme.UI_BgSecondary));
        Set("UI.Button.Secondary.DisabledText").To(ColorUtils.GrayScale(_theme.UI_TextSecondary));
        
        return Task.CompletedTask;
    }
}
