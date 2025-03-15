using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.LiveRankingModule.Themes;

[Theme(Name = "Live Ranking", Description = "Default theme for the live rankings widget.")]
public class DefaultLiveRankingTheme : Theme<DefaultLiveRankingTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.LiveRankingModule.Widget.AccentColor").To((int pos) => pos switch
        {
            1 => theme.Gold,
            2 => theme.Silver,
            3 => theme.Bronze,
            _ => theme.UI_AccentPrimary
        });

        Set("UI.LiveRankingModule.Widget.RowBg").To(theme.UI_BgPrimary);
        Set("UI.LiveRankingModule.Widget.RowBgHighlight").To(ColorUtils.Lighten(theme.UI_BgPrimary));

        return Task.CompletedTask;
    }
}
