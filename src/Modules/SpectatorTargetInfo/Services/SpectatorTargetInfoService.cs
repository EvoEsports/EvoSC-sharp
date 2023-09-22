using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using SpectatorTargetInfo.Interfaces;

namespace SpectatorTargetInfo.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class SpectatorTargetInfoService : ISpectatorTargetInfoService
{
    private const string WidgetTemplate = "SpectatorTargetInfo.SpectatorTargetInfo";

    private readonly IManialinkManager _manialinks;
    private readonly IServerClient _server;
    private readonly IEvoScBaseConfig _config;

    public SpectatorTargetInfoService(IManialinkManager manialinks, IServerClient server, IEvoScBaseConfig config)
    {
        _manialinks = manialinks;
        _server = server;
        _config = config;
    }

    public async Task SendManiaLink() =>
        await _manialinks.SendManialinkAsync(WidgetTemplate, await GetWidgetData());


    public async Task SendManiaLink(string playerLogin) =>
        await _manialinks.SendManialinkAsync(playerLogin, WidgetTemplate, await GetWidgetData());

    public async Task HideManiaLink() =>
        await _manialinks.HideManialinkAsync(WidgetTemplate);

    private async Task<dynamic> GetWidgetData()
    {
        return new
        {
            primaryColor = _config.Theme.UI.PrimaryColor,
            backgroundColor = _config.Theme.UI.BackgroundColor,
            headerColor = _config.Theme.UI.HeaderBackgroundColor
        };
    }

    public Task HideNadeoSpectatorInfo()
    {
        var hudSettings = new List<string>()
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

    public Task ShowNadeoSpectatorInfo()
    {
        var hudSettings = new List<string>()
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

    public Task ForwardCheckpointTimeToClients(WayPointEventArgs wayPointEventArgs)
    {
        return _manialinks.SendManialinkAsync("SpectatorTargetInfo.NewCpTime",
            new
            {
                accountId = wayPointEventArgs.AccountId,
                time = wayPointEventArgs.RaceTime,
                cpIndex = wayPointEventArgs.CheckpointInRace
            });
    }

    public Task ResetCheckpointTimes()
    {
        _manialinks.HideManialinkAsync("SpectatorTargetInfo.NewCpTime");
        return _manialinks.SendManialinkAsync("SpectatorTargetInfo.ResetCpTimes");
    }

    public Task ForwardDnf(PlayerUpdateEventArgs playerUpdateEventArgs)
    {
        return _manialinks.SendManialinkAsync("SpectatorTargetInfo.NewCpTime", new
        {
            accountId = playerUpdateEventArgs.AccountId,
            time = 0,
            cpIndex = -1
        });
    }
}
