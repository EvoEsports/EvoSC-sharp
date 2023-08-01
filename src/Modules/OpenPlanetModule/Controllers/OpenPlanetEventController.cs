using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.OpenPlanetModule.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Controllers;

[Controller]
public class OpenPlanetEventController : EvoScController<IEventControllerContext>
{
    private readonly IServerClient _server;

    public OpenPlanetEventController(IServerClient server) => _server = server;

    [Subscribe(OpenPlanetEvents.PlayerDueForKick)]
    public Task OnPlayerDueForKickAsync(object sender, PlayerDueForKickEventArgs args) =>
        _server.Remote.KickAsync(args.Player.GetLogin());
}
