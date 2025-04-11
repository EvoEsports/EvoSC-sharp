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
        
        /* Set("UI.Window.Default.Bg").To(_theme.UI_BgHighlight);
        Set("UI.Window.Default.Header.Bg").To(_theme.UI_BgPrimary);
        Set("UI.Window.Default.Header.BgFocus").To(ColorUtils.Lighten(_theme.UI_BgPrimary));
        Set("UI.Window.Default.Title.Text").To(ColorUtils.Lighten(_theme.UI_TextPrimary));
        Set("UI.Window.Default.CloseBtn.Text").To(ColorUtils.Lighten(_theme.UI_TextPrimary));
        Set("UI.Window.Default.MinimizeBtn.Text").To(ColorUtils.Lighten(_theme.UI_TextPrimary));
        
        Set("UI.Window.Secondary.Bg").To(_theme.UI_BgHighlight);
        Set("UI.Window.Secondary.Header.Bg").To(_theme.UI_BgHighlight);
        Set("UI.Window.Secondary.Header.BgFocus").To(ColorUtils.Lighten(_theme.UI_BgHighlight));
        Set("UI.Window.Secondary.Title.Text").To(ColorUtils.Lighten(_theme.UI_TextPrimary));
        Set("UI.Window.Secondary.CloseBtn.Text").To(ColorUtils.Lighten(_theme.UI_TextPrimary));
        Set("UI.Window.Secondary.MinimizeBtn.Text").To(ColorUtils.Lighten(_theme.UI_TextPrimary));
        
        Set("UI.Window.Default.Header.Bg").To(_theme.UI_BgHighlight); */

        return Task.CompletedTask;
    }
}
