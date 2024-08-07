using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.LiveRankingModule.Controllers;

[Controller]
public class LiveRankingEventController(ILiveRankingService service) : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.Scores)]
    public Task OnPlayerWaypointAsync(object sender, ScoresEventArgs args)
        => service.HandleScoresAsync(args);
}
