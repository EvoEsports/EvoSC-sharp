using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes.AdvancedComponents;

[Theme(Name = "ClubTag Component", Description = "Default theme for the ClubTag component.")]
public class DefaultClubTagTheme : Theme<DefaultClubTagTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.ClubTag.Bg").To(theme.UI_BgHighlight);

        return Task.CompletedTask;
    }
}
