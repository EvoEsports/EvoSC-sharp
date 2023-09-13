using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchRankingModule.Interfaces;

namespace EvoSC.Modules.Official.MatchRankingModule.Controllers;

[Controller]
public class MatchRankingEventController : EvoScController<IEventControllerContext>
{
    private readonly IMatchRankingService _matchRankingService;

    public MatchRankingEventController(IMatchRankingService matchRankingService)
    {
        _matchRankingService = matchRankingService;
    }

    [Subscribe(ModeScriptEvent.Scores)]
    public async Task OnScores(object data, ScoresEventArgs eventArgs) =>
        await _matchRankingService.UpdateAndShowScores(eventArgs);

    [Subscribe(ModeScriptEvent.StartMapStart)]
    public async Task OnBeginMap(object sender, MapEventArgs args) =>
        await _matchRankingService.SendManialink();

    [Subscribe(ModeScriptEvent.EndMatchEnd)]
    public async Task OnMatchEnd(object sender, EventArgs eventArgs)
        => await _matchRankingService.ResetMatchDataAndShow();

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public async Task OnPodiumStart(object sender, PodiumEventArgs args) =>
        await _matchRankingService.HideManialink();
}
