using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Modules.Official.Scoreboard.Themes;

[Theme(Name = "Scoreboard", Description = "Default theme for the scoreboard.")]
public class DefaultScoreboardTheme : Theme<DefaultScoreboardTheme>
{
    public override Task ConfigureAsync()
    {
        Set("UI.Scoreboard.PositionBackgroundColor").To("383b4a");
        Set("UI.Scoreboard.PlayerRowhighlightColor").To("767987");

        return Task.CompletedTask;
    }
}
