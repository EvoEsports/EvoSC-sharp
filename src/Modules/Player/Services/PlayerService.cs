using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
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
    private readonly dynamic _locale;

    public PlayerService(IPlayerManagerService playerManager, IServerClient server, ILogger<PlayerService> logger,
        IContextService context, Locale locale)
    {
        _playerManager = playerManager;
        _server = server;
        _logger = logger;
        _context = context;
        _locale = locale;
    }

    public async Task UpdateAndGreetPlayerAsync(string login)
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId(login);
        var player = await _playerManager.GetPlayerAsync(accountId);

        if (player == null)
        {
            player = await _playerManager.CreatePlayerAsync(accountId);
            await _server.InfoMessageAsync(_locale.PlayerFirstJoined(player.NickName));
        }
        else
        {
            await _server.InfoMessageAsync(_locale.PlayerJoined(player.NickName));
        }
        await _playerManager.UpdateLastVisitAsync(player);
    }

    public async Task KickAsync(IPlayer player, IPlayer actor)
    {
        if (await _server.Remote.KickAsync(player.GetLogin(), ""))
        {
            _context.Audit().Success()
                .WithEventName(AuditEvents.PlayerKicked)
                .HavingProperties(new {Player = player})
                .Comment(_locale.Audit_Kicked);
            
            await _server.SuccessMessageAsync(_locale.PlayerLanguage.PlayerKicked(player.NickName), actor);
        }
        else
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.PlayerKickingFailed, actor);
        }
    }

    public async Task MuteAsync(IPlayer player, IPlayer actor)
    {
        if (await _server.Remote.IgnoreAsync(player.GetLogin()))
        {
            _context.Audit().Success()
                .WithEventName(AuditEvents.PlayerMuted)
                .HavingProperties(new {Player = player})
                .Comment(_locale.Audit_Muted);
            
            await _server.WarningMessageAsync(_locale.PlayerLanguage.YouWereMuted, player);
            await _server.SuccessMessageAsync(_locale.PlayerLanguage.PlayerMuted(player.NickName), actor);
        }
        else
        {
            await _server.ErrorMessageAsync(_locale.PlayerMutingFailed);
        }
    }

    public async Task UnmuteAsync(IPlayer player, IPlayer actor)
    {
        if (await _server.Remote.UnIgnoreAsync(player.GetLogin()))
        {
            _context.Audit().Success()
                .WithEventName(AuditEvents.PlayerUnmuted)
                .HavingProperties(new {Player = player})
                .Comment(_locale.Audit_Unmuted);
            
            await _server.InfoMessageAsync(_locale.PlayerLanguage.YouGotUnmuted, player);
            await _server.SuccessMessageAsync(_locale.PlayerLanguage.PlayerUnmuted(player.NickName), actor);
        }
        else
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.PlayerUnmutingFailed, actor);
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
            .Comment(_locale.Audit_Banned);
        
        await _server.SuccessMessageAsync(_locale.PlayerLanguage.PlayerBanned(player.NickName), actor);
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
                    .Comment(_locale.Audit_Unbanned);
                
                await _server.SuccessMessageAsync(_locale.PlayerUnbanned(login));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to unban player {Login}", login);
            await _server.ErrorMessageAsync(_locale.PlayerUnbanningFailed(login));
        }

        try
        {
            if (await _server.Remote.UnBlackListAsync(login))
            {
                _context.Audit().Success()
                    .WithEventName(AuditEvents.PlayerUnblacklisted)
                    .HavingProperties(new {PlayerLogin = login})
                    .Comment(_locale.Audit_Unblacklisted);
                
                await _server.SuccessMessageAsync(_locale.PlayerUnblacklisted(login));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to un-blacklist player {Login}", login);
            await _server.ErrorMessageAsync(_locale.PlayerUnblacklistingFailed(login));
        }
    }
}
