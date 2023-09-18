using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
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
}
