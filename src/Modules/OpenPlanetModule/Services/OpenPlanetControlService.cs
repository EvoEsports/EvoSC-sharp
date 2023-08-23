using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Config;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.OpenPlanetModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class OpenPlanetControlService : IOpenPlanetControlService
{
    private readonly ILogger<OpenPlanetControlService> _logger;
    private readonly IPermissionManager _permissions;
    private readonly IOpenPlanetControlSettings _opcSettings;
    private readonly IManialinkManager _manialinks;
    private readonly IServerClient _server;
    private readonly IOpenPlanetScheduler _scheduler;
    private readonly dynamic _locale;

    public OpenPlanetControlService(ILogger<OpenPlanetControlService> logger, IPermissionManager permissions,
        IOpenPlanetControlSettings opcSettings, IManialinkManager manialinks, IServerClient server, IOpenPlanetScheduler scheduler,
        Locale locale)
    {
        _logger = logger;
        _permissions = permissions;
        _opcSettings = opcSettings;
        _manialinks = manialinks;
        _server = server;
        _scheduler = scheduler;
        _locale = locale;
    }

    public async Task VerifySignatureModeAsync(IPlayer player, IOpenPlanetInfo playerOpInfo)
    {
        _logger.LogDebug("Verifying OpenPlanet for Player {Player}", player.AccountId);

        if (!playerOpInfo.IsOpenPlanet)
        {
            await ReleasePlayerAsync(player);
            return;
        }
        
        if (playerOpInfo.Version < _opcSettings.MinimumRequiredVersion)
        {
            await JailPlayerAsync(player, OpJailReason.InvalidVersion);
            return;
        }

        var usingOp = playerOpInfo.IsOpenPlanet;
        var canBypass = await _permissions.HasPermissionAsync(player, OpenPlanetPermissions.CanBypassVerification);
        var correctSignature = _opcSettings.AllowedSignatureModes.HasFlag(playerOpInfo.SignatureMode);

        if (_opcSettings.AllowOpenplanet && (canBypass || correctSignature))
        {
            await ReleasePlayerAsync(player);
            return;
        }

        var jailReason = usingOp && !_opcSettings.AllowOpenplanet
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
        if (_scheduler.PlayerIsScheduledForKick(player))
        {
            return;
        }
        
        await _server.Remote.ForceSpectatorAsync(player.GetLogin(), 1);
        _scheduler.ScheduleKickPlayer(player);
        
        var allowedSignatures = Enum.GetValues<OpenPlanetSignatureMode>()
            .Where(f => _opcSettings.AllowedSignatureModes.HasFlag(f))
            .Select(Enum.GetName)
            .ToArray();
        
        await _manialinks.SendManialinkAsync(player, "OpenPlanetModule.WarningWindow", new
        {
            AllowedSignatures = allowedSignatures,
            Config = _opcSettings,
            Reason = reason,
            WhatToDo = GetWhatToDoByReason(reason),
            Locale = _locale
        });

        switch (reason)
        {
            case OpJailReason.InvalidVersion:
                await _server.ErrorMessageAsync(_locale.PlayerLanguage.ProhibitedVersion, player);
                break;
            case OpJailReason.InvalidSignatureMode:
                await _server.ErrorMessageAsync(_locale.PlayerLanguage.ProhibitedSignatureMode, player);
                break;
            case OpJailReason.OpenPlanetNotAllowed:
                await _server.ErrorMessageAsync(_locale.PlayerLanguage.OpenPlanetProhibited, player);
                break;
        }
    }
    
    private async Task ReleasePlayerAsync(IPlayer player)
    {
        if (!_scheduler.PlayerIsScheduledForKick(player))
        {
            return;
        }
        
        _scheduler.UnScheduleKickPlayer(player);
        await _manialinks.HideManialinkAsync("OpenPlanetModule.WarningWindow");
        await _server.Remote.ForceSpectatorAsync(player.GetLogin(), 0);
        await _server.SuccessMessageAsync(_locale.PlayerLanguage.CorrectSignatureMode, player);
    }
}
