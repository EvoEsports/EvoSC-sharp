using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class FlowControlService : IFlowControlService
{
    private readonly IServerClient _server;
    private readonly IEventManager _events;
    private readonly IContextService _context;
    
    public FlowControlService(IServerClient server, IEventManager events, IContextService context)
    {
        _server = server;
        _events = events;
        _context = context;
    }

    public async Task EndRoundAsync()
    {
        await _server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.ForceEndRound");
        await _server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.WarmUp.ForceStopRound");

        await _events.RaiseAsync(FlowControlEvent.ForcedRoundEnd, EventArgs.Empty);
        
        _context
            .Audit().Success()
            .WithEventName(AuditEvents.EndRound)
            .Comment("Round ended");
    }

    public async Task RestartMatchAsync()
    {
        await _server.Remote.RestartMapAsync();
        await _events.RaiseAsync(FlowControlEvent.MatchRestarted, EventArgs.Empty);
        
        _context
            .Audit().Success()
            .WithEventName(AuditEvents.RestartMatch)
            .Comment("Match restarted");
    }

    public async Task SkipMapAsync()
    {
        await _server.Remote.NextMapAsync();
        await _events.RaiseAsync(FlowControlEvent.MapSkipped, EventArgs.Empty);
        
        _context.Audit().Success()
            .WithEventName(AuditEvents.SkipMap)
            .Comment("Map Skipped");
    }
}
