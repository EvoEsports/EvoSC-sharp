using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Enums;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;
using EvoSC.Modules.Official.Scoreboard.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ScoreboardService(
    IManialinkManager manialinks,
    IServerClient server,
    IMatchSettingsService matchSettingsService,
    IScoreboardTrackerService scoreboardTracker,
    IGameModeUiModuleService gameModeUiModuleService,
    IThemeManager themes
)
    : IScoreboardService
{
    private const string ScoreboardTemplate = "Scoreboard.Scoreboard";

    public async Task ShowScoreboardToAllAsync()
    {
        await manialinks.SendPersistentManialinkAsync(ScoreboardTemplate, await GetDataAsync());
    }

    public async Task ShowScoreboardAsync(IPlayer playerLogin)
    {
        await manialinks.SendManialinkAsync(playerLogin, ScoreboardTemplate, await GetDataAsync());
    }

    private Task<dynamic> GetDataAsync()
    {
        return Task.FromResult<dynamic>(new
        {
            scoreboardTracker.MaxPlayers,
            scoreboardTracker.RoundsPerMap,
            PositionColors = new Dictionary<int, string>
            {
                { 1, themes.Theme.Gold }, { 2, themes.Theme.Silver }, { 3, themes.Theme.Bronze }
            },
        });
    }

    public Task HideNadeoScoreboardAsync() =>
        gameModeUiModuleService.ApplyComponentSettingsAsync(GameModeUiComponents.ScoresTable, false, 0, 0, 1);

    public Task ShowNadeoScoreboardAsync() =>
        gameModeUiModuleService.ApplyComponentSettingsAsync(GameModeUiComponents.ScoresTable, false, -50, 0, 1);

    public async Task SendRequiredAdditionalInfoAsync()
    {
        await manialinks.SendPersistentManialinkAsync("Scoreboard.RoundsInfo",
            new { scoreboardTracker.RoundsPerMap, scoreboardTracker.CurrentRound, scoreboardTracker.PointsLimit });
    }

    public async Task LoadAndSendRequiredAdditionalInfoAsync()
    {
        var settings = await matchSettingsService.GetCurrentScriptSettingsAsync();

        if (settings == null)
        {
            return;
        }

        var roundsPerMap = -1;
        var pointsLimit = -1;

        if (settings.TryGetValue("S_RoundsPerMap", out var rounds))
        {
            roundsPerMap = (int)rounds;
        }

        if (settings.TryGetValue("S_PointsLimit", out var pointsLimitValue))
        {
            pointsLimit = (int)pointsLimitValue;
        }

        scoreboardTracker.MaxPlayers = (await server.Remote.GetMaxPlayersAsync()).CurrentValue;
        scoreboardTracker.RoundsPerMap = roundsPerMap;
        scoreboardTracker.PointsLimit = pointsLimit;

        await SendRequiredAdditionalInfoAsync();
    }

    public void SetCurrentRound(int round)
    {
        scoreboardTracker.CurrentRound = round;
    }
}
