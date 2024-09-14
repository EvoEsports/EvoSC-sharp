using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.SpectatorCamModeModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.SpectatorCamModeModule.Controllers;

[Controller]
public class SpectatorCamModeEventController(
    ISpectatorCamModeService spectatorCamModeService,
    IPlayerManagerService playerManagerService)
    : EvoScController<EventControllerContext>
{
    [Subscribe(ModeScriptEvent.StartMapEnd)]
    public Task OnStartMapEndAsync(object sender, MapEventArgs args) =>
        spectatorCamModeService.SendCamModeWidgetToSpectatorsAsync();

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public Task OnPodiumStartAsync(object sender, PodiumEventArgs args) =>
        spectatorCamModeService.HideCamModeWidgetAsync();

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerGbxEventArgs args)
    {
        var onlinePlayer = await playerManagerService.GetOnlinePlayerAsync(
            PlayerUtils.ConvertLoginToAccountId(args.Login)
        );

        if (onlinePlayer.State == PlayerState.Playing) return;

        await spectatorCamModeService.SendCamModeWidgetAsync(args.Login);
    }

    [Subscribe(ModeScriptEvent.GiveUp)]
    public Task OnGiveUpAsync(object sender, PlayerUpdateEventArgs eventArgs) =>
        spectatorCamModeService.SendCamModeWidgetAsync(eventArgs.Login);

    [Subscribe(ModeScriptEvent.EndRoundStart)]
    public Task OnEndRoundStartAsync(object sender, RoundEventArgs eventArgs) =>
        spectatorCamModeService.HideCamModeWidgetAsync();

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public Task OnStartRoundStartAsync(object sender, RoundEventArgs eventArgs) =>
        spectatorCamModeService.SendCamModeWidgetToSpectatorsAsync();

    [Subscribe(GbxRemoteEvent.PlayerInfoChanged)]
    public async Task OnPlayerInfoChangedAsync(object sender, PlayerInfoChangedGbxEventArgs eventArgs)
    {
        var onlinePlayer = await playerManagerService.GetOnlinePlayerAsync(
            PlayerUtils.ConvertLoginToAccountId(eventArgs.PlayerInfo.Login)
        );

        if (onlinePlayer.State == PlayerState.Playing)
        {
            await spectatorCamModeService.HideCamModeWidgetAsync(eventArgs.PlayerInfo.Login);
        }
        else
        {
            await spectatorCamModeService.SendCamModeWidgetAsync(eventArgs.PlayerInfo.Login);
        }
    }
}
