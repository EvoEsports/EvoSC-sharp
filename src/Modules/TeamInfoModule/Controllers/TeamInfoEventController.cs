using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Models;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.TeamInfoModule.Controllers;

[Controller]
public class TeamInfoEventController(ITeamInfoService teamInfoService, ILogger<TeamInfoEventController> logger)
    : EvoScController<IEventControllerContext>
{
    //Subscribe to team info changed

    [Subscribe(ModeScriptEvent.Scores)]
    public async Task OnScores(object data, ScoresEventArgs eventArgs)
    {
        var teamInfos = eventArgs.Teams.ToList();
        logger.LogInformation("Team info length: {length}", teamInfos.Count);
        logger.LogInformation("Section: {section}", eventArgs.Section.ToString());

        var team1Points = teamInfos[0]!.MapPoints;
        var team2Points = teamInfos[1]!.MapPoints;

        logger.LogInformation("Points: {team1} - {team2}", team1Points, team2Points);

        if (eventArgs.Section == ModeScriptSection.PreEndRound)
        {
            await teamInfoService.UpdateGainedPointsAsync(team1Points, team2Points);
        }
        else
        {
            await teamInfoService.UpdatePointsAsync(team1Points, team2Points);
        }
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public async Task OnPodiumStart(object sender, PodiumEventArgs args)
    {
        await teamInfoService.SetWidgetVisibility(false);
        await teamInfoService.HideTeamInfoWidgetEveryoneAsync();
    }

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public async Task OnRoundStart(object sender, RoundEventArgs args)
    {
        await teamInfoService.UpdateRoundNumber(args.Count);
        await teamInfoService.UpdateGainedPointsAsync(0, 0);
        await teamInfoService.SetWidgetVisibility(true);
        await teamInfoService.RequestScoresFromServerAsync();
        // await teamInfoService.SendTeamInfoWidgetEveryoneAsync();
    }

    // [Subscribe(ModeScriptEvent.EndRoundEnd)]
    // public async Task OnEndRoundEnd(object sender, RoundEventArgs args)
    // {
    //     //TODO: clear gained points
    // }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnect(object sender, PlayerConnectGbxEventArgs args)
    {
        await teamInfoService.SendTeamInfoWidgetAsync(args.Login);
    }

    // [Subscribe(GbxRemoteEvent.EndMap)]
    // public async Task OnEndMap(object sender, MapGbxEventArgs args)
    // {
    //     await teamInfoService.HideTeamInfoWidgetEveryoneAsync();
    // }

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMap(object sender, MapGbxEventArgs args)
    {
        await teamInfoService.RequestScoresFromServerAsync();
    }
}
