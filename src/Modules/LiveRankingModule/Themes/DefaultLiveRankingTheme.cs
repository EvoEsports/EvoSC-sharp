using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.LiveRankingModule.Themes;

[Theme(Name = "Live Ranking", Description = "Default theme for the live rankings widget.")]
public class DefaultLiveRankingTheme(IThemeManager theme) : Theme<DefaultLiveRankingTheme>
{ 
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("UI.LiveRankingModule.Widget.AccentColor").To((int pos) => pos switch
        {
            1 => _theme.Gold,
            2 => _theme.Silver,
            3 => _theme.Bronze,
            _ => _theme.UI_AccentPrimary
        });

        Set("UI.LiveRankingModule.Widget.RowBg").To(_theme.UI_BgPrimary);
        Set("UI.LiveRankingModule.Widget.RowBgHighlight").To(ColorUtils.Lighten(_theme.UI_BgPrimary));

        return Task.CompletedTask;
    }
}
