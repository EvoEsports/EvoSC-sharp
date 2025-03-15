using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Widget Component", Description = "Default theme for the Widget component.")]
public class DefaultWidgetTheme : Theme<DefaultWidgetTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.Widget.Header.Text").To(theme.UI_TextPrimary);
        Set("UI.Widget.Header.Bg").To(theme.UI_HeaderBg);
        Set("UI.Widget.Header.Bg.Opacity").To(0.95);
        Set("UI.Widget.Header.Bg.Image").To("");
        
        Set("UI.Widget.Body.Bg").To(theme.UI_BgPrimary);
        Set("UI.Widget.Body.Bg.Opacity").To(0.9);
        
        Set("UI.Widget.Accent").To(theme.UI_AccentPrimary);

        return Task.CompletedTask;
    }
}
