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
public class MatchTrackerEventController(ITrackerSettings settings, IMatchTracker tracker) : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.Scores)]
    public Task OnScoresAsync(object sender, ScoresEventArgs args)
    {
        return tracker.TrackScoresAsync(args);
    }

    [Subscribe(GbxRemoteEvent.BeginMatch)]
    public Task OnBeginMatchAsync(object sender, EventArgs args)
    {
        if (!settings.AutomaticTracking)
        {
            return Task.CompletedTask;
        }

        return tracker.BeginMatchAsync();
    }

    [Subscribe(FlowControlEvent.MatchStarted)]
    public Task OnMatchStarted(object sender, EventArgs args)
    {
        if (settings.AutomaticTracking)
        {
            return Task.CompletedTask;
        }
        
        return tracker.BeginMatchAsync();
    }

    [Subscribe(FlowControlEvent.MatchEnded)]
    public Task OnMatchEnded(object sender, EventArgs args) => tracker.EndMatchAsync();

    [Subscribe(FlowControlEvent.MatchRestarted)]
    public Task OnMatchRestarted(object sender, EventArgs args) => tracker.BeginMatchAsync();
}
