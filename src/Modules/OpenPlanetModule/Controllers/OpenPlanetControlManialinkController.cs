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
public class OpenPlanetControlManialinkController(IOpenPlanetControlService opControl, IServerClient server,
        IOpenPlanetTrackerService trackerService)
    : ManialinkController
{
    public async Task CheckAsync(IOpenPlanetInfo openPlanetInfo)
    {
        await opControl.VerifySignatureModeAsync(Context.Player, openPlanetInfo);
        trackerService.AddOrUpdatePlayer(Context.Player, openPlanetInfo);
    }

    public Task DisconnectAsync() => server.Remote.KickAsync(Context.Player.GetLogin());
}
