using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.LiveRankingModule.Themes;

[Theme(Name = "Live Ranking", Description = "Default theme for the Live Ranking module.")]
public class DefaultLiveRankingTheme : Theme<DefaultLiveRankingTheme>
{ 
    private readonly dynamic _theme;

    public DefaultLiveRankingTheme(IThemeManager theme) => _theme = theme.Theme;
    
    public override Task ConfigureAsync()
    {
        Set("LiveRankingModule.LiveRanking.Default.BgHeaderGrad1").To(_theme.UI_BgPrimary);
        Set("LiveRankingModule.LiveRanking.Default.BgHeaderGrad2").To(ColorUtils.Darken(_theme.UI_BgPrimary));
        Set("LiveRankingModule.LiveRanking.Default.BgContent").To(_theme.UI_BgSecondary);
        Set("LiveRankingModule.LiveRanking.Default.Logo").To(_theme.UI_LogoLight);
        
        Set("LiveRankingModule.CurrentPlayer.Default.Text").To(_theme.Black);
        Set("LiveRankingModule.CurrentPlayer.Default.Bg").To(_theme.Gold);
        Set("LiveRankingModule.CurrentPlayer.Default.Gradient").To(_theme.UI_BgSecondary);
        
        Set("LiveRankingModule.PlayerScore.Default.Bg").To(ColorUtils.Lighten(_theme.UI_BgSecondary));
        
        return Task.CompletedTask;
    }
}
