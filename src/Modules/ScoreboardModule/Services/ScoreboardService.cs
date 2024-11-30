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
    IScoreboardStateService scoreboardStateService,
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
    private const string MetaDataTemplate = "ScoreboardModule.MetaData";

    public async Task SendScoreboardAsync()
    {
        var currentNextMaxPlayers = await server.Remote.GetMaxPlayersAsync();
        var currentNextMaxSpectators = await server.Remote.GetMaxSpectatorsAsync();
        
        await SendMetaDataAsync();
        await manialinks.SendPersistentManialinkAsync(ScoreboardTemplate, new
        {
            settings,
            MaxPlayers = currentNextMaxPlayers.CurrentValue + currentNextMaxSpectators.CurrentValue,
        });
        await nicknamesService.SendNicknamesManialinkAsync();
    }

    public async Task SendMetaDataAsync()
    {
        var modeScriptSettings = await matchSettingsService.GetCurrentScriptSettingsAsync();
        int currentRoundNumber = await scoreboardStateService.GetCurrentRoundNumberAsync();
        bool isWarmUp = await scoreboardStateService.GetIsWarmUpAsync();

        await manialinks.SendPersistentManialinkAsync(MetaDataTemplate, new
        {
            roundNumber = currentRoundNumber,
            isWarmUp,
            warmUpCount = (int)(modeScriptSettings?.GetValueOrDefault("S_WarmUpNb") ?? 0),
            roundsPerMap = (int)(modeScriptSettings?.GetValueOrDefault("S_RoundsPerMap") ?? 0),
            pointsLimit = (int)(modeScriptSettings?.GetValueOrDefault("S_PointsLimit") ?? 0),
        });
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
