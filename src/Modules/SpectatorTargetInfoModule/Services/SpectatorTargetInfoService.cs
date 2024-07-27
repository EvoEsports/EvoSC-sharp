using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using SpectatorTargetInfo.Interfaces;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class SpectatorTargetInfoService
    (IManialinkManager manialinks, IServerClient server, IPlayerManagerService playerManagerService) : ISpectatorTargetInfoService
{
    private const string WidgetTemplate = "SpectatorTargetInfoModule.SpectatorTargetInfo";

    public async Task SendManiaLinkAsync() =>
        await manialinks.SendManialinkAsync(WidgetTemplate);


    public async Task SendManiaLinkAsync(string playerLogin)
    {
        var player = await playerManagerService.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerLogin));
        await manialinks.SendManialinkAsync(player, WidgetTemplate);
    }

    public async Task HideManiaLinkAsync() =>
        await manialinks.HideManialinkAsync(WidgetTemplate);

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

        return server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", hudSettings.ToArray());
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

        return server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", hudSettings.ToArray());
    }

    public Task ForwardCheckpointTimeToClientsAsync(WayPointEventArgs wayPointEventArgs)
    {
        return manialinks.SendManialinkAsync("SpectatorTargetInfoModule.NewCpTime",
            new
            {
                accountId = wayPointEventArgs.AccountId,
                time = wayPointEventArgs.RaceTime,
                cpIndex = wayPointEventArgs.CheckpointInRace
            });
    }

    public Task ResetCheckpointTimesAsync()
    {
        manialinks.HideManialinkAsync("SpectatorTargetInfoModule.NewCpTime");
        return manialinks.SendManialinkAsync("SpectatorTargetInfoModule.ResetCpTimes");
    }

    public Task ForwardDnfToClientsAsync(PlayerUpdateEventArgs playerUpdateEventArgs)
    {
        return manialinks.SendManialinkAsync("SpectatorTargetInfoModule.NewCpTime", new
        {
            accountId = playerUpdateEventArgs.AccountId,
            time = 0,
            cpIndex = -1
        });
    }
}
