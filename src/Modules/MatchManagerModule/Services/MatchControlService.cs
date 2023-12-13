using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchControlService(IServerClient server, IEventManager events) : IMatchControlService
{
    public async Task StartMatchAsync()
    {
        await RestartMatchAsync();

        await events.RaiseAsync(FlowControlEvent.MatchStarted, EventArgs.Empty);
    }

    public async Task EndMatchAsync()
    {
        await events.RaiseAsync(FlowControlEvent.MatchEnded, EventArgs.Empty);
    }

    public async Task EndRoundAsync()
    {
        await server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.ForceEndRound");
        await server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.WarmUp.ForceStopRound");

        await events.RaiseAsync(FlowControlEvent.ForcedRoundEnd, EventArgs.Empty);
    }

    public async Task RestartMatchAsync()
    {
        await server.Remote.RestartMapAsync();
        
        await events.RaiseAsync(FlowControlEvent.MatchRestarted, EventArgs.Empty);
    }

    public async Task SkipMapAsync()
    {
        await server.Remote.NextMapAsync();
        
        await events.RaiseAsync(FlowControlEvent.MapSkipped, EventArgs.Empty);
    }
}
