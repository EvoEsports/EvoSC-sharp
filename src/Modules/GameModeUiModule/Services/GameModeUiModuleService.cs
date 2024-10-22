using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.GameModeUiModule.Config;
using EvoSC.Modules.Official.GameModeUiModule.Enums;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Models;
using Newtonsoft.Json;

namespace EvoSC.Modules.Official.GameModeUiModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class GameModeUiModuleService(IServerClient server, IGameModeUiModuleSettings settings)
    : IGameModeUiModuleService
{
    public Task ApplyComponentSettingsAsync(IEnumerable<GameModeUiComponentSettings> componentSettingsList) =>
        server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties",
            GetUiModulesPropertiesJson(componentSettingsList));

    public Task ApplyComponentSettingsAsync(GameModeUiComponentSettings componentSettings) =>
        ApplyComponentSettingsAsync([componentSettings]);

    public Task ApplyComponentSettingsAsync(string name, bool visible, double x, double y, double scale) =>
        ApplyComponentSettingsAsync([new GameModeUiComponentSettings(name, visible, x, y, scale)]);

    public string GetUiModulesPropertiesJson(IEnumerable<GameModeUiComponentSettings> componentSettingsList)
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
                GameModeUiComponents.SmallScoresTable,
                settings.SmallScoresTableVisible,
                settings.SmallScoresTableX,
                settings.SmallScoresTableY,
                settings.SmallScoresTableScale
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
}
