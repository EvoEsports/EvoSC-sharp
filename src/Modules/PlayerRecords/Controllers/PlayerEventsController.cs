using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Repository.Maps;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Repository;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using GBX.NET;
using GBX.NET.Engines.Game;

namespace EvoSC.Modules.Official.PlayerRecords.Controllers;

[Controller]
public class PlayerEventsController : EvoScController<EventControllerContext>
{
    private readonly IPlayerRecordsService _playerRecords;
    private readonly IServerClient _server;
    private readonly IMapService _maps;
    private readonly MapRepository _mapsRepo;
    private readonly IPlayerManagerService _players;

    public PlayerEventsController(IPlayerRecordsService playerRecords, IServerClient server, IMapService maps,
        MapRepository mapsRepo, IPlayerManagerService players)
    {
        _playerRecords = playerRecords;
        _server = server;
        _maps = maps;
        _mapsRepo = mapsRepo;
        _players = players;
    }

    [Subscribe(ModeScriptEvent.WayPoint)]
    public async Task OnWayPoint(object sender, WayPointEventArgs wayPoint)
    {
        if (!wayPoint.IsEndRace)
        {
            return;
        }
        
        var currentMap = await _server.Remote.GetCurrentMapInfoAsync();
        var map = await _maps.GetMapByUid(currentMap.UId);
        var player = await _players.GetOnlinePlayerAsync(wayPoint.AccountId);

        if (map == null)
        {
            var mapAuthor = await _players.GetOrCreatePlayerAsync(PlayerUtils.ConvertLoginToAccountId(currentMap.Author));
            var dbMap = new DbMap
            {
                Uid = currentMap.Author,
                FilePath = currentMap.FileName,
                AuthorId = mapAuthor.Id,
                Enabled = true,
                Name = currentMap.Name,
                ExternalId = null,
                ExternalVersion = null,
                ExternalMapProvider = null,
                CreatedAt = default,
                UpdatedAt = default,
                Author = mapAuthor
            };

            var mapMeta = new MapMetadata
            {
                MapUid = currentMap.Author,
                MapName = currentMap.Name,
                AuthorId = mapAuthor.AccountId,
                AuthorName = mapAuthor.NickName,
                ExternalId = "",
                ExternalVersion = null,
                ExternalMapProvider = null
            };

            map = await _mapsRepo.AddMap(mapMeta, player, currentMap.FileName);
            await _playerRecords.AddRecordAsync(player, map, wayPoint.RaceTime, wayPoint.CurrentRaceCheckpoints);
        }
    }
}
