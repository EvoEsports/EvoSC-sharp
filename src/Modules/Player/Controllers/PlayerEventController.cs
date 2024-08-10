using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Arguments;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Modules.Official.Player.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.Player.Controllers;

[Controller]
public class PlayerEventController(IPlayerService playerService) : EvoScController<IEventControllerContext>
{
    [Subscribe(PlayerEvents.PlayerJoined)]
    public Task OnPlayerJoined(object sender, PlayerJoinedEventArgs args)
    {
        if (args.IsNewPlayer)
        {
            return playerService.SetupPlayerAsync(args.Player, args.IsPlayerListUpdate);
        }
        
        return playerService.GreetPlayerAsync(args.Player);
    }
}
