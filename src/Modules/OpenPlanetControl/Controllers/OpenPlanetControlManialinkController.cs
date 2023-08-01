using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.OpenPlanetControl.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetControl.Models;

namespace EvoSC.Modules.Official.OpenPlanetControl.Controllers;

[Controller]
[ManialinkRoute(Route = "OpenPlanetActions")]
public class OpenPlanetControlManialinkController : ManialinkController
{
    public async Task CheckAsync(IOpenPlanetInfo openPlanetInfo)
    {
        
    }
}
