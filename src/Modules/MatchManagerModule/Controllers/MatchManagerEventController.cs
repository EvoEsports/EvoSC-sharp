using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule.Controllers;

[Controller]
public class MatchManagerEventController(IMatchControlService matchControlService)
    : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.Scores)]
    public async Task OnScoresAsync(object sender, ScoresEventArgs args)
    {
        var team1 = args.Teams.First();
        var team2 = args.Teams.Skip(1).First();

        await matchControlService.UpdateTeamPointsAsync(
            team1?.MapPoints ?? 0,
            team1?.MatchPoints ?? 0,
            team2?.MapPoints ?? 0,
            team2?.MatchPoints ?? 0
        );
    }
}
