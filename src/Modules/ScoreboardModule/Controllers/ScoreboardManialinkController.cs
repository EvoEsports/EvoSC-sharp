using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;

namespace EvoSC.Modules.Official.ScoreboardModule.Controllers;

[Controller]
public class ScoreboardManialinkController(IScoreboardService scoreboardService) : ManialinkController
{
    public Task ResendScoreboardAsync() => scoreboardService.ShowScoreboardAsync(Context.Player);
}
