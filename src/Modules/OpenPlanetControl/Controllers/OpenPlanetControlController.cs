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
    private readonly IOpenPlanetControlService _service;
    
    public OpenPlanetControlController(IOpenPlanetControlService service)
    {
        _service = service;
    }

    [Subscribe(GbxRemoteEvent.PlayerDisconnect)]
    public Task OnPlayerConnectAsync(object sender, PlayerDisconnectGbxEventArgs args)
    {
        _service.RemovePlayerByLogin(args.Login);
        return Task.CompletedTask;
    }
}
