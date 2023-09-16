using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Models;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchRankingModule.Interfaces;
using GbxRemoteNet.Events;
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
        if (eventArgs.Section == ModeScriptSection.EndMatch)
        {
            _logger.LogInformation("End match.");
            await _matchRankingService.ResetMatchData();
            await _matchRankingService.HideManialink();
            return;
        }
        
        _logger.LogInformation("Scores, section: {Section}.", eventArgs.Section);
        await _matchRankingService.UpdateAndShowScores(eventArgs);
    }

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMapAsync(object sender, MapGbxEventArgs args)
    {
        _logger.LogInformation("Begin map.");
        await _matchRankingService.SendManialink();
    }

    // [Subscribe(ModeScriptEvent.EndMapStart)]
    // public async Task OnEndMap(object sender, MapEventArgs args)
    // {
    //     _logger.LogInformation("End map start.");
    //     await _matchRankingService.HideManialink();
    // }

    [Subscribe(ModeScriptEvent.StartMatchStart)]
    public async Task OnStartMatch(object sender, MatchEventArgs eventArgs)
    {
        _logger.LogInformation("Start match start.");
        // await _matchRankingService.ResetMatchData();
        await _matchRankingService.SendManialink();
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public async Task OnPodiumStart(object sender, PodiumEventArgs args)
    {
        _logger.LogInformation("Podium start.");
        await _matchRankingService.HideManialink();
    }
}
