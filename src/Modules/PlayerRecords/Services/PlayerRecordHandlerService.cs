using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.PlayerRecords.Config;
using EvoSC.Modules.Official.PlayerRecords.Events;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class PlayerRecordHandlerService(IPlayerRecordsService playerRecords, IPlayerManagerService players,
        IEventManager events, IPlayerRecordSettings recordOptions, IServerClient server, IMapService maps,
        Locale locale)
    : IPlayerRecordHandlerService
{
    private readonly dynamic _locale = locale;

    public async Task CheckWaypointAsync(WayPointEventArgs waypoint)
    {
        if (!waypoint.IsEndRace)
        {
            return;
        }

        var map = await maps.GetOrAddCurrentMapAsync();
        var player = await players.GetOnlinePlayerAsync(waypoint.AccountId);
        var (record, status) =
            await playerRecords.SetPbRecordAsync(player, map, waypoint.RaceTime, waypoint.CurrentLapCheckpoints);

        await events.RaiseAsync(PlayerRecordsEvent.PbRecord, new PbRecordUpdateEventArgs
        {
            Player = player,
            Record = record,
            Map = map,
            Status = status
        });
    }

    public Task SendRecordUpdateToChatAsync(IPlayerRecord record) => recordOptions.EchoPb switch
    {
        EchoOptions.All => server.InfoMessageAsync(
            _locale.PlayerGotANewPb(record.Player.NickName, FormattingUtils.FormatTime(record.Score))),
        EchoOptions.Player => server.InfoMessageAsync(
            _locale.PlayerLanguage.YouGotANewPb(FormattingUtils.FormatTime(record.Score)), record.Player),
        _ => Task.CompletedTask
    };

    public async Task ShowCurrentPlayerPbAsync(IPlayer player)
    {
        var map = await maps.GetOrAddCurrentMapAsync();
        var pb = await playerRecords.GetPlayerRecordAsync(player, map);

        if (pb == null)
        {
            await server.InfoMessageAsync(_locale.PlayerLanguage.YouHaveNotSetATime, player);
            return;
        }

        var ms = pb.Score % 1000;
        var s = pb.Score / 1000 % 60;
        var m = pb.Score / 1000 / 60;
        var formattedTime = $"{(m > 0 ? m + ":" : "")}{s:00}.{ms:000}";

        await server.InfoMessageAsync(_locale.PlayerLanguage.YourCurrentPbIs(formattedTime), player);
    }
}
