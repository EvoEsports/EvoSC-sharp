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

    public Task<string> GetUiModulesPropertiesJsonAsync()
    {
        var uiModulesProperties = JsonConvert.SerializeObject(new
        {
            uimodules = new List<dynamic>
            {
                GeneratePropertyObject("Race_Chrono", settings.ChronoVisible, settings.ChronoX, settings.ChronoY, settings.ChronoScale),
                GeneratePropertyObject("Race_RespawnHelper", settings.RespawnHelperVisible, settings.RespawnHelperX, settings.RespawnHelperY, settings.RespawnHelperScale),
                GeneratePropertyObject("Race_Checkpoint", settings.CheckpointVisible, settings.CheckpointX, settings.CheckpointY, settings.CheckpointScale),
                GeneratePropertyObject("Race_LapsCounter", settings.LapsCounterVisible, settings.LapsCounterX, settings.LapsCounterY, settings.LapsCounterScale),
                GeneratePropertyObject("Race_TimeGap", settings.TimeGapVisible, settings.TimeGapX, settings.TimeGapY, settings.TimeGapScale),
                GeneratePropertyObject("Race_ScoresTable", settings.ScoresTableVisible, settings.ScoresTableX, settings.ScoresTableY, settings.ScoresTableScale),
                GeneratePropertyObject("Race_DisplayMessage", settings.DisplayMessageVisible, settings.DisplayMessageX, settings.DisplayMessageY, settings.DisplayMessageScale),
                GeneratePropertyObject("Race_Countdown", settings.CountdownVisible, settings.CountdownX, settings.CountdownY, settings.CountdownScale),
                GeneratePropertyObject("Race_SpectatorBase_Name", settings.SpectatorBaseNameVisible, settings.SpectatorBaseNameX, settings.SpectatorBaseNameY, settings.SpectatorBaseNameScale),
                GeneratePropertyObject("Race_SpectatorBase_Commands", settings.SpectatorBaseCommandsVisible, settings.SpectatorBaseCommandsX, settings.SpectatorBaseCommandsY, settings.SpectatorBaseCommandsScale),
                GeneratePropertyObject("Race_Record", settings.RecordVisible, settings.RecordX, settings.RecordY, settings.RecordScale),
                GeneratePropertyObject("Race_BigMessage", settings.BigMessageVisible, settings.BigMessageX, settings.BigMessageY, settings.BigMessageScale),
                GeneratePropertyObject("Race_BlockHelper", settings.BlockHelperVisible, settings.BlockHelperX, settings.BlockHelperY, settings.BlockHelperScale),
                GeneratePropertyObject("Race_WarmUp", settings.WarmUpVisible, settings.WarmUpX, settings.WarmUpY, settings.WarmUpScale),
                GeneratePropertyObject("Race_BestRaceViewer", settings.BestRaceViewerVisible, settings.BestRaceViewerX, settings.BestRaceViewerY, settings.BestRaceViewerScale),
            }
        });

        return Task.FromResult(uiModulesProperties);
    }

    public dynamic GeneratePropertyObject(string uiModuleName, bool visible, double x, double y, double scale)
    {
        return new
        {
            id = uiModuleName,
            position = (double[]) [x, y],
            visible,
            scale,
            position_update = true,
            visible_update = true,
            scale_update = true,
        };
    }
}
