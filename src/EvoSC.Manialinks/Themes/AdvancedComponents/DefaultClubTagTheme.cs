using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes.AdvancedComponents;

[Theme(Name = "ClubTag Component", Description = "Default theme for the ClubTag component.")]
public class DefaultClubTagTheme(IThemeManager theme) : Theme<DefaultClubTagTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("UI.ClubTag.Bg").To(_theme.UI_BgHighlight);

        return Task.CompletedTask;
    }
}
