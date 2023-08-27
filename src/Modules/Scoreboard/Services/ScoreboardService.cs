using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.Scoreboard.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Scoreboard.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ScoreboardService : IScoreboardService
{
    private readonly IManialinkManager _manialinks;
    private readonly IPlayerManagerService _playerManager;
    private readonly IServerClient _server;
    private readonly ILogger<ScoreboardService> _logger;

    private int _roundsPerMap = -1;

    public ScoreboardService(ILogger<ScoreboardService> logger, IManialinkManager manialinks,
        IPlayerManagerService playerManager,
        IServerClient server)
    {
        _logger = logger;
        _manialinks = manialinks;
        _playerManager = playerManager;
        _server = server;
    }

    public async Task ShowScoreboard(string playerLogin)
    {
        await _manialinks.SendManialinkAsync(
            await _playerManager.GetPlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerLogin)),
            "Scoreboard.Scoreboard", GetData());
    }

    public async Task ShowScoreboard()
    {
        await _manialinks.SendManialinkAsync("Scoreboard.Scoreboard", GetData());
    }

    private dynamic GetData()
    {
        return new
        {
            MaxPlayers = 64,
            RoundsPerMap = _roundsPerMap,
            PositionColors = new Dictionary<int, string> { { 1, "d1b104" }, { 2, "9e9e9e" }, { 3, "915d29" } }
        };
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

    public async Task SendRoundsInfo()
    {
        await _manialinks.SendManialinkAsync("Scoreboard.RoundsInfo",
            new { RoundsPerMap = _roundsPerMap, CurrentRound = -1 });
    }

    public async void LoadAndUpdateRoundsPerMap()
    {
        _roundsPerMap = await GetRoundsPerMapAsync();
        _logger.LogInformation("Rounds per Map: {mode}", _roundsPerMap);
        await SendRoundsInfo();
    }

    private async Task<int> GetRoundsPerMapAsync()
    {
        var variables = await _server.Remote.GetModeScriptSettingsAsync();
        if (!variables.TryGetValue("S_RoundsPerMap", out var rounds))
        {
            return -1;
        }

        return (int)rounds;
    }
}
