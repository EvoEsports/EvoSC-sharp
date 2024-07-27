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
    public async Task OnScoresAsync(object sender, ScoresEventArgs eventArgs)
    {
        var isTeamsModeActive = await teamInfoService.GetModeIsTeamsAsync();

        if (!eventArgs.UseTeams)
        {
            if (!isTeamsModeActive)
            {
                return;
            }

            await teamInfoService.SetModeIsTeamsAsync(false);
            await teamInfoService.HideTeamInfoWidgetEveryoneAsync();

            return;
        }

        if (!isTeamsModeActive)
        {
            await teamInfoService.SetModeIsTeamsAsync(true);
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
    public async Task OnRoundStartAsync(object sender, RoundEventArgs args)
    {
        if (!await teamInfoService.GetModeIsTeamsAsync())
        {
            return;
        }

        await teamInfoService.UpdateRoundNumberAsync(args.Count);
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public async Task OnPodiumStartAsync(object sender, PodiumEventArgs args)
    {
        if (!await teamInfoService.GetModeIsTeamsAsync())
        {
            return;
        }

        await teamInfoService.HideTeamInfoWidgetEveryoneAsync();
    }

    [Subscribe(GbxRemoteEvent.EndMap)]
    public async Task OnEndMap(object sender, MapGbxEventArgs args)
    {
        if (!await teamInfoService.GetModeIsTeamsAsync())
        {
            return;
        }

        await teamInfoService.HideTeamInfoWidgetEveryoneAsync();
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args)
    {
        if (!await teamInfoService.GetModeIsTeamsAsync() || !await teamInfoService.GetWidgetVisibilityAsync())
        {
            return;
        }

        await teamInfoService.SendTeamInfoWidgetAsync(args.Login);
    }
}
