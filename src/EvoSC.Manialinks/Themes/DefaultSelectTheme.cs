using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Select Component", Description = "Default theme for the Select & SelectItem components.")]
public class DefaultSelectTheme : Theme<DefaultSelectTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.Select.Default.Bg").To(theme.UI_BgHighlight);
        Set("UI.Select.Default.Border").To(theme.UI_AccentSecondary);
        Set("UI.SelectItem.Default.Bg").To(theme.UI_BgPrimary);

        return Task.CompletedTask;
    }
}
