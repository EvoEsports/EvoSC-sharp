using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.Player.Interfaces;

namespace EvoSC.Modules.Official.Player.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class PlayerService : IPlayerService
{
    private readonly IPlayerManagerService _playerManager;
    private readonly IServerClient _server;
    
    public PlayerService(IPlayerManagerService playerManager, IServerClient server)
    {
        _playerManager = playerManager;
        _server = server;
    }

    public async Task UpdateAndGreetPlayer(string login)
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId(login);
        var player = await _playerManager.GetPlayerAsync(accountId);

        if (player == null)
        {
            player = await _playerManager.CreatePlayerAsync(accountId);
            await _server.SendChatMessage($"$<{player.NickName}$> joined for the first time!");
        }
        else
        {
            await _playerManager.UpdateLastVisitAsync(player);
            await _server.SendChatMessage($"$<{player.NickName}$> joined!");
        }
    }
}
