using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.PlayerRecords.Events;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;

namespace EvoSC.Modules.Official.PlayerRecords.Controllers;

[Controller]
public class PlayerEventsController : EvoScController<IEventControllerContext>
{
    private readonly IPlayerRecordHandlerService _playerRecordHandler;

    public PlayerEventsController(IPlayerRecordHandlerService playerRecordHandler) =>
        _playerRecordHandler = playerRecordHandler;

    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task OnWayPoint(object sender, WayPointEventArgs wayPoint) =>
        _playerRecordHandler.CheckWaypointAsync(wayPoint);

    [Subscribe(PlayerRecordsEvent.PbRecord)]
    public Task OnPbRecord(object sender, PbRecordUpdateEventArgs pbUpdate) =>
        _playerRecordHandler.SendRecordUpdateToChatAsync(pbUpdate.Record);
}
