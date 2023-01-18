using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.PlayerRecords.Config;
using EvoSC.Modules.Official.PlayerRecords.Events;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class PlayerRecordHandlerService : IPlayerRecordHandlerService
{
    private readonly IPlayerRecordsService _playerRecords;
    private readonly IPlayerManagerService _players;
    private readonly IEventManager _events;
    private readonly IPlayerRecordSettings _recordOptions;
    private readonly IServerClient _server;
    
    public PlayerRecordHandlerService(IPlayerRecordsService playerRecords, IPlayerManagerService players,
        IEventManager events, IPlayerRecordSettings recordOptions, IServerClient server)
    {
        _playerRecords = playerRecords;
        _players = players;
        _events = events;
        _recordOptions = recordOptions;
        _server = server;
    }
    
    public async Task CheckWaypointAsync(WayPointEventArgs waypoint)
    {
        if (!waypoint.IsEndRace)
        {
            return;
        }

        var map = await _playerRecords.GetOrAddCurrentMapAsync();
        var player = await _players.GetOnlinePlayerAsync(waypoint.AccountId);
        var (record, status) =
            await _playerRecords.SetPbRecordAsync(player, map, waypoint.RaceTime, waypoint.CurrentLapCheckpoints);

        await _events.Raise(PlayerRecordsEvent.PbRecord, new PbRecordUpdateEventArgs
        {
            Player = player,
            Record = record,
            Map = map,
            Status = status
        });
    }

    public Task SendRecordUpdateToChatAsync(IPlayerRecord record) => _recordOptions.EchoPb switch
    {
        EchoOptions.All => _server.InfoMessage(
            $"$<{record.Player.NickName}$> got a new pb with time {FormattingUtils.FormatTime(record.Score)}"),
        EchoOptions.Player => _server.InfoMessage(
            $"You got a new pb with time {FormattingUtils.FormatTime(record.Score)}", record.Player),
        _ => Task.CompletedTask
    };

    public async Task ShowCurrentPlayerPbAsync(IPlayer player)
    {
        var map = await _playerRecords.GetOrAddCurrentMapAsync();
        var pb = await _playerRecords.GetPlayerRecordAsync(player, map);

        if (pb == null)
        {
            await _server.InfoMessage("You have not set a time on this map yet.");
            return;
        }

        var ms = pb.Score % 1000;
        var s = pb.Score / 1000 % 60;
        var m = pb.Score / 1000 / 60;
        var formattedTime = $"{(m > 0 ? m + ":" : "")}{s:00}.{ms:000}";

        await _server.InfoMessage($"Your current pb is $<$fff{formattedTime}$>");
    }
}
