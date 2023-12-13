using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.PlayerRecords.Events;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;

namespace EvoSC.Modules.Official.PlayerRecords.Controllers;

[Controller]
public class PlayerEventsController(IPlayerRecordHandlerService playerRecordHandler) : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task OnWayPoint(object sender, WayPointEventArgs wayPoint) =>
        playerRecordHandler.CheckWaypointAsync(wayPoint);

    [Subscribe(PlayerRecordsEvent.PbRecord)]
    public Task OnPbRecord(object sender, PbRecordUpdateEventArgs pbUpdate) =>
        playerRecordHandler.SendRecordUpdateToChatAsync(pbUpdate.Record);
}
