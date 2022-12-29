using EvoSC.Common.Config.Models;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.PlayerRecords.Config;
using EvoSC.Modules.Official.PlayerRecords.Events;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;

namespace EvoSC.Modules.Official.PlayerRecords.Controllers;

[Controller]
public class PlayerEventsController : EvoScController<EventControllerContext>
{
    private readonly IPlayerRecordsService _playerRecords;
    private readonly IPlayerManagerService _players;
    private readonly IEventManager _events;
    private readonly IPlayerRecordSettings _recordOptions;
    private readonly IServerClient _server;

    public PlayerEventsController(IPlayerRecordsService playerRecords, IPlayerManagerService players,
        IEventManager events, IPlayerRecordSettings recordOptions, IServerClient server)
    {
        _playerRecords = playerRecords;
        _players = players;
        _events = events;
        _recordOptions = recordOptions;
        _server = server;
    }

    [Subscribe(ModeScriptEvent.WayPoint)]
    public async Task OnWayPoint(object sender, WayPointEventArgs wayPoint)
    {
        if (!wayPoint.IsEndRace)
        {
            return;
        }

        var map = await _playerRecords.GetOrAddCurrentMapAsync();
        var player = await _players.GetOnlinePlayerAsync(wayPoint.AccountId);
        var (record, status) =
            await _playerRecords.SetPbRecordAsync(player, map, wayPoint.RaceTime, wayPoint.CurrentLapCheckpoints);

        await _events.Raise(PlayerRecordsEvent.PbRecord, new PbRecordUpdateEventArgs
        {
            Player = player,
            Record = record,
            Map = map,
            Status = status
        });
    }

    [Subscribe(PlayerRecordsEvent.PbRecord)]
    public Task OnPbRecord(object sender, PbRecordUpdateEventArgs pbUpdate)
    {
        switch (_recordOptions.EchoPb)
        {
            case EchoOptions.All:
                return _server.InfoMessage(
                    $"$<{pbUpdate.Player.NickName}$> got a new pb with time {FormattingUtils.FormatTime(pbUpdate.Record.Score)}");
            case EchoOptions.Player:
                return _server.InfoMessage(
                    $"You got a new pb with time {FormattingUtils.FormatTime(pbUpdate.Record.Score)}");
            case EchoOptions.None:
                break;
        }

        return Task.CompletedTask;
    }
}
