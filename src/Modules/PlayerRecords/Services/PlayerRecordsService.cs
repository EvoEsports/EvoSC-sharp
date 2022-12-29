using EvoSC.Common.Database.Repository.Maps;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Repository;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Util;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class PlayerRecordsService : IPlayerRecordsService
{
    private readonly IPlayerRecordsRepository _recordsRepo;
    private readonly IServerClient _server;
    private readonly IMapService _maps;
    private readonly IPlayerManagerService _players;
    private readonly MapRepository _mapsRepo;

    public PlayerRecordsService(IPlayerRecordsRepository recordsRepo, IServerClient server, IMapService maps,
        IPlayerManagerService players, MapRepository mapsRepo)
    {
        _recordsRepo = recordsRepo;
        _server = server;
        _maps = maps;
        _players = players;
        _mapsRepo = mapsRepo;
    }

    public async Task<IMap> GetOrAddCurrentMapAsync()
    {
        var currentMap = await _server.Remote.GetCurrentMapInfoAsync();
        var map = await _maps.GetMapByUid(currentMap.UId);
        
        if (map == null)
        {
            var mapAuthor = await _players.GetOrCreatePlayerAsync(PlayerUtils.ConvertLoginToAccountId(currentMap.Author));

            var mapMeta = new MapMetadata
            {
                MapUid = currentMap.UId,
                MapName = currentMap.Name,
                AuthorId = mapAuthor.AccountId,
                AuthorName = mapAuthor.NickName,
                ExternalId = currentMap.UId,
                ExternalVersion = null,
                ExternalMapProvider = null
            };

            map = await _mapsRepo.AddMap(mapMeta, mapAuthor, currentMap.FileName);
        }

        return map;
    }

    public async Task<(IPlayerRecord, RecordUpdateStatus)> SetPbRecordAsync(IPlayer player, IMap map, int score,
        IEnumerable<int> checkpoints)
    {
        var record = await _recordsRepo.GetRecordAsync(player, map);

        if (record == null)
        {
            record = new DbPlayerRecord
            {
                Player = player,
                PlayerId = player.Id,
                Map = map,
                MapId = map.Id,
                Score = score,
                RecordType = PlayerRecordType.Time,
                Checkpoints = string.Join(',', checkpoints),
                CreatedAt = default,
                UpdatedAt = default
            };

            await _recordsRepo.InsertRecordAsync(record);
            return (record, RecordUpdateStatus.New);
        }

        if (score >= record.Score)
        {
            return (record, score > record.Score ? RecordUpdateStatus.NotUpdated : RecordUpdateStatus.Equal);
        }

        record.Score = score;
        record.Checkpoints = string.Join(',', checkpoints);
        record.UpdatedAt = DateTime.UtcNow;
        await _recordsRepo.UpdateRecordAsync(record);

        return (record, RecordUpdateStatus.Updated);
    }
}
