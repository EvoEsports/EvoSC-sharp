using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Modules.Official.Player.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.Player.Controllers;

[Controller]
public class PlayerEventController(IPlayerService playerService) : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnect(object sender, PlayerConnectGbxEventArgs args) =>
        playerService.UpdateAndGreetPlayerAsync(args.Login);
}
