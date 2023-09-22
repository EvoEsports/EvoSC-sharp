using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.SponsorsModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.SponsorsModule.Controllers;

[Controller]
public class SponsorsController : EvoScController<IEventControllerContext>
{
    private readonly ISponsorsService _sponsorsService;
    private readonly IPlayerManagerService _playerManager;

    public SponsorsController(ISponsorsService sponsorsService, IPlayerManagerService playerManagerService)
    {
        _sponsorsService = sponsorsService;
        _playerManager = playerManagerService;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerJoin(object sender, PlayerConnectGbxEventArgs playerConnectArgs)
    {
        var player =
            await _playerManager.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerConnectArgs.Login));
        if (player.State == PlayerState.Spectating)
        {
            await _sponsorsService.ShowWidget(playerConnectArgs.Login);
        }
    }

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMap(object sender, MapGbxEventArgs mapGbxEventArgs)
    {
        await _sponsorsService.ShowWidgetToAllSpectators();
    }

    [Subscribe(GbxRemoteEvent.PlayerInfoChanged)]
    public Task OnPlayerInfoChange(object sender, PlayerInfoChangedGbxEventArgs playerInfoChangedArgs) =>
        _sponsorsService.ShowOrHide(playerInfoChangedArgs);
}
