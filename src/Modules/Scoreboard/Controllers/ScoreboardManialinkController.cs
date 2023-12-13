using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.Scoreboard.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard.Controllers;

[Controller]
public class ScoreboardManialinkController(IScoreboardService scoreboardService) : ManialinkController
{
    public Task ResendScoreboardAsync() => scoreboardService.ShowScoreboardAsync(Context.Player);
}
