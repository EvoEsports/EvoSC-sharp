using EvoSC.Common.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class FlowControlService : IFlowControlService
{
    private readonly IServerClient _server;
    private readonly IEventManager _events;
    
    public FlowControlService(IServerClient server, IEventManager events)
    {
        _server = server;
        _events = events;
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
