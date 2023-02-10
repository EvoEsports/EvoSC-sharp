using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Models.Audit;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class FlowControlService : IFlowControlService
{
    private readonly IServerClient _server;
    private readonly IEventManager _events;
    private readonly IContextService _contextService;
    
    public FlowControlService(IServerClient server, IEventManager events, IContextService contextService)
    {
        _server = server;
        _events = events;
        _contextService = contextService;
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
        
        var map = await _server.Remote.GetCurrentMapInfoAsync();

        _contextService
            .GetContext().AuditEvent
            .AsSuccess()
            .WithEventName("RestartMatch")
            .WithProperties(new {MapName = map.Name, MapUid = map.UId})
            .WithComment("Match restarted");
    }

    public async Task SkipMapAsync()
    {
        await _server.Remote.NextMapAsync();
        await _events.RaiseAsync(FlowControlEvent.MapSkipped, EventArgs.Empty);
    }
}
