using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "RadioButton Component", Description = "Default theme for the RadioButton component.")]
public class DefaultRadioButtonTheme(IThemeManager theme) : Theme<DefaultRadioButtonTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("UI.RadioButton.Default.Text").To(_theme.UI_TextPrimary);
        Set("UI.RadioButton.Default.CheckSize").To(2);

        return Task.CompletedTask;
    }
}
