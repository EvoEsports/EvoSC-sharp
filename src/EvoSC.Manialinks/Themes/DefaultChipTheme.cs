using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Chip Component", Description = "Default theme for the Chip component.")]
public class DefaultChipTheme(IThemeManager theme) : Theme<DefaultChipTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("UI.Chip.Default.Bg").To(ColorUtils.Lighten(_theme.UI_BgSecondary));
        Set("UI.Chip.Default.Text").To(_theme.UI_TextSecondary);

        return Task.CompletedTask;
    }
}
