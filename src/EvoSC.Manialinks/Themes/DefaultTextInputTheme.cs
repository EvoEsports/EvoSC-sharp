using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "TextInput Component", Description = "Default theme for the TextInput component.")]
public class DefaultTextInputTheme(IThemeManager theme) : Theme<DefaultTextInputTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("UI.TextField.Default.Text").To(_theme.UI_TextPrimary);
        Set("UI.TextField.Default.Bg").To(_theme.UI_BgHighlight);
        Set("UI.TextField.Default.Border").To(_theme.UI_AccentSecondary);
        Set("UI.TextField.Default.Placeholder").To(_theme.UI_TextSecondary);
        
        return Task.CompletedTask;
    }
}
