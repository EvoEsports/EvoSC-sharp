using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
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
    IGameModeUiModuleService gameModeUiModuleService,
    ISpectatorCamModeSettings settings
)
    : ISpectatorCamModeService
{
    private const string WidgetTemplate = "SpectatorCamModeModule.SpectatorMode";

    public Task SendCamModeWidgetAsync() =>
        manialinks.SendPersistentManialinkAsync(WidgetTemplate, new { settings });

    public Task HideCamModeWidgetAsync() =>
        manialinks.HideManialinkAsync(WidgetTemplate);

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
