using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Remote;
using EvoSC.Modules.Official.OpenPlanetControl.Interfaces;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.OpenPlanetControl.Controllers;

[Controller]
public class OpenPlanetControlController : EvoScController<EventControllerContext>
{
    private readonly ILogger<OpenPlanetControlController> _logger;
    private readonly IOpenPlanetControlService _service;
    private readonly IServerClient _server;
    
    public OpenPlanetControlController(
        ILogger<OpenPlanetControlController> logger,
        IOpenPlanetControlService service,
        IServerClient server
    )
    {
        _logger = logger;
        _service = service;
        _server = server;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(PlayerConnectGbxEventArgs args)
    {
        if (args.IsSpectator) return;
      
        
    }
}
