using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using SpectatorTargetInfo.Interfaces;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class SpectatorTargetInfoService : ISpectatorTargetInfoService
{
    private const string WidgetTemplate = "SpectatorTargetInfoModule.SpectatorTargetInfo";

    private readonly IManialinkManager _manialinks;
    private readonly IServerClient _server;
    private readonly IEvoScBaseConfig _config;
    private readonly IThemeManager _themes;

    public SpectatorTargetInfoService(IManialinkManager manialinks, IServerClient server, IEvoScBaseConfig config, IThemeManager themes)
    {
        _manialinks = manialinks;
        _server = server;
        _config = config;
        _themes = themes;
    }

    public async Task SendManiaLinkAsync() =>
        await _manialinks.SendManialinkAsync(WidgetTemplate, GetWidgetData());


    public async Task SendManiaLinkAsync(string playerLogin) =>
        await _manialinks.SendManialinkAsync(playerLogin, WidgetTemplate, GetWidgetData());

    public async Task HideManiaLinkAsync() =>
        await _manialinks.HideManialinkAsync(WidgetTemplate);

    public Task HideNadeoSpectatorInfoAsync()
    {
        var hudSettings = new List<string>
        {
            @"{""uimodules"": [
                {
                    ""id"": ""Race_SpectatorBase_Name"",
                    ""visible"": false,
                    ""visible_update"": true,
                },
                {
                    ""id"": ""Race_SpectatorBase_Commands"",
                    ""visible"": true,
                    ""visible_update"": true,
                }
            ]}"
        };

        return _server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", hudSettings.ToArray());
    }

    public Task ShowNadeoSpectatorInfoAsync()
    {
        var hudSettings = new List<string>
        {
            @"{""uimodules"": [
                {
                    ""id"": ""Race_SpectatorBase_Name"",
                    ""visible"": true,
                    ""visible_update"": true,
                },
                {
                    ""id"": ""Race_SpectatorBase_Commands"",
                    ""visible"": true,
                    ""visible_update"": true,
                }
            ]}"
        };

        return _server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", hudSettings.ToArray());
    }

    public Task ForwardCheckpointTimeToClientsAsync(WayPointEventArgs wayPointEventArgs)
    {
        return _manialinks.SendManialinkAsync("SpectatorTargetInfoModule.NewCpTime",
            new
            {
                accountId = wayPointEventArgs.AccountId,
                time = wayPointEventArgs.RaceTime,
                cpIndex = wayPointEventArgs.CheckpointInRace
            });
    }

    public Task ResetCheckpointTimesAsync()
    {
        _manialinks.HideManialinkAsync("SpectatorTargetInfoModule.NewCpTime");
        return _manialinks.SendManialinkAsync("SpectatorTargetInfoModule.ResetCpTimes");
    }

    public Task ForwardDnfToClientsAsync(PlayerUpdateEventArgs playerUpdateEventArgs)
    {
        return _manialinks.SendManialinkAsync("SpectatorTargetInfoModule.NewCpTime", new
        {
            accountId = playerUpdateEventArgs.AccountId,
            time = 0,
            cpIndex = -1
        });
    }
    
    private dynamic GetWidgetData()
    {
        return new
        {
            primaryColor = _themes.Theme.UI_PrimaryColor,
            backgroundColor = _themes.Theme.UI_BackgroundColor,
            headerColor = _themes.Theme.UI_HeaderBackgroundColor
        };
    }
}
