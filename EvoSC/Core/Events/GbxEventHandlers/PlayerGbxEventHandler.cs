using EvoSC.Interfaces;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;

namespace EvoSC.Core.Events.GbxEventHandlers;

public class PlayerGbxEventHandler : IGbxEventHandler
{
    private readonly IPlayerService _playerService;

    public PlayerGbxEventHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public void HandleEvents(GbxRemoteClient client)
    {
        client.OnPlayerConnect += _playerService.ClientOnPlayerConnect;
        client.OnPlayerDisconnect += _playerService.ClientOnPlayerDisconnect;
    }
}
