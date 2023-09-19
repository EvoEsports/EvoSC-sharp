using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.Scoreboard.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Scoreboard.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ScoreboardService : IScoreboardService
{
    private readonly ILogger<ScoreboardService> _logger;
    private readonly IManialinkManager _manialinks;
    private readonly IServerClient _server;
    private readonly IEvoScBaseConfig _config;
    private readonly IMatchSettingsService _matchSettingsService;

    private int _roundsPerMap = -1;
    private int _pointsLimit = -1;
    private int _currentRound = -1;
    private int _maxPlayers = -1;

    public ScoreboardService(IManialinkManager manialinks, IServerClient server, IEvoScBaseConfig config, IMatchSettingsService matchSettingsService, ILogger<ScoreboardService> logger)
    {
        _manialinks = manialinks;
        _server = server;
        _config = config;
        _matchSettingsService = matchSettingsService;
        _logger = logger;
    }

    public async Task ShowScoreboard()
    {
        await _manialinks.SendPersistentManialinkAsync("Scoreboard.Scoreboard", await GetData());
    }

    private Task<dynamic> GetData()
    {
        return Task.FromResult<dynamic>(new
        {
            MaxPlayers = _maxPlayers,
            RoundsPerMap = _roundsPerMap,
            PositionColors = new Dictionary<int, string> { { 1, "d1b104" }, { 2, "9e9e9e" }, { 3, "915d29" } },
            headerColor = _config.Theme.UI.HeaderBackgroundColor,
            primaryColor = _config.Theme.UI.PrimaryColor,
            positionBackgroundColor = _config.Theme.UI.Scoreboard.PositionBackgroundColor,
            backgroundColor = _config.Theme.UI.BackgroundColor,
            playerRowHighlightColor = _config.Theme.UI.Scoreboard.PlayerRowHighlightColor,
            playerRowBackgroundColor = _config.Theme.UI.PlayerRowBackgroundColor,
            logoUrl = _config.Theme.UI.LogoUrl
        });
    }

    public async Task HideNadeoScoreboard()
    {
        var hudSettings = new List<string>()
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

    public async Task ShowNadeoScoreboard()
    {
        var hudSettings = new List<string>()
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

    public async Task SendRequiredAdditionalInfos()
    {
        await _manialinks.SendPersistentManialinkAsync("Scoreboard.RoundsInfo",
            new { RoundsPerMap = _roundsPerMap, CurrentRound = _currentRound, PointsLimit = _pointsLimit });
    }

    public async Task LoadAndSendRequiredAdditionalInfos()
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

        _maxPlayers = (await _server.Remote.GetMaxPlayersAsync()).CurrentValue;
        _roundsPerMap = roundsPerMap;
        _pointsLimit = pointsLimit;
        
        await SendRequiredAdditionalInfos();
    }

    public void SetCurrentRound(int round)
    {
        _currentRound = round;
    }
}
