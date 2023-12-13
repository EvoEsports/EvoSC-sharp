using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Models;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.OpenPlanetModule.Controllers;

[Controller]
public class OpenPlanetEventController(IServerClient server, IOpenPlanetTrackerService trackerService,
        IPlayerManagerService players)
    : EvoScController<IEventControllerContext>
{
    [Subscribe(OpenPlanetEvents.PlayerDueForKick)]
    public Task OnPlayerDueForKickAsync(object sender, PlayerDueForKickEventArgs args) =>
        server.Remote.KickAsync(args.Player.GetLogin());

    [Subscribe(GbxRemoteEvent.PlayerDisconnect)]
    public async Task OnPlayerDisconnectAsync(object sender, PlayerDisconnectGbxEventArgs args)
    {
        var player = await players.GetPlayerAsync(PlayerUtils.ConvertLoginToAccountId(args.Login));
        trackerService.RemovePlayer(player);
    }
}
