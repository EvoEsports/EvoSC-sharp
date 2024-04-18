using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Window Component", Description = "Default theme for the Window component.")]
public class DefaultWindowTheme(IThemeManager theme) : Theme<DefaultWindowTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("UI.Window.Default.Bg").To(_theme.UI_BgHighlight);
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
        
        Set("UI.Window.Default.Header.Bg").To(_theme.UI_BgHighlight);

        return Task.CompletedTask;
    }
}
