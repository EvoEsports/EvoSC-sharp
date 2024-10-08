using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Enums;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;
using EvoSC.Modules.Official.ScoreboardModule.Config;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;

namespace EvoSC.Modules.Official.ScoreboardModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ScoreboardService(
    IManialinkManager manialinks,
    IServerClient server,
    IScoreboardNicknamesService nicknamesService,
    IScoreboardSettings settings,
    IGameModeUiModuleService gameModeUiModuleService,
    IMatchSettingsService matchSettingsService
)
    : IScoreboardService
{
    private const string ScoreboardTemplate = "ScoreboardModule.Scoreboard";

    public async Task SendScoreboardAsync()
    {
        await manialinks.SendPersistentManialinkAsync(ScoreboardTemplate, await GetDataAsync());
        await nicknamesService.SendNicknamesManialinkAsync();
    }

    private async Task<dynamic> GetDataAsync()
    {
        var currentNextMaxPlayers = await server.Remote.GetMaxPlayersAsync();
        var currentNextMaxSpectators = await server.Remote.GetMaxSpectatorsAsync();
        var modeScriptSettings = await matchSettingsService.GetCurrentScriptSettingsAsync();

        return new
        {
            settings,
            MaxPlayers = currentNextMaxPlayers.CurrentValue + currentNextMaxSpectators.CurrentValue,
            PointsLimit = (int)(modeScriptSettings?["S_PointsLimit"] ?? 0),
            RoundsPerMap = (int)(modeScriptSettings?["S_RoundsPerMap"] ?? 0),
        };
    }

    public Task HideNadeoScoreboardAsync() =>
        gameModeUiModuleService.ApplyComponentSettingsAsync(
            GameModeUiComponents.ScoresTable,
            false,
            0.0,
            0.0,
            1.0
        );

    public Task ShowNadeoScoreboardAsync() =>
        gameModeUiModuleService.ApplyComponentSettingsAsync(
            GameModeUiComponents.ScoresTable,
            true,
            0.0,
            0.0,
            1.0
        );
}
