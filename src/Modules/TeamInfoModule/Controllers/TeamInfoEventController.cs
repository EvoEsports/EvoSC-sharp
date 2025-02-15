using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Events;
using EvoSC.Modules.Official.TeamSettingsModule.Events.EventArgs;
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

        if (teamInfoService.ShouldUpdateTeamPoints(eventArgs.Section))
        {
            await teamInfoService.UpdatePointsAsync(
                eventArgs.Teams.FirstOrDefault()?.MapPoints ?? 0,
                eventArgs.Teams.Skip(1).FirstOrDefault()?.MapPoints ?? 0,
                teamInfoService.ShouldExecuteManiaScript(eventArgs.Section)
            );
        }
    }

    [Subscribe(ModeScriptEvent.StartMatchStart)]
    public async Task OnMatchStartAsync(object sender, MatchEventArgs args)
    {
        if (!await teamInfoService.GetModeIsTeamsAsync())
        {
            return;
        }

        await teamInfoService.UpdatePointsAsync(0, 0, false);
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
    public async Task OnEndMapAsync(object sender, MapGbxEventArgs args)
    {
        if (!await teamInfoService.GetModeIsTeamsAsync())
        {
            return;
        }

        await teamInfoService.HideTeamInfoWidgetEveryoneAsync();
    }

    [Subscribe(TeamSettingsEvents.SettingsUpdated, IsAsync = true)]
    public async Task OnTeamSettingsUpdatedAsync(object sender, TeamSettingsEventArgs args)
    {
        if (!await teamInfoService.GetModeIsTeamsAsync())
        {
            return;
        }

        await Task.Delay(500);
        await teamInfoService.SendTeamInfoWidgetEveryoneAsync();
    }
}
