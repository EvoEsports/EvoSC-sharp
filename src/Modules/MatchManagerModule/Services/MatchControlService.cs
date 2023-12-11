using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchControlService : IMatchControlService
{
    private readonly IServerClient _server;
    private readonly IEventManager _events;
    
    public MatchControlService(IServerClient server, IEventManager events)
    {
        _server = server;
        _events = events;
    }

    public async Task StartMatchAsync()
    {
        await RestartMatchAsync();

        await _events.RaiseAsync(FlowControlEvent.MatchStarted, EventArgs.Empty);
    }

    public async Task EndMatchAsync()
    {
        await _events.RaiseAsync(FlowControlEvent.MatchEnded, EventArgs.Empty);
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
