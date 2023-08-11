using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchManagerModule.Config;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchManagerModule.Controllers;

[Controller]
public class MatchTrackerEventController : EvoScController<IEventControllerContext>
{
    private readonly ITrackerSettings _settings;
    private readonly IMatchTracker _tracker;

    public MatchTrackerEventController(ITrackerSettings settings, IMatchTracker tracker)
    {
        _settings = settings;
        _tracker = tracker;
    }

    [Subscribe(ModeScriptEvent.Scores)]
    public Task OnScoresAsync(object sender, ScoresEventArgs args)
    {
        return _tracker.TrackScoresAsync(args);
    }

    [Subscribe(GbxRemoteEvent.BeginMatch)]
    public Task OnBeginMatchAsync(object sender, EventArgs args)
    {
        if (!_settings.AutomaticTracking)
        {
            return Task.CompletedTask;
        }

        return _tracker.BeginMatchAsync();
    }
}
