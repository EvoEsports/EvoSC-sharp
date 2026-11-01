using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.Player.Config;
using EvoSC.Modules.Official.Player.Events;
using EvoSC.Modules.Official.Player.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Player.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class PlayerService(IPlayerManagerService playerManager, IServerClient server, ILogger<PlayerService> logger,
        IContextService context, Locale locale, IPlayerModuleSettings playerModuleSettings, IPermissionManager permissions)
    : IPlayerService
{
    private readonly dynamic _locale = locale;

    public async Task GreetPlayerAsync(IPlayer player)
    {
        await server.Chat.InfoMessageAsync(_locale.PlayerJoined(player.NickName));
        await playerManager.UpdateLastVisitAsync(player);
    }

    public async Task SetupPlayerAsync(IPlayer player)
    {
        // assign the player to the default group
        if (playerModuleSettings.AddToDefaultGroup)
        {
            await permissions.SetDisplayGroupAsync(player, playerModuleSettings.DefaultGroupId);
        }
    }


    public async Task KickAsync(IPlayer player, IPlayer actor)
    {
        if (await server.Remote.KickAsync(player.GetLogin(), ""))
        {
            context.Audit().Success()
                .WithEventName(AuditEvents.PlayerKicked)
                .HavingProperties(new {Player = player})
                .Comment(_locale.Audit_Kicked);
            
            await server.Chat.SuccessMessageAsync(_locale.PlayerLanguage.PlayerKicked(player.NickName), actor);
        }
        else
        {
            await server.Chat.ErrorMessageAsync(_locale.PlayerLanguage.PlayerKickingFailed, actor);
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
            
            await server.Chat.WarningMessageAsync(_locale.PlayerLanguage.YouWereMuted, player);
            await server.Chat.SuccessMessageAsync(_locale.PlayerLanguage.PlayerMuted(player.NickName), actor);
        }
        else
        {
            await server.Chat.ErrorMessageAsync(_locale.PlayerMutingFailed, actor);
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
            
            await server.Chat.InfoMessageAsync(_locale.PlayerLanguage.YouGotUnmuted, player);
            await server.Chat.SuccessMessageAsync(_locale.PlayerLanguage.PlayerUnmuted(player.NickName), actor);
        }
        else
        {
            await server.Chat.ErrorMessageAsync(_locale.PlayerLanguage.PlayerUnmutingFailed, actor);
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
        
        await server.Chat.SuccessMessageAsync(_locale.PlayerLanguage.PlayerBanned(player.NickName), actor);
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
                
                await server.Chat.SuccessMessageAsync(_locale.PlayerLanguage.PlayerUnbanned(login), actor);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to unban player {Login}", login);
            await server.Chat.ErrorMessageAsync(_locale.PlayerLanguage.PlayerUnbanningFailed(login), actor);
        }

        try
        {
            if (await server.Remote.UnBlackListAsync(login))
            {
                context.Audit().Success()
                    .WithEventName(AuditEvents.PlayerUnblacklisted)
                    .HavingProperties(new {PlayerLogin = login})
                    .Comment(_locale.Audit_Unblacklisted);
                
                await server.Chat.SuccessMessageAsync(_locale.PlayerLanguage.PlayerUnblacklisted(login), actor);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to un-blacklist player {Login}", login);
            await server.Chat.ErrorMessageAsync(_locale.PlayerLanguage.PlayerUnblacklistingFailed(login), actor);
        }
    }

    public Task ForceSpectatorAsync(IPlayer player) => server.Remote.ForceSpectatorAsync(player.GetLogin(), 3);
}
