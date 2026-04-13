using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchTrackerModule.Config;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.MatchTrackerModule.Controllers;

[Controller]
public class MatchTrackerEventController(ITrackerSettings settings, IMatchTracker tracker) : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.Scores)]
    public Task OnScoresAsync(object sender, ScoresEventArgs args)
    {
        return tracker.TrackScoresAsync(args);
    }

    [Subscribe(ModeScriptEvent.StartMatchStart)]
    public Task OnBeginMatchAsync(object sender, EventArgs args)
    {
        return settings.AutomaticTracking ? tracker.BeginMatchAsync() : Task.CompletedTask;
    }

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public Task OnBeginMapAsync(object sender, MapGbxEventArgs args)
    {
        return settings.RecordMapChanges ? tracker.TrackMapChangeAsync(args) : Task.CompletedTask;
    }

    [Subscribe(ModeScriptEvent.EndMatchStart)]
    public Task OnMatchEndedAsync(object sender, EventArgs args)
    {
        return tracker.EndMatchAsync();
    }

    [Subscribe(FlowControlEvent.MatchStarted)]
    public Task OnMatchStarted(object sender, EventArgs args)
    {
        return settings.AutomaticTracking ? Task.CompletedTask : tracker.BeginMatchAsync();
    }

    [Subscribe(FlowControlEvent.MatchEnded)]
    public Task OnMatchEnded(object sender, EventArgs args) => tracker.EndMatchAsync();

    [Subscribe(FlowControlEvent.MatchRestarted)]
    public Task OnMatchRestarted(object sender, EventArgs args) => tracker.BeginMatchAsync();
}
