using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "RadioButton Component", Description = "Default theme for the RadioButton component.")]
public class DefaultRadioButtonTheme : Theme<DefaultRadioButtonTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.RadioButton.Default.Text").To(theme.UI_TextPrimary);
        Set("UI.RadioButton.Default.CheckSize").To(2);

        return Task.CompletedTask;
    }
}
