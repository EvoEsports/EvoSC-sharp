using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Select Component", Description = "Default theme for the Select & SelectItem components.")]
public class DefaultSelectTheme(IThemeManager theme) : Theme<DefaultSelectTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("UI.Select.Default.Bg").To(_theme.UI_BgHighlight);
        Set("UI.Select.Default.Border").To(_theme.UI_AccentSecondary);
        Set("UI.SelectItem.Default.Bg").To(_theme.UI_BgPrimary);

        return Task.CompletedTask;
    }
}
