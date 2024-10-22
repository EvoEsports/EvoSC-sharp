using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.RoundRankingModule.Themes;

[Theme(Name = "Round Ranking", Description = "Default theme for the round rankings widget.")]
public class DefaultRoundRankingTheme(IThemeManager theme) : Theme<DefaultRoundRankingTheme>
{ 
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("UI.RoundRankingModule.Widget.AccentColor").To((int pos) => pos switch
        {
            1 => _theme.Gold,
            2 => _theme.Silver,
            3 => _theme.Bronze,
            _ => _theme.UI_AccentPrimary
        });

        Set("UI.RoundRankingModule.Widget.RowBg").To(_theme.UI_BgPrimary);
        Set("UI.RoundRankingModule.Widget.RowBgHighlight").To(ColorUtils.Lighten(_theme.UI_BgPrimary));

        return Task.CompletedTask;
    }
}
