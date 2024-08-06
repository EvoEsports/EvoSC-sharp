using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.UIModule.Config;
using EvoSC.Modules.Official.UIModule.Interfaces;
using Newtonsoft.Json;

namespace EvoSC.Modules.Official.UIModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class UiModuleService(IServerClient server, IUiModuleSettings settings) : IUiModuleService
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
                await GeneratePropertyObject("Race_Chrono", settings.ChronoVisible, settings.ChronoX, settings.ChronoY, settings.ChronoScale),
                await GeneratePropertyObject("Race_RespawnHelper", settings.RespawnHelperVisible, settings.RespawnHelperX, settings.RespawnHelperY, settings.RespawnHelperScale),
                await GeneratePropertyObject("Race_Checkpoint", settings.CheckpointVisible, settings.CheckpointX, settings.CheckpointY, settings.CheckpointScale),
                await GeneratePropertyObject("Race_LapsCounter", settings.LapsCounterVisible, settings.LapsCounterX, settings.LapsCounterY, settings.LapsCounterScale),
                await GeneratePropertyObject("Race_TimeGap", settings.TimeGapVisible, settings.TimeGapX, settings.TimeGapY, settings.TimeGapScale),
                await GeneratePropertyObject("Race_ScoresTable", settings.ScoresTableVisible, settings.ScoresTableX, settings.ScoresTableY, settings.ScoresTableScale),
                await GeneratePropertyObject("Race_DisplayMessage", settings.DisplayMessageVisible, settings.DisplayMessageX, settings.DisplayMessageY, settings.DisplayMessageScale),
                await GeneratePropertyObject("Race_Countdown", settings.CountdownVisible, settings.CountdownX, settings.CountdownY, settings.CountdownScale),
                await GeneratePropertyObject("Race_SpectatorBase_Name", settings.SpectatorBaseNameVisible, settings.SpectatorBaseNameX, settings.SpectatorBaseNameY, settings.SpectatorBaseNameScale),
                await GeneratePropertyObject("Race_SpectatorBase_Commands", settings.SpectatorBaseCommandsVisible, settings.SpectatorBaseCommandsX, settings.SpectatorBaseCommandsY, settings.SpectatorBaseCommandsScale),
                await GeneratePropertyObject("Race_Record", settings.RecordVisible, settings.RecordX, settings.RecordY, settings.RecordScale),
                await GeneratePropertyObject("Race_BigMessage", settings.BigMessageVisible, settings.BigMessageX, settings.BigMessageY, settings.BigMessageScale),
                await GeneratePropertyObject("Race_BlockHelper", settings.BlockHelperVisible, settings.BlockHelperX, settings.BlockHelperY, settings.BlockHelperScale),
                await GeneratePropertyObject("Race_WarmUp", settings.WarmUpVisible, settings.WarmUpX, settings.WarmUpY, settings.WarmUpScale),
                await GeneratePropertyObject("Race_BestRaceViewer", settings.BestRaceViewerVisible, settings.BestRaceViewerX, settings.BestRaceViewerY, settings.BestRaceViewerScale),
            }
        });
    }

    public Task<dynamic> GeneratePropertyObject(string uiModuleName, bool visible, double x, double y, double scale)
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
