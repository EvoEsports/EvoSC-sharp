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

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
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
    private const string MetaDataTemplate = "ScoreboardModule.MetaData";
    private readonly object _currentRoundMutex = new();
    private readonly object _isWarmUpMutex = new();
    private int _roundNumber = 1;
    private bool _isWarmUp;

    public async Task SendScoreboardAsync()
    {
        await SendMetaDataAsync();
        await manialinks.SendPersistentManialinkAsync(ScoreboardTemplate, await GetDataAsync());
        await nicknamesService.SendNicknamesManialinkAsync();
    }

    private async Task<dynamic> GetDataAsync()
    {
        var currentNextMaxPlayers = await server.Remote.GetMaxPlayersAsync();
        var currentNextMaxSpectators = await server.Remote.GetMaxSpectatorsAsync();

        return new
        {
            settings, MaxPlayers = currentNextMaxPlayers.CurrentValue + currentNextMaxSpectators.CurrentValue,
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

    public async Task SetCurrentRoundAsync(int roundNumber)
    {
        lock (_currentRoundMutex)
        {
            _roundNumber = roundNumber;
        }

        await SendMetaDataAsync();
    }

    public Task SetIsWarmUpAsync(bool isWarmUp)
    {
        lock (_isWarmUpMutex)
        {
            _isWarmUp = isWarmUp;
        }

        return Task.CompletedTask;
    }

    public async Task SendMetaDataAsync()
    {
        var modeScriptSettings = await matchSettingsService.GetCurrentScriptSettingsAsync();
        int roundNumber;
        bool isWarmUp;

        lock (_currentRoundMutex)
        {
            roundNumber = _roundNumber;
        }

        lock (_isWarmUpMutex)
        {
            isWarmUp = _isWarmUp;
        }

        await manialinks.SendPersistentManialinkAsync(MetaDataTemplate, new
        {
            roundNumber,
            isWarmUp,
            warmUpCount = (int)(modeScriptSettings?.GetValueOrDefault("S_WarmUpNb") ?? 0),
            roundsPerMap = (int)(modeScriptSettings?.GetValueOrDefault("S_RoundsPerMap") ?? 0),
            pointsLimit = (int)(modeScriptSettings?.GetValueOrDefault("S_PointsLimit") ?? 0),
        });
    }
}
