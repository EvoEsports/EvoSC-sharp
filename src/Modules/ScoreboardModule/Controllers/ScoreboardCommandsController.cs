using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;

namespace EvoSC.Modules.Official.ScoreboardModule.Controllers;

[Controller]
public class ScoreboardCommandsController(IScoreboardService scoreboardService) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("scoreboard", "[Command.ShowScoreboard]")]
    public async Task ShowScoreboardAsync()
    {
        await scoreboardService.ShowScoreboardAsync(Context.Player);
    }
}
