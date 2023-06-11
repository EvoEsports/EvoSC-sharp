using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.OpenPlanetControl.Interfaces;
using EvoSC.Modules.Official.OpenPlanetControl.Models;

namespace EvoSC.Modules.Official.OpenPlanetControl.Controllers;

[Controller]
[ManialinkRoute(Route = "OpenPlanetControl")]
public class OpenPlanetManialinkController : Manialinks.ManialinkController
{
    private readonly IOpenPlanetControlService _service;
    
    public OpenPlanetManialinkController(IOpenPlanetControlService service)
    {
        _service = service;
    }

    [ManialinkRoute(Route = "Detect")]
    public async Task ActionDetect(DetectorEntry entry)
    {
        
        await _service.OnDetectAsync(Context.Player.GetLogin(), entry.Data);
    }
    
    [ManialinkRoute(Route = "Kick")]
    public async Task ActionKick()
    {
        await _service.KickAsync(Context.Player.GetLogin());
    }
}
