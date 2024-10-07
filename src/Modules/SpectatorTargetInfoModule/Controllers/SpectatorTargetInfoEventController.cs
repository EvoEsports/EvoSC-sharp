using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Controllers;

[Controller]
public class SpectatorTargetInfoEventController(ISpectatorTargetInfoService spectatorTargetInfoService)
    : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerDisconnect)]
    public Task OnPlayerDisconnectAsync(object sender, PlayerGbxEventArgs eventArgs) =>
        spectatorTargetInfoService.RemovePlayerAsync(eventArgs.Login);

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMapAsync(object sender, MapGbxEventArgs eventArgs)
    {
        await spectatorTargetInfoService.DetectIsTeamsModeAsync();
        await spectatorTargetInfoService.DetectIsTimeAttackModeAsync();
    }

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
        await spectatorTargetInfoService.FetchAndCacheTeamInfoAsync();
        await spectatorTargetInfoService.ResetWidgetForSpectatorsAsync();
    }

    [Subscribe(ModeScriptEvent.WarmUpStartRound)]
    public async Task OnNewWarmUpRoundAsync(object sender, WarmUpRoundEventArgs roundEventArgs)
    {
        await spectatorTargetInfoService.ClearCheckpointsAsync();
        await spectatorTargetInfoService.FetchAndCacheTeamInfoAsync();
        await spectatorTargetInfoService.ResetWidgetForSpectatorsAsync();
    }

    [Subscribe(ModeScriptEvent.WarmUpStart)]
    public Task OnWarmUpStartAsync(object sender, EventArgs args) =>
        spectatorTargetInfoService.UpdateIsTimeAttackModeAsync(true);

    [Subscribe(ModeScriptEvent.WarmUpEnd)]
    public Task OnWarmUpEndAsync(object sender, EventArgs args) =>
        spectatorTargetInfoService.DetectIsTimeAttackModeAsync();

    [Subscribe(ModeScriptEvent.GiveUp)]
    public Task OnPlayerGiveUpAsync(object sender, PlayerUpdateEventArgs args) =>
        spectatorTargetInfoService.ClearCheckpointsAsync(args.Login);
}
