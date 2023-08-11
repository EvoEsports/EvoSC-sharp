using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard.Controllers;

[Controller]
public class ScoreboardCommandsController : EvoScController<ICommandInteractionContext>
{
    private readonly IManialinkManager _manialinks;
    private readonly IServerClient _server;
    private readonly dynamic _locale;

    public ScoreboardCommandsController(IManialinkManager manialinks, IServerClient server, Locale locale)
    {
        _manialinks = manialinks;
        _locale = locale;
        _server = server;
    }

    [ChatCommand("sb", "[Command.ShowScoreboard]")]
    public async Task ShowScoreboard()
    {
        var hudSettings = new List<string>()
        {
            @"
{
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

        await _manialinks.SendManialinkAsync(Context.Player, "Scoreboard.Scoreboard",
            new { Locale = _locale, MaxPlayers = 64 });
    }

    [ChatCommand("fake", "[Command.FakePlayer]")]
    public async Task FakePlayer()
    {
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
    }
}
