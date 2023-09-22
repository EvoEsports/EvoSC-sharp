using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.MatchManagerModule.Permissions;
using EvoSC.Modules.Official.Scoreboard.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard.Controllers;

[Controller]
public class ScoreboardCommandsController : EvoScController<ICommandInteractionContext>
{
    private readonly IServerClient _server;
    private readonly IScoreboardService _scoreboardService;

    public ScoreboardCommandsController(IServerClient server, IScoreboardService scoreboardService)
    {
        _server = server;
        _scoreboardService = scoreboardService;
    }
    
    [ChatCommand("sb", "[Command.ShowScoreboard]")]
    public async Task ShowScoreboard()
    {
        await _scoreboardService.ShowScoreboard(Context.Player.GetLogin());
    }

    [ChatCommand("fake", "[Command.FakePlayer]", MatchManagerPermissions.LoadMatchSettings)]
    public async Task FakePlayer()
    {
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
        await _server.Remote.ConnectFakePlayerAsync();
    }
}
