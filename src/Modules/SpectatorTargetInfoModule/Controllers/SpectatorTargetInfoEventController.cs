using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using GbxRemoteNet.Events;
using SpectatorTargetInfo.Interfaces;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Controllers;

[Controller]
public class SpectatorTargetInfoEventController
    (ISpectatorTargetInfoService spectatorTargetInfoService) : EvoScController<EventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnectAsync(object x, PlayerConnectGbxEventArgs args) =>
        spectatorTargetInfoService.SendManiaLinkAsync(args.Login);

    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task OnWayPointAsync(object sender, WayPointEventArgs wayPointEventArgs) =>
        spectatorTargetInfoService.ForwardCheckpointTimeToClientsAsync(wayPointEventArgs);

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public Task OnNewRoundAsync(object sender, RoundEventArgs roundEventArgs) =>
        spectatorTargetInfoService.ResetCheckpointTimesAsync();

    [Subscribe(ModeScriptEvent.GiveUp)]
    public Task OnPlayerGiveUpAsync(object sender, PlayerUpdateEventArgs playerUpdateEventArgs) =>
        spectatorTargetInfoService.ForwardDnfToClientsAsync(playerUpdateEventArgs);
}
