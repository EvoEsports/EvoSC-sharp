using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.GameModeUiModule.Config;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;
using Newtonsoft.Json;

namespace EvoSC.Modules.Official.GameModeUiModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class GameModeUiModuleService(IServerClient server, IGameModeUiModuleSettings settings)
    : IGameModeUiModuleService
{
    public async Task ApplyConfigurationAsync()
    {
        var uiModuleProperties = await GetUiModulesPropertiesJsonAsync();
        await server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", uiModuleProperties);
    }

    public async Task<string> GetUiModulesPropertiesJsonAsync()
    {
        return JsonConvert.SerializeObject(new
        {
            uimodules = new List<dynamic>
            {
                await GeneratePropertyObjectAsync(
                    "Race_Chrono",
                    settings.ChronoVisible,
                    settings.ChronoX,
                    settings.ChronoY,
                    settings.ChronoScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_RespawnHelper",
                    settings.RespawnHelperVisible,
                    settings.RespawnHelperX,
                    settings.RespawnHelperY,
                    settings.RespawnHelperScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_Checkpoint",
                    settings.CheckpointVisible,
                    settings.CheckpointX,
                    settings.CheckpointY,
                    settings.CheckpointScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_LapsCounter",
                    settings.LapsCounterVisible,
                    settings.LapsCounterX,
                    settings.LapsCounterY,
                    settings.LapsCounterScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_TimeGap",
                    settings.TimeGapVisible,
                    settings.TimeGapX,
                    settings.TimeGapY,
                    settings.TimeGapScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_ScoresTable",
                    settings.ScoresTableVisible,
                    settings.ScoresTableX,
                    settings.ScoresTableY,
                    settings.ScoresTableScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_DisplayMessage",
                    settings.DisplayMessageVisible,
                    settings.DisplayMessageX,
                    settings.DisplayMessageY,
                    settings.DisplayMessageScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_Countdown",
                    settings.CountdownVisible,
                    settings.CountdownX,
                    settings.CountdownY,
                    settings.CountdownScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_SpectatorBase_Name",
                    settings.SpectatorBaseNameVisible,
                    settings.SpectatorBaseNameX,
                    settings.SpectatorBaseNameY,
                    settings.SpectatorBaseNameScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_SpectatorBase_Commands",
                    settings.SpectatorBaseCommandsVisible,
                    settings.SpectatorBaseCommandsX,
                    settings.SpectatorBaseCommandsY,
                    settings.SpectatorBaseCommandsScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_Record",
                    settings.RecordVisible,
                    settings.RecordX,
                    settings.RecordY,
                    settings.RecordScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_BigMessage",
                    settings.BigMessageVisible,
                    settings.BigMessageX,
                    settings.BigMessageY,
                    settings.BigMessageScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_BlockHelper",
                    settings.BlockHelperVisible,
                    settings.BlockHelperX,
                    settings.BlockHelperY,
                    settings.BlockHelperScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_WarmUp",
                    settings.WarmUpVisible,
                    settings.WarmUpX,
                    settings.WarmUpY,
                    settings.WarmUpScale
                ),
                await GeneratePropertyObjectAsync(
                    "Race_BestRaceViewer",
                    settings.BestRaceViewerVisible,
                    settings.BestRaceViewerX,
                    settings.BestRaceViewerY,
                    settings.BestRaceViewerScale
                ),
            }
        });
    }

    public Task<dynamic> GeneratePropertyObjectAsync(string uiModuleName, bool visible, double x, double y,
        double scale)
    {
        return Task.FromResult<dynamic>(new
        {
            id = uiModuleName,
            position = (double[]) [x, y],
            visible,
            scale,
            position_update = true,
            visible_update = true,
            scale_update = true,
        });
    }
}
