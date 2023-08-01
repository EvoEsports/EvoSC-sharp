using EvoSC.Common.Interfaces;
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

    public OpenPlanetControlService(ILogger<OpenPlanetControlService> logger, IPermissionManager permissions,
        IOpenPlanetControlSettings opcSettings, IManialinkManager manialinks, IServerClient server, IOpenPlanetScheduler scheduler)
    {
        _logger = logger;
        _permissions = permissions;
        _opcSettings = opcSettings;
        _manialinks = manialinks;
        _server = server;
        _scheduler = scheduler;
    }

    public async Task VerifySignatureModeAsync(IPlayer player, IOpenPlanetInfo playerOpInfo)
    {
        _logger.LogDebug("Verifying OpenPlanet for Player {Player}", player.AccountId);

        if (playerOpInfo.Version < _opcSettings.MinimumRequiredVersion)
        {
            await JailPlayerAsync(player, playerOpInfo);
            return;
        }
        
        if (!playerOpInfo.IsOpenPlanet)
        {
            // player is not using openplanet
            await ReleasePlayerAsync(player);
            return;
        }
        
        if (await _permissions.HasPermissionAsync(player, OpenPlanetPermissions.CanBypassVerification))
        {
            await ReleasePlayerAsync(player);
            return;
        }

        if (_opcSettings.AllowedSignatureModes.HasFlag(playerOpInfo.SignatureMode))
        {
            // player has valid signature mode
            await ReleasePlayerAsync(player);
            return;
        }

        await JailPlayerAsync(player, playerOpInfo);
    }

    private async Task JailPlayerAsync(IPlayer player, IOpenPlanetInfo playerOpInfo)
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
            allowedSignatures,
            config = _opcSettings
        });

        await _server.ErrorMessageAsync("Prohibited OpenPlanet signature mode detected! Forced into spectator.", player);
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
        await _server.SuccessMessageAsync("Correct OpenPlanet signature mode selected! You can now play.", player);
    }
}
