using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.RoundRankingModule.Themes;

[Theme(Name = "Round Ranking", Description = "Default theme for the round rankings widget.")]
public class DefaultRoundRankingTheme : Theme<DefaultRoundRankingTheme>
{ 
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.RoundRankingModule.Widget.AccentColor").To((int pos) => pos switch
        {
            1 => theme.Gold,
            2 => theme.Silver,
            3 => theme.Bronze,
            _ => theme.UI_AccentPrimary
        });

        Set("UI.RoundRankingModule.Widget.Row.Bg").To(theme.UI_BgPrimary);
        Set("UI.RoundRankingModule.Widget.Row.Bg.Opacity").To(theme.UI_Widget_Body_Bg_Opacity);
        Set("UI.RoundRankingModule.Widget.Row.Bg.Highlight").To(ColorUtils.Lighten(theme.UI_BgPrimary));

        return Task.CompletedTask;
    }
}
