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
public class PlayerService(IPlayerManagerService playerManager, IServerClient server, ILogger<PlayerService> logger,
        IContextService context, Locale locale)
    : IPlayerService
{
    private readonly dynamic _locale = locale;

    public async Task UpdateAndGreetPlayerAsync(string login)
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId(login);
        var player = await playerManager.GetPlayerAsync(accountId);

        if (player == null)
        {
            player = await playerManager.CreatePlayerAsync(accountId);
            await server.InfoMessageAsync(_locale.PlayerFirstJoined(player.NickName));
        }
        else
        {
            await server.InfoMessageAsync(_locale.PlayerJoined(player.NickName));
        }
        await playerManager.UpdateLastVisitAsync(player);
    }

    public async Task KickAsync(IPlayer player, IPlayer actor)
    {
        if (await server.Remote.KickAsync(player.GetLogin(), ""))
        {
            context.Audit().Success()
                .WithEventName(AuditEvents.PlayerKicked)
                .HavingProperties(new {Player = player})
                .Comment(_locale.Audit_Kicked);
            
            await server.SuccessMessageAsync(actor, _locale.PlayerLanguage.PlayerKicked(player.NickName));
        }
        else
        {
            await server.ErrorMessageAsync(actor, _locale.PlayerLanguage.PlayerKickingFailed);
        }
    }

    public async Task MuteAsync(IPlayer player, IPlayer actor)
    {
        if (await server.Remote.IgnoreAsync(player.GetLogin()))
        {
            context.Audit().Success()
                .WithEventName(AuditEvents.PlayerMuted)
                .HavingProperties(new {Player = player})
                .Comment(_locale.Audit_Muted);
            
            await server.WarningMessageAsync(player, _locale.PlayerLanguage.YouWereMuted);
            await server.SuccessMessageAsync(actor, _locale.PlayerLanguage.PlayerMuted(player.NickName));
        }
        else
        {
            await server.ErrorMessageAsync(actor, _locale.PlayerMutingFailed);
        }
    }

    public async Task UnmuteAsync(IPlayer player, IPlayer actor)
    {
        if (await server.Remote.UnIgnoreAsync(player.GetLogin()))
        {
            context.Audit().Success()
                .WithEventName(AuditEvents.PlayerUnmuted)
                .HavingProperties(new {Player = player})
                .Comment(_locale.Audit_Unmuted);
            
            await server.InfoMessageAsync(player, _locale.PlayerLanguage.YouGotUnmuted);
            await server.SuccessMessageAsync(actor, _locale.PlayerLanguage.PlayerUnmuted(player.NickName));
        }
        else
        {
            await server.ErrorMessageAsync(actor, _locale.PlayerLanguage.PlayerUnmutingFailed);
        }
    }

    public async Task BanAsync(IPlayer player, IPlayer actor)
    {
        try
        {
            await server.Remote.BanAsync(player.GetLogin());
        }
        catch (Exception ex)
        {
            // ignore this as we don't need to handle it, we'll blacklist the player anyways
            logger.LogTrace(ex, "Failed to ban player {AccountId}", player.AccountId);
        }
        
        await server.Remote.BlackListAsync(player.GetLogin());

        context.Audit().Success()
            .WithEventName(AuditEvents.PlayerBanned)
            .HavingProperties(new {Player = player})
            .Comment(_locale.Audit_Banned);
        
        await server.SuccessMessageAsync(actor, _locale.PlayerLanguage.PlayerBanned(player.NickName));
    }

    public async Task UnbanAsync(string login, IPlayer actor)
    {
        try
        {
            if (await server.Remote.UnBanAsync(login))
            {
                context.Audit().Success()
                    .WithEventName(AuditEvents.PlayerUnbanned)
                    .HavingProperties(new {PlayerLogin = login})
                    .Comment(_locale.Audit_Unbanned);
                
                await server.SuccessMessageAsync(actor, _locale.PlayerLanguage.PlayerUnbanned(login));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to unban player {Login}", login);
            await server.ErrorMessageAsync(actor, _locale.PlayerLanguage.PlayerUnbanningFailed(login));
        }

        try
        {
            if (await server.Remote.UnBlackListAsync(login))
            {
                context.Audit().Success()
                    .WithEventName(AuditEvents.PlayerUnblacklisted)
                    .HavingProperties(new {PlayerLogin = login})
                    .Comment(_locale.Audit_Unblacklisted);
                
                await server.SuccessMessageAsync(actor, _locale.PlayerLanguage.PlayerUnblacklisted(login));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to un-blacklist player {Login}", login);
            await server.ErrorMessageAsync(actor, _locale.PlayerLanguage.PlayerUnblacklistingFailed(login));
        }
    }
}
