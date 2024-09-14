using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Controllers;

[Controller]
public class SpectatorTargetInfoEventController(
    ISpectatorTargetInfoService spectatorTargetInfoService,
    ILogger<SpectatorTargetInfoEventController> logger)
    : EvoScController<EventControllerContext>
{
    // [Subscribe(ModeScriptEvent.GiveUp)]
    // public Task OnPlayerGiveUpAsync(object sender, PlayerUpdateEventArgs playerUpdateEventArgs) =>
    //     spectatorTargetInfoService.ForwardDnfToClientsAsync(playerUpdateEventArgs);

    /**
     *
     */
    // [Subscribe(GbxRemoteEvent.PlayerConnect)]
    // public async Task OnPlayerConnectAsync(object x, PlayerConnectGbxEventArgs args)
    // {
    //     if (!args.IsSpectator)
    //     {
    //         return;
    //     }
    //
    //     //TODO: do nothing?
    //     // await spectatorTargetInfoService.SendManiaLinkAsync(args.Login);
    // }
    [Subscribe(GbxRemoteEvent.PlayerDisconnect)]
    public Task OnPlayerDisconnect(object sender, PlayerGbxEventArgs eventArgs) =>
        spectatorTargetInfoService.RemovePlayerFromSpectatorsListAsync(eventArgs.Login);

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMap(object sender, MapGbxEventArgs eventArgs)
    {
        await spectatorTargetInfoService.UpdateIsTeamsModeAsync();
    }

    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task OnWayPointAsync(object sender, WayPointEventArgs wayPointEventArgs) =>
        spectatorTargetInfoService.AddCheckpointAsync(
            wayPointEventArgs.Login,
            wayPointEventArgs.CheckpointInLap,
            wayPointEventArgs.LapTime
        );

    [Subscribe(ModeScriptEvent.EndRoundStart)]
    public Task OnEndRoundStartAsync(object sender, RoundEventArgs eventArgs) =>
        spectatorTargetInfoService.HideSpectatorInfoWidgetAsync();

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public async Task OnNewRoundAsync(object sender, RoundEventArgs roundEventArgs)
    {
        await spectatorTargetInfoService.ClearCheckpointsAsync();
        await spectatorTargetInfoService.UpdateTeamInfoAsync();
        await spectatorTargetInfoService.ResetWidgetForSpectatorsAsync();
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public async Task OnPodiumStartAsync()
    {
        await spectatorTargetInfoService.HideSpectatorInfoWidgetAsync();
    }

    [Subscribe(GbxRemoteEvent.PlayerInfoChanged)]
    public async Task OnPlayerInfoChangedAsync(object sender, PlayerInfoChangedGbxEventArgs eventArgs)
    {
        var spectatorInfo = spectatorTargetInfoService.ParseSpectatorStatus(eventArgs.PlayerInfo.SpectatorStatus);
        var spectatorLogin = eventArgs.PlayerInfo.Login;

        logger.LogInformation("Spectator status: {status}", spectatorInfo);

        if (spectatorInfo is { IsSpectator: true, TargetPlayerId: > 0 })
        {
            var targetLogin =
                await spectatorTargetInfoService.GetLoginOfDedicatedPlayerAsync(spectatorInfo.TargetPlayerId);
            if (targetLogin != null)
            {
                await spectatorTargetInfoService.SetSpectatorTargetLoginAsync(spectatorLogin, targetLogin);
                return;
            }
        }

        await spectatorTargetInfoService.RemovePlayerFromSpectatorsListAsync(spectatorLogin);
        await spectatorTargetInfoService.HideSpectatorInfoWidgetAsync(spectatorLogin);
    }

    [Subscribe(ModeScriptEvent.GiveUp)]
    public Task OnGiveUpAsync(object sender, PlayerUpdateEventArgs eventArgs) =>
        spectatorTargetInfoService.SendRequestTargetManialinkAsync(eventArgs.Login);
}
