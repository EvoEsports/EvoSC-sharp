using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Controllers;

[Controller]
public class SpectatorTargetInfoEventController(ISpectatorTargetInfoService spectatorTargetInfoService)
    : EvoScController<EventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerDisconnect)]
    public Task OnPlayerDisconnect(object sender, PlayerGbxEventArgs eventArgs) =>
        spectatorTargetInfoService.RemovePlayerFromSpectatorsListAsync(eventArgs.Login);

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public Task OnBeginMap(object sender, MapGbxEventArgs eventArgs) =>
        spectatorTargetInfoService.UpdateIsTeamsModeAsync();

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
        await spectatorTargetInfoService.UpdateTeamInfoAsync();
    }
}
