using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchTrackerModule.Config;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchTrackerModule.Controllers;

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

    [Subscribe(FlowControlEvent.MatchStarted)]
    public Task OnMatchStarted(object sender, EventArgs args) => _tracker.BeginMatchAsync();

    [Subscribe(FlowControlEvent.MatchEnded)]
    public Task OnMatchEnded(object sender, EventArgs args) => _tracker.EndMatchAsync();

    [Subscribe(FlowControlEvent.MatchRestarted)]
    public Task OnMatchRestarted(object sender, EventArgs args) => _tracker.BeginMatchAsync();
}
