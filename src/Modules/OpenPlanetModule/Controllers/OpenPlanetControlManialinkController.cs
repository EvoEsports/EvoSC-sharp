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
    private readonly IOpenPlanetTrackerService _trackerService;

    public OpenPlanetControlManialinkController(IOpenPlanetControlService opControl, IServerClient server,
        IOpenPlanetTrackerService trackerService)
    {
        _opControl = opControl;
        _server = server;
        _trackerService = trackerService;
    }

    public async Task CheckAsync(IOpenPlanetInfo openPlanetInfo)
    {
        await _opControl.VerifySignatureModeAsync(Context.Player, openPlanetInfo);
        _trackerService.AddOrUpdatePlayer(Context.Player, openPlanetInfo);
    }

    public Task DisconnectAsync() => _server.Remote.KickAsync(Context.Player.GetLogin());
}
