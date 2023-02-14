using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.Player.Events;
using EvoSC.Modules.Official.Player.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Player.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class PlayerService : IPlayerService
{
    private readonly IPlayerManagerService _playerManager;
    private readonly IServerClient _server;
    private readonly ILogger<PlayerService> _logger;
    private readonly IContextService _context;
    
    public PlayerService(IPlayerManagerService playerManager, IServerClient server, ILogger<PlayerService> logger, IContextService context)
    {
        _playerManager = playerManager;
        _server = server;
        _logger = logger;
        _context = context;
    }

    public async Task UpdateAndGreetPlayerAsync(string login)
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId(login);
        var player = await _playerManager.GetPlayerAsync(accountId);

        if (player == null)
        {
            player = await _playerManager.CreatePlayerAsync(accountId);
            await _server.InfoMessageAsync($"$<{player.NickName}$> joined for the first time!");
        }
        else
        {
            await _playerManager.UpdateLastVisitAsync(player);
            await _server.InfoMessageAsync($"$<{player.NickName}$> joined!");
        }
    }

    public async Task KickAsync(IPlayer player, IPlayer actor)
    {
        if (await _server.Remote.KickAsync(player.GetLogin(), ""))
        {
            _context.Audit().Success()
                .WithEventName(AuditEvents.PlayerKicked)
                .HavingProperties(new {Player = player})
                .Comment("Player kicked from the server.");
            
            await _server.SuccessMessageAsync($"$284{player.NickName} was kicked.", actor);
        }
        else
        {
            await _server.ErrorMessageAsync("$f13Failed to kick the player. Did they leave already?", actor);
        }
    }

    public async Task MuteAsync(IPlayer player, IPlayer actor)
    {
        if (await _server.Remote.IgnoreAsync(player.GetLogin()))
        {
            _context.Audit().Success()
                .WithEventName(AuditEvents.PlayerMuted)
                .HavingProperties(new {Player = player})
                .Comment("Player muted from the chat.");
            
            await _server.WarningMessageAsync("$f13You were muted by an admin.", player);
            await _server.SuccessMessageAsync($"$284{player.NickName} was muted.", actor);
        }
        else
        {
            await _server.ErrorMessageAsync("$f13Failed to mute the player. Did they leave already?");
        }
    }

    public async Task UnmuteAsync(IPlayer player, IPlayer actor)
    {
        if (await _server.Remote.UnIgnoreAsync(player.GetLogin()))
        {
            _context.Audit().Success()
                .WithEventName(AuditEvents.PlayerUnmuted)
                .HavingProperties(new {Player = player})
                .Comment("Player un-muted from the chat.");
            
            await _server.InfoMessageAsync("$284You got un-muted by an admin.", player);
            await _server.SuccessMessageAsync($"$284{player.NickName} was muted.", actor);
        }
        else
        {
            await _server.ErrorMessageAsync("$f13Failed to mute the player. Did they leave already?", actor);
        }
    }

    public async Task BanAsync(IPlayer player, IPlayer actor)
    {
        try
        {
            await _server.Remote.BanAsync(player.GetLogin());
        }
        catch (Exception ex)
        {
            // ignore this as we don't need to handle it, we'll blacklist the player anyways
            _logger.LogTrace(ex, "Failed to ban player {AccountId}", player.AccountId);
        }
        
        await _server.Remote.BlackListAsync(player.GetLogin());

        _context.Audit().Success()
            .WithEventName(AuditEvents.PlayerBanned)
            .HavingProperties(new {Player = player})
            .Comment("Player banned and added to the blacklist.");
        
        await _server.SuccessMessageAsync($"$284{player.NickName} was banned.", actor);
    }

    public async Task UnbanAsync(string login, IPlayer actor)
    {
        try
        {
            if (await _server.Remote.UnBanAsync(login))
            {
                _context.Audit().Success()
                    .WithEventName(AuditEvents.PlayerUnbanned)
                    .HavingProperties(new {PlayerLogin = login})
                    .Comment("Player was unbanned.");
                
                await _server.SuccessMessageAsync($"$284Player with login '{login}' was unbanned.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to unban player {Login}", login);
            await _server.ErrorMessageAsync($"$f13The login '{login}' was not found in the banlist.");
        }

        try
        {
            if (await _server.Remote.UnBlackListAsync(login))
            {
                _context.Audit().Success()
                    .WithEventName(AuditEvents.PlayerUnblacklisted)
                    .HavingProperties(new {PlayerLogin = login})
                    .Comment("Player removed from the blacklist.");
                
                await _server.SuccessMessageAsync($"$284Player with login '{login}' removed from the blacklist.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to un-blacklist player {Login}", login);
            await _server.ErrorMessageAsync($"$f13Player with login '{login}' was not found in the blacklist.");
        }
    }
}
