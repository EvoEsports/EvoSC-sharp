using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.MatchRankingModule.Themes;

[Theme(Name = "Match Ranking", Description = "Default theme for the match ranking module.")]
public class DefaultMatchRankingTheme : Theme<DefaultMatchRankingTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("MatchRankingModule.MatchRanking.Default.Text").To(theme.UI_TextPrimary);
        Set("MatchRankingModule.MatchRanking.Default.PositionText").To(theme.UI_TextPrimary);
        Set("MatchRankingModule.MatchRanking.Default.BgHeaderGrad1").To(theme.UI_BgPrimary);
        Set("MatchRankingModule.MatchRanking.Default.BgHeaderGrad2").To(ColorUtils.Darken(theme.UI_BgPrimary));
        Set("MatchRankingModule.MatchRanking.Default.BgContent").To(theme.UI_BgHighlight);
        Set("MatchRankingModule.MatchRanking.Default.BgRow").To(ColorUtils.Lighten(theme.UI_BgHighlight));
        Set("MatchRankingModule.MatchRanking.Default.Logo").To(theme.UI_LogoLight);
        
        Set("MatchRankingModule.MatchRanking.Default.Bg").To(ColorUtils.Lighten(theme.UI_BgHighlight));
        
        return Task.CompletedTask;
    }
}
