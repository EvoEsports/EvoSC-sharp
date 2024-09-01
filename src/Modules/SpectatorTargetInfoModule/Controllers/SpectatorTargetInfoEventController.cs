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

    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task OnWayPointAsync(object sender, WayPointEventArgs wayPointEventArgs) =>
        spectatorTargetInfoService.AddCheckpointAsync(
            wayPointEventArgs.Login,
            wayPointEventArgs.CheckpointInLap,
            wayPointEventArgs.LapTime
        );

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public async Task OnNewRoundAsync(object sender, RoundEventArgs roundEventArgs)
    {
        await spectatorTargetInfoService.ClearCheckpointsAsync();
        await spectatorTargetInfoService.ResetWidgetForSpectatorsAsync();
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public Task OnPodiumStartAsync() =>
        spectatorTargetInfoService.HideWidgetAsync();

    [Subscribe(GbxRemoteEvent.PlayerInfoChanged)]
    public async Task OnPlayerInfoChangedAsync(object sender, PlayerInfoChangedGbxEventArgs eventArgs)
    {
        var spectatorInfo = spectatorTargetInfoService.ParseSpectatorStatus(eventArgs.PlayerInfo.SpectatorStatus);
        var spectatorLogin = eventArgs.PlayerInfo.Login;

        if (spectatorInfo.IsSpectator)
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
        await spectatorTargetInfoService.HideWidgetAsync(spectatorLogin);
    }
}
