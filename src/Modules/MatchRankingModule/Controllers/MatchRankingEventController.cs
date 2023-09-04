using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchRankingModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.MatchRankingModule.Controllers;

[Controller]
public class MatchRankingEventController: EvoScController<IEventControllerContext>
{
    private readonly IMatchRankingService _matchRankingService;

    public MatchRankingEventController(IMatchRankingService matchRankingService)
    {
        _matchRankingService = matchRankingService;
    }
    
    [Subscribe(ModeScriptEvent.Scores)]
    public Task OnScores(object data, ScoresEventArgs eventArgs) => _matchRankingService.OnScores(eventArgs);

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnect(object data, PlayerConnectGbxEventArgs eventArgs) => _matchRankingService.SendManialink(eventArgs.Login);

    [Subscribe(GbxRemoteEvent.EndMatch)]
    public Task OnMatchEnd(object data, EndMatchGbxEventArgs eventArgs) => _matchRankingService.Reset();
}
