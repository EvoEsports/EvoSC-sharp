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
public class SpectatorTargetInfoEventController : EvoScController<EventControllerContext>
{
    private readonly ISpectatorTargetInfoService _spectatorTargetInfoService;

    public SpectatorTargetInfoEventController(ISpectatorTargetInfoService spectatorTargetInfoService) =>
        _spectatorTargetInfoService = spectatorTargetInfoService;

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnectAsync(object x, PlayerConnectGbxEventArgs args) =>
        _spectatorTargetInfoService.SendManiaLinkAsync(args.Login);

    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task OnWayPointAsync(object sender, WayPointEventArgs wayPointEventArgs) =>
        _spectatorTargetInfoService.ForwardCheckpointTimeToClientsAsync(wayPointEventArgs);

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public Task OnNewRoundAsync(object sender, RoundEventArgs roundEventArgs) =>
        _spectatorTargetInfoService.ResetCheckpointTimesAsync();

    [Subscribe(ModeScriptEvent.GiveUp)]
    public Task OnPlayerGiveUpAsync(object sender, PlayerUpdateEventArgs playerUpdateEventArgs) =>
        _spectatorTargetInfoService.ForwardDnfToClientsAsync(playerUpdateEventArgs);
}
