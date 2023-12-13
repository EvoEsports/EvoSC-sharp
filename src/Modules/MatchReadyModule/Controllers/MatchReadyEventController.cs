using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.MatchReadyModule.Controllers;

[Controller]
public class MatchReadyEventController(IPlayerReadyService playerReadyService, IPlayerManagerService players)
    : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args)
    {
        if (playerReadyService.MatchIsStarted)
        {
            return;
        }
        
        var player = await players.GetPlayerAsync(PlayerUtils.ConvertLoginToAccountId(args.Login));

        if (player == null)
        {
            return;
        }

        await playerReadyService.SendWidgetAsync(player);
    }
}
