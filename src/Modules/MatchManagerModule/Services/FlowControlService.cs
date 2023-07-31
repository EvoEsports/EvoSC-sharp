using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
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
    private readonly dynamic _locale;
    
    public FlowControlService(IServerClient server, IEventManager events, IContextService context, Locale locale)
    {
        _server = server;
        _events = events;
        _context = context;
        _locale = locale;
    }

    public async Task EndRoundAsync()
    {
        await _server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.ForceEndRound");
        await _server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.WarmUp.ForceStopRound");

        await _events.RaiseAsync(FlowControlEvent.ForcedRoundEnd, EventArgs.Empty);
        
        _context
            .Audit().Success()
            .WithEventName(AuditEvents.EndRound)
            .Comment(_locale.Audit_RoundEnded);
    }

    public async Task RestartMatchAsync()
    {
        await _server.Remote.RestartMapAsync();
        await _events.RaiseAsync(FlowControlEvent.MatchRestarted, EventArgs.Empty);
        
        _context
            .Audit().Success()
            .WithEventName(AuditEvents.RestartMatch)
            .Comment(_locale.Audit_MatchRestarted);
    }

    public async Task SkipMapAsync()
    {
        await _server.Remote.NextMapAsync();
        await _events.RaiseAsync(FlowControlEvent.MapSkipped, EventArgs.Empty);
        
        _context.Audit().Success()
            .WithEventName(AuditEvents.SkipMap)
            .Comment(_locale.Audit_MapSkipped);
    }
}
