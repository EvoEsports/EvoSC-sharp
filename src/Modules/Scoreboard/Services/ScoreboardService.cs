using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.Scoreboard.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ScoreboardService : IScoreboardService
{
    private readonly IManialinkManager _manialinks;
    private readonly IServerClient _server;
    private readonly IEvoScBaseConfig _config;
    private readonly IMatchSettingsService _matchSettingsService;
    private readonly IScoreboardTrackerService _scoreboardTracker;
    private readonly IThemeManager _themes;

    public ScoreboardService(IManialinkManager manialinks, IServerClient server, IEvoScBaseConfig config,
        IMatchSettingsService matchSettingsService, IScoreboardTrackerService scoreboardTracker, IThemeManager themes)
    {
        _manialinks = manialinks;
        _server = server;
        _config = config;
        _matchSettingsService = matchSettingsService;
        _scoreboardTracker = scoreboardTracker;
        _themes = themes;
    }

    public async Task ShowScoreboardToAllAsync()
    {
        await _manialinks.SendPersistentManialinkAsync("Scoreboard.Scoreboard", await GetDataAsync());
    }

    public async Task ShowScoreboardAsync(IPlayer playerLogin)
    {
        await _manialinks.SendManialinkAsync(playerLogin, "Scoreboard.Scoreboard", await GetDataAsync());
    }

    private Task<dynamic> GetDataAsync()
    {
        return Task.FromResult<dynamic>(new
        {
            _scoreboardTracker.MaxPlayers,
            _scoreboardTracker.RoundsPerMap,
            PositionColors = new Dictionary<int, string> { { 1, "d1b104" }, { 2, "9e9e9e" }, { 3, "915d29" } },
            headerColor = _themes.Theme.UI_HeaderBackground,
            primaryColor = _themes.Theme.UI_TextPrimary,
            positionBackgroundColor = _themes.Theme.UI_Scoreboard_PositionBackgroundColor,
            backgroundColor = _themes.Theme.UI_BackgroundPrimary,
            playerRowHighlightColor = _themes.Theme.UI_Scoreboard_PlayerRowHighlightColor,
            playerRowBackgroundColor = _themes.Theme.UI_RowBackground,
            logoUrl = _themes.Theme.UI_LogoDark
        });
    }

    public async Task HideNadeoScoreboardAsync()
    {
        var hudSettings = new List<string>
        {
            @"{
    ""uimodules"": [
        {
            ""id"": ""Race_ScoresTable"",
            ""position"": [-50,0],
            ""scale"": 1,
            ""visible"": false,
            ""visible_update"": true
        }
    ]
}"
        };

        await _server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", hudSettings.ToArray());
    }

    public async Task ShowNadeoScoreboardAsync()
    {
        var hudSettings = new List<string>
        {
            @"{
    ""uimodules"": [
        {
            ""id"": ""Race_ScoresTable"",
            ""position"": [-50,0],
            ""scale"": 1,
            ""visible"": true,
            ""visible_update"": true
        }
    ]
}"
        };

        await _server.Remote.TriggerModeScriptEventArrayAsync("Common.UIModules.SetProperties", hudSettings.ToArray());
    }

    public async Task SendRequiredAdditionalInfoAsync()
    {
        await _manialinks.SendPersistentManialinkAsync("Scoreboard.RoundsInfo",
            new { _scoreboardTracker.RoundsPerMap, _scoreboardTracker.CurrentRound, _scoreboardTracker.PointsLimit });
    }

    public async Task LoadAndSendRequiredAdditionalInfoAsync()
    {
        var settings = await _matchSettingsService.GetCurrentScriptSettingsAsync();

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

        _scoreboardTracker.MaxPlayers = (await _server.Remote.GetMaxPlayersAsync()).CurrentValue;
        _scoreboardTracker.RoundsPerMap = roundsPerMap;
        _scoreboardTracker.PointsLimit = pointsLimit;
        
        await SendRequiredAdditionalInfoAsync();
    }

    public void SetCurrentRound(int round)
    {
        _scoreboardTracker.CurrentRound = round;
    }
}
