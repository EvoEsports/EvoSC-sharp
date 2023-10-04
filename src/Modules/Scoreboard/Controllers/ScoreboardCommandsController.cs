using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.Scoreboard.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard.Controllers;

[Controller]
public class ScoreboardCommandsController : EvoScController<ICommandInteractionContext>
{
    private readonly IScoreboardService _scoreboardService;

    public ScoreboardCommandsController(IScoreboardService scoreboardService) =>
        _scoreboardService = scoreboardService;
    
    [ChatCommand("scoreboard", "[Command.ShowScoreboard]")]
    public async Task ShowScoreboardAsync()
    {
        await _scoreboardService.ShowScoreboardAsync(Context.Player);
    }
}
