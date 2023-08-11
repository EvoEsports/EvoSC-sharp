using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Events.EventArgObjects;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchControlService : IMatchControlService
{
    private readonly IServerClient _server;
    private readonly IEventManager _events;
    private readonly IMatchTracker _matchTracker;
    
    public MatchControlService(IServerClient server, IEventManager events, IMatchTracker matchTracker)
    {
        _server = server;
        _events = events;
        _matchTracker = matchTracker;
    }

    public async Task<Guid> StartMatchAsync()
    {
        await RestartMatchAsync();
        var timelineId = await _matchTracker.BeginMatchAsync();

        await _events.RaiseAsync(FlowControlEvent.MatchStarted, new MatchStartedEventArgs {TimelineId = timelineId});

        return timelineId;
    }

    public async Task<Guid> EndMatchAsync()
    {
        var timeline = await _matchTracker.EndMatchAsync();

        await _events.RaiseAsync(FlowControlEvent.MatchEnded, new MatchEndedEventArgs {Timeline = timeline});

        return timeline.TimelineId;
    }

    public async Task EndRoundAsync()
    {
        await _server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.ForceEndRound");
        await _server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.WarmUp.ForceStopRound");

        await _events.RaiseAsync(FlowControlEvent.ForcedRoundEnd, EventArgs.Empty);
    }

    public async Task RestartMatchAsync()
    {
        await _server.Remote.RestartMapAsync();
        
        await _events.RaiseAsync(FlowControlEvent.MatchRestarted, EventArgs.Empty);
    }

    public async Task SkipMapAsync()
    {
        await _server.Remote.NextMapAsync();
        
        await _events.RaiseAsync(FlowControlEvent.MapSkipped, EventArgs.Empty);
    }
}
