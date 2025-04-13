using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Window Component", Description = "Default theme for the Window component.")]
public class DefaultWindowTheme : Theme<DefaultWindowTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.Window.Default.Title.Icon").To(ColorUtils.SetLightness(theme.UI_AccentSecondary, 65));
        Set("UI.Window.Default.Title.Text").To(ColorUtils.SetLightness(theme.UI_TextPrimary, 65));
        
        Set("UI.Window.Header.Bg").To(theme.UI_HeaderBg);
        Set("UI.Window.Header.Bg.Opacity").To(1.0);
        Set("UI.Window.Header.Icon").To(theme.UI_TextPrimary);
        Set("UI.Window.Header.Title").To(theme.UI_TextPrimary);
        Set("UI.Window.Header.Separator").To(theme.UI_AccentPrimary);
        Set("UI.Window.Header.Separator.Opacity").To(1.0);
        
        Set("UI.Window.Body.Bg").To(theme.UI_BgPrimary);
        Set("UI.Window.Body.Bg.Opacity").To(1.0);

        return Task.CompletedTask;
    }
}
