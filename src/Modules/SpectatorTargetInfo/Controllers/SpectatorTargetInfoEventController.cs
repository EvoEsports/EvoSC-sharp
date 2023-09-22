using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using GbxRemoteNet.Events;
using SpectatorTargetInfo.Interfaces;

namespace SpectatorTargetInfo.Controllers;

[Controller]
public class SpectatorTargetInfoEventController : EvoScController<EventControllerContext>
{
    private readonly ISpectatorTargetInfoService _spectatorTargetInfoService;

    public SpectatorTargetInfoEventController(ISpectatorTargetInfoService spectatorTargetInfoService)
    {
        _spectatorTargetInfoService = spectatorTargetInfoService;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnect(object x, PlayerConnectGbxEventArgs args) =>
        _spectatorTargetInfoService.SendManiaLink(args.Login);

    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task OnWayPoint(object sender, WayPointEventArgs wayPointEventArgs) =>
        _spectatorTargetInfoService.ForwardCheckpointTimeToClients(wayPointEventArgs);

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public Task OnNewRound(object sender, RoundEventArgs roundEventArgs) =>
        _spectatorTargetInfoService.ResetCheckpointTimes();

    [Subscribe(ModeScriptEvent.GiveUp)]
    public Task OnPlayerGiveUp(object sender, PlayerUpdateEventArgs playerUpdateEventArgs) =>
        _spectatorTargetInfoService.ForwardDnf(playerUpdateEventArgs);
}
