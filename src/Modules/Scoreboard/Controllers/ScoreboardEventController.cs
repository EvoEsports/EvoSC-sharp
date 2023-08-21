using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Modules.Official.Scoreboard.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.Scoreboard.Controllers;

[Controller]
public class ScoreboardEventController : EvoScController<IEventControllerContext>
{
    private readonly IScoreboardService _scoreboardService;

    public ScoreboardEventController(IScoreboardService scoreboardService)
    {
        _scoreboardService = scoreboardService;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args)
        => await _scoreboardService.ShowScoreboard(args.Login);
    
    //TODO: catch round number from event
}
