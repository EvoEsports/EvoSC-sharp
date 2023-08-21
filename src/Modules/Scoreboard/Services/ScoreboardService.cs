using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
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
    private readonly Locale _locale;
    private readonly IPlayerManagerService _playerManager;
    private readonly IServerClient _server;

    public ScoreboardService(IManialinkManager manialinks, Locale locale, IPlayerManagerService playerManager,
        IServerClient server)
    {
        _manialinks = manialinks;
        _locale = locale;
        _playerManager = playerManager;
        _server = server;
    }

    public async Task ShowScoreboard(string playerLogin)
    {
        await _manialinks.SendManialinkAsync(
            await _playerManager.GetPlayerAsync(PlayerUtils.ConvertLoginToAccountId(playerLogin)),
            "Scoreboard.Scoreboard",
            new { Locale = _locale, MaxPlayers = 64 });
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
}
