using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
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

    public async Task KickAsync(IPlayer player, IPlayer actor)
    {
        if (await _server.Remote.KickAsync(player.GetLogin()))
        {
            await _server.SendChatMessage($"$284{player.NickName} was kicked.", actor);
        }
        else
        {
            await _server.SendChatMessage("$f13Failed to kick the player. Did they leave already?", actor);
        }
    }

    public async Task MuteAsync(IPlayer player, IPlayer actor)
    {
        if (await _server.Remote.IgnoreAsync(player.GetLogin()))
        {
            await _server.SendChatMessage("$f13You were muted by an admin.", player);
            await _server.SendChatMessage($"$284{player.NickName} was muted.", actor);
        }
        else
        {
            await _server.SendChatMessage("$f13Failed to mute the player. Did they leave already?");
        }
    }

    public async Task UnmuteAsync(IPlayer player, IPlayer actor)
    {
        if (await _server.Remote.UnIgnoreAsync(player.GetLogin()))
        {
            await _server.SendChatMessage("$284You got un-muted by an admin.", player);
            await _server.SendChatMessage($"$284{player.NickName} was muted.", actor);
        }
        else
        {
            await _server.SendChatMessage("$f13Failed to mute the player. Did they leave already?", actor);
        }
    }

    public async Task BanAsync(IPlayer player, IPlayer actor)
    {
        await _server.Remote.BanAsync(player.GetLogin());
        await _server.Remote.BlackListAsync(player.GetLogin());
        
        await _server.SendChatMessage($"$284{player.NickName} was banned.", actor);
    }
}
