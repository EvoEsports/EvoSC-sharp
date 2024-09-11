using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.GameModeUiModule.Config;
using EvoSC.Modules.Official.GameModeUiModule.Enums;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EvoSC.Modules.Official.GameModeUiModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class GameModeUiModuleService(IServerClient server, IGameModeUiModuleSettings settings, ILogger<GameModeUiModuleService> logger)
    : IGameModeUiModuleService
{
    private readonly List<GameModeUiComponentSettings> _componentSettings = [];

    public async Task InitializeAsync()
    {
        _componentSettings.AddRange(GetDefaultSettings());
        await ApplyConfigurationAsync(_componentSettings);
    }

    public async Task ApplyConfigurationAsync(List<GameModeUiComponentSettings> componentSettingsList)
    {
        var uiModuleProperties = GetUiModulesPropertiesJson(componentSettingsList);
        logger.LogDebug("Applying UI properties: {json}", uiModuleProperties);
        await server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", uiModuleProperties);
    }

    public async Task ApplyComponentSettingsAsync(GameModeUiComponentSettings componentSettings)
    {
        await ApplyConfigurationAsync([componentSettings]);
    }

    public async Task ApplyAndSaveComponentSettingsAsync(GameModeUiComponentSettings componentSettings)
    {
        //TODO: overwrite value in _componentSettings
        await ApplyConfigurationAsync([componentSettings]);
    }

    public List<GameModeUiComponentSettings> GetDefaultSettings()
    {
        return
        [
            new GameModeUiComponentSettings(
                GameModeUiComponents.Chrono,
                settings.ChronoVisible,
                settings.ChronoX,
                settings.ChronoY,
                settings.ChronoScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.RespawnHelper,
                settings.RespawnHelperVisible,
                settings.RespawnHelperX,
                settings.RespawnHelperY,
                settings.RespawnHelperScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.Checkpoint,
                settings.CheckpointVisible,
                settings.CheckpointX,
                settings.CheckpointY,
                settings.CheckpointScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.LapsCounter,
                settings.LapsCounterVisible,
                settings.LapsCounterX,
                settings.LapsCounterY,
                settings.LapsCounterScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.TimeGap,
                settings.TimeGapVisible,
                settings.TimeGapX,
                settings.TimeGapY,
                settings.TimeGapScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.ScoresTable,
                settings.ScoresTableVisible,
                settings.ScoresTableX,
                settings.ScoresTableY,
                settings.ScoresTableScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.DisplayMessage,
                settings.DisplayMessageVisible,
                settings.DisplayMessageX,
                settings.DisplayMessageY,
                settings.DisplayMessageScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.Countdown,
                settings.CountdownVisible,
                settings.CountdownX,
                settings.CountdownY,
                settings.CountdownScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.SpectatorBaseName,
                settings.SpectatorBaseNameVisible,
                settings.SpectatorBaseNameX,
                settings.SpectatorBaseNameY,
                settings.SpectatorBaseNameScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.SpectatorBaseCommands,
                settings.SpectatorBaseCommandsVisible,
                settings.SpectatorBaseCommandsX,
                settings.SpectatorBaseCommandsY,
                settings.SpectatorBaseCommandsScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.Record,
                settings.RecordVisible,
                settings.RecordX,
                settings.RecordY,
                settings.RecordScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.BigMessage,
                settings.BigMessageVisible,
                settings.BigMessageX,
                settings.BigMessageY,
                settings.BigMessageScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.BlockHelper,
                settings.BlockHelperVisible,
                settings.BlockHelperX,
                settings.BlockHelperY,
                settings.BlockHelperScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.WarmUp,
                settings.WarmUpVisible,
                settings.WarmUpX,
                settings.WarmUpY,
                settings.WarmUpScale
            ),
            new GameModeUiComponentSettings(
                GameModeUiComponents.BestRaceViewer,
                settings.BestRaceViewerVisible,
                settings.BestRaceViewerX,
                settings.BestRaceViewerY,
                settings.BestRaceViewerScale
            )
        ];
    }

    public string GetUiModulesPropertiesJson(List<GameModeUiComponentSettings> componentSettingsList)
    {
        var propertyObjects = componentSettingsList
            .Select(uiElement => GeneratePropertyObject(uiElement))
            .ToList();

        return JsonConvert.SerializeObject(new { uimodules = propertyObjects });
    }

    public dynamic GeneratePropertyObject(GameModeUiComponentSettings componentSettings)
    {
        return new
        {
            id = componentSettings.Name,
            position = (double[]) [componentSettings.X, componentSettings.Y],
            visible = componentSettings.Visible,
            scale = componentSettings.Scale,
            position_update = componentSettings.UpdatePosition,
            visible_update = componentSettings.UpdateVisible,
            scale_update = componentSettings.UpdateScale,
        };
    }
}
