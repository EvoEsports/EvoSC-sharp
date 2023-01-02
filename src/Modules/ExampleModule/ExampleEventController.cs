using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class ExampleEventController : EvoScController<EventControllerContext>
{
    private readonly ILogger<ExampleEventController> _logger;
    
    public ExampleEventController(ILogger<ExampleEventController> logger)
    {
        _logger = logger;
    }
    
    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task OnWaypoint(object sender, WayPointEventArgs args)
    {
        _logger.LogInformation("Player waypoint, {Player}: {Time}", args.AccountId, args.RaceTime);
        return Task.CompletedTask;
    }
}
