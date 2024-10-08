﻿using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Config;
using EvoSC.Modules.Official.OpenPlanetModule.Events;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.OpenPlanetModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class OpenPlanetControlService(ILogger<OpenPlanetControlService> logger, IPermissionManager permissions,
        IOpenPlanetControlSettings opcSettings, IManialinkManager manialinks, IServerClient server,
        IOpenPlanetScheduler scheduler,
        Locale locale, IAuditService auditService)
    : IOpenPlanetControlService
{
    private readonly dynamic _locale = locale;

    public async Task VerifySignatureModeAsync(IPlayer player, IOpenPlanetInfo playerOpInfo)
    {
        logger.LogDebug("Verifying OpenPlanet for Player {Player}", player.AccountId);

        if (opcSettings.AuditAllChecks)
        {
            await auditService.NewEvent(OpAuditEvents.SignatureModeCheck)
                .HavingProperties(new { Player = player, OpenPlanetData = playerOpInfo })
                .Comment("OpenPlanet state checked for player.")
                .Info()
                .LogAsync();
        }
        
        if (!playerOpInfo.IsOpenPlanet)
        {
            await ReleasePlayerAsync(player);
            return;
        }
        
        if (playerOpInfo.Version < opcSettings.MinimumRequiredVersion)
        {
            await JailPlayerAsync(player, OpJailReason.InvalidVersion);
            return;
        }

        var usingOp = playerOpInfo.IsOpenPlanet;
        var canBypass = await permissions.HasPermissionAsync(player, OpenPlanetPermissions.CanBypassVerification);
        var correctSignature = opcSettings.AllowedSignatureModes.HasFlag(playerOpInfo.SignatureMode);

        if (opcSettings.AllowOpenplanet && (canBypass || correctSignature))
        {
            await ReleasePlayerAsync(player);
            return;
        }

        var jailReason = usingOp && !opcSettings.AllowOpenplanet
            ? OpJailReason.OpenPlanetNotAllowed
            : OpJailReason.InvalidSignatureMode;
        
        await JailPlayerAsync(player, jailReason);
    }

    private (string Explanation, string Question) GetWhatToDoByReason(OpJailReason reason) => reason switch
    {
        OpJailReason.InvalidVersion => (
            _locale.PlayerLanguage.Explanations_UpdateOpenPlanet,
            _locale.PlayerLanguage.HowToQuestions_UpdateOpenPlanet
        ),
        OpJailReason.InvalidSignatureMode => (
            _locale.PlayerLanguage.Explanations_SwitchSignatureMode,
            _locale.PlayerLanguage.HowToQuestions_SwitchSignatureMode
        ),
        OpJailReason.OpenPlanetNotAllowed => (
            _locale.PlayerLanguage.Explanations_DisableOpenPlanet,
            _locale.PlayerLanguage.HowToQuestions_DisableOpenPlanet
        )
    };
    
    private async Task JailPlayerAsync(IPlayer player, OpJailReason reason)
    {
        if (opcSettings.AuditJails)
        {
            await auditService.NewEvent(OpAuditEvents.PlayerJailed)
                .HavingProperties(new { Player = player, Reason = reason })
                .Success()
                .LogAsync("Player jailed.");
        }
        
        if (scheduler.PlayerIsScheduledForKick(player))
        {
            return;
        }
        
        await server.Remote.ForceSpectatorAsync(player.GetLogin(), 1);
        scheduler.ScheduleKickPlayer(player);
        
        var allowedSignatures = Enum.GetValues<OpenPlanetSignatureMode>()
            .Where(f => opcSettings.AllowedSignatureModes.HasFlag(f))
            .Select(Enum.GetName)
            .ToArray();
        
        await manialinks.SendManialinkAsync(player, "OpenPlanetModule.WarningWindow", new
        {
            AllowedSignatures = allowedSignatures,
            Config = opcSettings,
            Reason = reason,
            WhatToDo = GetWhatToDoByReason(reason),
            Locale = _locale
        });

        switch (reason)
        {
            case OpJailReason.InvalidVersion:
                await server.Chat.ErrorMessageAsync(_locale.PlayerLanguage.ProhibitedVersion, player);
                break;
            case OpJailReason.InvalidSignatureMode:
                await server.Chat.ErrorMessageAsync(_locale.PlayerLanguage.ProhibitedSignatureMode, player);
                break;
            case OpJailReason.OpenPlanetNotAllowed:
                await server.Chat.ErrorMessageAsync(_locale.PlayerLanguage.OpenPlanetProhibited, player);
                break;
        }
    }
    
    private async Task ReleasePlayerAsync(IPlayer player)
    {
        if (!scheduler.PlayerIsScheduledForKick(player))
        {
            return;
        }
        
        scheduler.UnScheduleKickPlayer(player);
        await manialinks.HideManialinkAsync("OpenPlanetModule.WarningWindow");
        await server.Remote.ForceSpectatorAsync(player.GetLogin(), 0);
        await server.Chat.SuccessMessageAsync(_locale.PlayerLanguage.CorrectSignatureMode, player);
    }
}
