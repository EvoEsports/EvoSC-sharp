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
public class MatchReadyEventController : EvoScController<IEventControllerContext>
{
    private readonly IPlayerReadyService _playerReadyService;
    private readonly IPlayerManagerService _players;

    public MatchReadyEventController(IPlayerReadyService playerReadyService, IPlayerManagerService players)
    {
        _playerReadyService = playerReadyService;
        _players = players;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args)
    {
        if (_playerReadyService.MatchIsStarted)
        {
            return;
        }
        
        var player = await _players.GetPlayerAsync(PlayerUtils.ConvertLoginToAccountId(args.Login));

        if (player == null)
        {
            return;
        }

        await _playerReadyService.SendWidgetAsync(player);
    }
}
