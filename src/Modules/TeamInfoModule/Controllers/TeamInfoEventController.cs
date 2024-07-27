using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Models;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.TeamInfoModule.Controllers;

[Controller]
public class TeamInfoEventController(ITeamInfoService teamInfoService) : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.Scores)]
    public async Task OnScores(object sender, ScoresEventArgs eventArgs)
    {
        var isTeamsModeActive = await teamInfoService.GetModeIsTeams();

        if (!eventArgs.UseTeams)
        {
            if (!isTeamsModeActive)
            {
                return;
            }

            await teamInfoService.SetModeIsTeams(false);
            await teamInfoService.HideTeamInfoWidgetEveryoneAsync();

            return;
        }

        if (!isTeamsModeActive)
        {
            await teamInfoService.SetModeIsTeams(true);
        }

        var teamInfos = eventArgs.Teams.ToList();
        var team1Points = teamInfos[0]!.MatchPoints;
        var team2Points = teamInfos[1]!.MatchPoints;

        if (eventArgs.Section is ModeScriptSection.EndRound or ModeScriptSection.Undefined)
        {
            await teamInfoService.UpdatePointsAsync(team1Points, team2Points);
        }
    }

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public async Task OnRoundStart(object sender, RoundEventArgs args)
        => await teamInfoService.UpdateRoundNumberAsync(args.Count);

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public async Task OnPodiumStart(object sender, PodiumEventArgs args)
        => await teamInfoService.HideTeamInfoWidgetEveryoneAsync();

    [Subscribe(GbxRemoteEvent.EndMap)]
    public async Task OnEndMap(object sender, MapGbxEventArgs args)
        => await teamInfoService.HideTeamInfoWidgetEveryoneAsync();

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnect(object sender, PlayerConnectGbxEventArgs args)
        => await teamInfoService.SendTeamInfoWidgetAsync(args.Login);
}
