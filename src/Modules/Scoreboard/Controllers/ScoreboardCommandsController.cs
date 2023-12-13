using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.Scoreboard.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard.Controllers;

[Controller]
public class ScoreboardCommandsController(IScoreboardService scoreboardService) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("scoreboard", "[Command.ShowScoreboard]")]
    public async Task ShowScoreboardAsync()
    {
        await scoreboardService.ShowScoreboardAsync(Context.Player);
    }
}
