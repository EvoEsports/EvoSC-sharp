using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchManagerModule.Config;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchManagerModule.Controllers;

[Controller]
public class MatchTrackerController : EvoScController<IEventControllerContext>
{
    private readonly ILogger<MatchTrackerController> _logger;
    private readonly ITrackerSettings _settings;
    private readonly IMatchTracker _tracker;
    
    public MatchTrackerController(ILogger<MatchTrackerController> logger, ITrackerSettings settings, IMatchTracker tracker)
    {
        _logger = logger;
        _settings = settings;
        _tracker = tracker;
    }
    
    [Subscribe(ModeScriptEvent.Scores)]
    public Task OnScores(object sender, ScoresEventArgs args)
    {
        if (!_settings.AutomaticTracking)
        {
            return Task.CompletedTask;
        }

        return _tracker.TrackScoresAsync(TODO);
    }
}
