using EvoSC.Common.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class FlowControlService : IFlowControlService
{
    private readonly IServerClient _server;
    
    public FlowControlService(IServerClient server)
    {
        _server = server;
    }

    public async Task EndRoundAsync()
    {
        await _server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.ForceEndRound");
        await _server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.WarmUp.ForceStopRound");
    }

    public Task RestartMatchAsync() => _server.Remote.RestartMapAsync();

    public Task SkipMapAsync() => _server.Remote.NextMapAsync();
}
