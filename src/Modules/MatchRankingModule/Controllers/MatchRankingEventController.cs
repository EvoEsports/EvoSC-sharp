using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Models;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchRankingModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchRankingModule.Controllers;

[Controller]
public class MatchRankingEventController : EvoScController<IEventControllerContext>
{
    private readonly IMatchRankingService _matchRankingService;
    private readonly ILogger<MatchRankingEventController> _logger;

    public MatchRankingEventController(IMatchRankingService matchRankingService, ILogger<MatchRankingEventController> logger)
    {
        _matchRankingService = matchRankingService;
        _logger = logger;
    }

    [Subscribe(ModeScriptEvent.Scores)]
    public async Task OnScores(object data, ScoresEventArgs eventArgs)
    {
        _logger.LogInformation("Scores, section: {Section}.", eventArgs.Section);
        
        if (eventArgs.Section is ModeScriptSection.EndMatch)
        {
            await _matchRankingService.ResetMatchData();
            await _matchRankingService.HideManialink();
            return;
        }
        
        await _matchRankingService.UpdateAndShowScores(eventArgs);
    }

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public async Task OnBeginMapAsync(object sender, RoundEventArgs args)
    {
        _logger.LogInformation("Start round.");
        await _matchRankingService.SendManialink();
    }

    [Subscribe(ModeScriptEvent.StartMatchStart)]
    public async Task OnStartMatch(object sender, MatchEventArgs eventArgs)
    {
        _logger.LogInformation("Start match start.");
        await _matchRankingService.ResetMatchData();
        await _matchRankingService.SendManialink();
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public async Task OnPodiumStart(object sender, PodiumEventArgs args)
    {
        _logger.LogInformation("Podium start.");
        await _matchRankingService.HideManialink();
    }
}
