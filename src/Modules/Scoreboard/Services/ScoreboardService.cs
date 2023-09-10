using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.Scoreboard.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ScoreboardService : IScoreboardService
{
    private readonly IManialinkManager _manialinks;
    private readonly IPlayerManagerService _playerManager;
    private readonly IServerClient _server;
    private readonly IEvoScBaseConfig _config;

    private int _roundsPerMap = -1;
    private int _currentRound = 0;

    public ScoreboardService(IManialinkManager manialinks,
        IPlayerManagerService playerManager, IServerClient server, IEvoScBaseConfig config)
    {
        _manialinks = manialinks;
        _playerManager = playerManager;
        _server = server;
        _config = config;
    }

    public async Task ShowScoreboard(string playerLogin)
    {
        await _manialinks.SendManialinkAsync(
            await _playerManager.GetPlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerLogin)),
            "Scoreboard.Scoreboard",
            await GetData()
        );
        await SendRoundsInfo(playerLogin);
    }

    public async Task ShowScoreboard()
    {
        await _manialinks.SendManialinkAsync("Scoreboard.Scoreboard", await GetData());
    }

    private async Task<dynamic> GetData()
    {
        return new
        {
            MaxPlayers = await GetMaxPlayersAsync(),
            RoundsPerMap = _roundsPerMap,
            PositionColors = new Dictionary<int, string> { { 1, "d1b104" }, { 2, "9e9e9e" }, { 3, "915d29" } },
            headerColor = _config.Theme.UI.HeaderBackgroundColor,
            primaryColor = _config.Theme.UI.PrimaryColor,
            positionBackgroundColor = _config.Theme.UI.Scoreboard.PositionBackgroundColor,
            backgroundColor = _config.Theme.UI.BackgroundColor,
            playerRowHighlightColor = _config.Theme.UI.Scoreboard.PlayerRowHighlightColor,
            playerRowBackgroundColor = _config.Theme.UI.PlayerRowBackgroundColor,
            logoUrl = _config.Theme.UI.LogoUrl
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
            new { RoundsPerMap = _roundsPerMap, CurrentRound = _currentRound });
    }

    public async Task SendRoundsInfo(string playerLogin)
    {
        await _manialinks.SendManialinkAsync(
            await _playerManager.GetPlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerLogin)),
            "Scoreboard.RoundsInfo",
            new { RoundsPerMap = _roundsPerMap, CurrentRound = _currentRound }
        );
    }

    public async Task LoadAndUpdateRoundsPerMap()
    {
        _roundsPerMap = await GetRoundsPerMapAsync();
        await SendRoundsInfo();
    }

    public void SetCurrentRound(int round)
    {
        _currentRound = round;
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

    private async Task<int> GetMaxPlayersAsync()
    {
        return (await _server.Remote.GetMaxPlayersAsync()).CurrentValue;
    }
}
