using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Util;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Controllers;

[Controller]
[ManialinkRoute(Route = "OpenPlanetActions")]
public class OpenPlanetControlManialinkController : ManialinkController
{
    private readonly IOpenPlanetControlService _opControl;
    private readonly IServerClient _server;

    public OpenPlanetControlManialinkController(IOpenPlanetControlService opControl, IServerClient server)
    {
        _opControl = opControl;
        _server = server;
    }

    public Task CheckAsync(IOpenPlanetInfo openPlanetInfo) =>
        _opControl.VerifySignatureModeAsync(Context.Player, openPlanetInfo);

    public Task DisconnectAsync() => _server.Remote.KickAsync(Context.Player.GetLogin());
}
