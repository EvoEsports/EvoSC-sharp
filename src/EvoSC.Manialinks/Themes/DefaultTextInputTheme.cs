using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "TextInput Component", Description = "Default theme for the TextInput component.")]
public class DefaultTextInputTheme : Theme<DefaultTextInputTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.TextField.Default.Text").To(theme.UI_TextPrimary);
        Set("UI.TextField.Default.Bg").To(theme.UI_BgHighlight);
        Set("UI.TextField.Default.Border").To(theme.UI_AccentSecondary);
        Set("UI.TextField.Default.Placeholder").To(theme.UI_TextSecondary);
        
        return Task.CompletedTask;
    }
}
