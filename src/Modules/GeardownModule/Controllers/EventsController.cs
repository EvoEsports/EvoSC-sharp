using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Modules.Official.MatchReadyModule.Events;
using EvoSC.Modules.Official.MatchReadyModule.Events.Args;

namespace EvoSC.Modules.Evo.GeardownModule.Controllers;

[Controller]
public class EventsController : EvoScController<IEventControllerContext>
{
    [Subscribe(MatchReadyEvents.AllPlayersReady)]
    public async Task OnAllPlayersReadyAsync(object sender, AllPlayersReadyEventArgs args)
    {
        
    }
}
