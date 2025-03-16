using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Chip Component", Description = "Default theme for the Chip component.")]
public class DefaultChipTheme : Theme<DefaultChipTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.Chip.Default.Bg").To(ColorUtils.Lighten(theme.UI_BgHighlight));
        Set("UI.Chip.Default.Text").To(theme.UI_TextSecondary);

        return Task.CompletedTask;
    }
}
