using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Enums;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Models;
using EvoSC.Modules.Official.SpectatorCamModeModule.Config;
using EvoSC.Modules.Official.SpectatorCamModeModule.Interfaces;

namespace EvoSC.Modules.Official.SpectatorCamModeModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class SpectatorCamModeService(
    IManialinkManager manialinks,
    IPlayerManagerService playerManagerService,
    IGameModeUiModuleService gameModeUiModuleService,
    ISpectatorCamModeSettings settings
    )
    : ISpectatorCamModeService
{
    private const string WidgetTemplate = "SpectatorCamModeModule.SpectatorMode";

    public async Task InitializeAsync()
    {
        await SendCamModeWidgetToSpectatorsAsync();
        await HideGameModeUiAsync();
    }

    public async Task SendCamModeWidgetToSpectatorsAsync()
    {
        var players = (await playerManagerService.GetOnlinePlayersAsync())
            .Where(onlinePlayer => onlinePlayer.State == PlayerState.Spectating)
            .ToList();

        foreach (var player in players)
        {
            await SendCamModeWidgetAsync(player.GetLogin());
        }
    }

    public Task SendCamModeWidgetAsync(string playerLogin) =>
        manialinks.SendManialinkAsync(playerLogin, WidgetTemplate, new
        {
            settings
        });

    public Task HideCamModeWidgetAsync() =>
        manialinks.HideManialinkAsync(WidgetTemplate);

    public Task HideCamModeWidgetAsync(string playerLogin) =>
        manialinks.HideManialinkAsync(playerLogin, WidgetTemplate);

    public async Task HideGameModeUiAsync()
    {
        await gameModeUiModuleService.ApplyAndSaveComponentSettingsAsync(new GameModeUiComponentSettings(
            GameModeUiComponents.SpectatorBaseCommands,
            false,
            0.0,
            0.0,
            1.0
        ));
    }
}
