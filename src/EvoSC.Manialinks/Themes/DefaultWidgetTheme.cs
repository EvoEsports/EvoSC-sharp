using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Widget Component", Description = "Default theme for the Widget component.")]
public class DefaultWidgetTheme(IThemeManager theme) : Theme<DefaultWidgetTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("UI.Widget.Header.Text").To(_theme.UI_TextPrimary);
        Set("UI.Widget.Header.Bg").To(_theme.UI_HeaderBg);
        Set("UI.Widget.Header.Bg.Opacity").To(0.95);
        Set("UI.Widget.Header.Bg.Image").To("");
        
        Set("UI.Widget.Body.Bg").To(_theme.UI_BgPrimary);
        Set("UI.Widget.Body.Bg.Opacity").To(0.9);
        
        Set("UI.Widget.Accent").To(_theme.UI_AccentPrimary);

        return Task.CompletedTask;
    }
}
