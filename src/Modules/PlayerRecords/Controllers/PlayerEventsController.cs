using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.PlayerRecords.Events;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;

namespace EvoSC.Modules.Official.PlayerRecords.Controllers;

[Controller]
public class PlayerEventsController : EvoScController<EventControllerContext>
{
    private readonly IRecordEventService _recordService;

    public PlayerEventsController(IRecordEventService recordEventService) => _recordService = recordEventService;

    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task OnWayPoint(object sender, WayPointEventArgs wayPoint) =>
        _recordService.CheckWaypointAsync(wayPoint);

    [Subscribe(PlayerRecordsEvent.PbRecord)]
    public Task OnPbRecord(object sender, PbRecordUpdateEventArgs pbUpdate) =>
        _recordService.SendRecordUpdateToChatAsync(pbUpdate.Record);
}
