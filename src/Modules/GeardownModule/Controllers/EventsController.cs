using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Evo.GeardownModule.Settings;
using EvoSC.Modules.Official.MatchReadyModule.Events;
using EvoSC.Modules.Official.MatchReadyModule.Events.Args;
using EvoSC.Modules.Official.MatchTrackerModule.Events;
using EvoSC.Modules.Official.MatchTrackerModule.Events.EventArgObjects;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Models;

namespace EvoSC.Modules.Evo.GeardownModule.Controllers;

[Controller]
public class EventsController : EvoScController<IEventControllerContext>
{
    private readonly IGeardownService _geardownService;
    private readonly IGeardownSettings _settings;

    public EventsController(IGeardownService geardownService, IGeardownSettings settings)
    {
        _geardownService = geardownService;
        _settings = settings;
    }

    [Subscribe(MatchReadyEvents.AllPlayersReady)]
    public async Task OnAllPlayersReadyAsync(object sender, AllPlayersReadyEventArgs args)
    {
        if (!_settings.AutomaticMatchStart)
        {
            return;
        }
        
        await _geardownService.StartMatchAsync();
    }

    [Subscribe(MatchTrackerEvent.StateTracked)]
    public async Task OnMatchStateTracked(object sender, MatchStateTrackedEventArgs args)
    {
        if (args.State.Status == MatchStatus.Ended)
        {
            await _geardownService.EndMatchAsync(args.Timeline);
        }
    }
}
