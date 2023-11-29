using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.MatchRankingModule.Themes;

[Theme(Name = "Match Ranking", Description = "Default theme for the match ranking module.")]
public class DefaultMatchRankingTheme : Theme<DefaultMatchRankingTheme>
{
    private readonly dynamic _theme;

    public DefaultMatchRankingTheme(IThemeManager theme) => _theme = theme.Theme;
    
    public override Task ConfigureAsync()
    {
        Set("MatchRankingModule.MatchRanking.Default.BgHeaderGrad1").To(_theme.UI_BgPrimary);
        Set("MatchRankingModule.MatchRanking.Default.BgHeaderGrad2").To(ColorUtils.Darken(_theme.UI_BgPrimary));
        Set("MatchRankingModule.MatchRanking.Default.BgContent").To(_theme.UI_BgSecondary);
        Set("MatchRankingModule.MatchRanking.Default.BgRow").To(ColorUtils.Lighten(_theme.UI_BgSecondary));
        Set("MatchRankingModule.MatchRanking.Default.Logo").To(_theme.UI_LogoLight);
        
        Set("MatchRankingModule.MatchRanking.Default.Bg").To(ColorUtils.Lighten(_theme.UI_BgSecondary));
        
        return Task.CompletedTask;
    }
}
