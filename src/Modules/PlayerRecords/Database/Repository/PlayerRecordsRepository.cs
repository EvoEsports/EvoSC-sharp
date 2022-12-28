using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;

namespace EvoSC.Modules.Official.PlayerRecords.Database.Repository;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class PlayerRecordsRepository : IPlayerRecordsRepository
{
    private readonly DbConnection _db;

    public PlayerRecordsRepository(DbConnection db) => _db = db;

    public async Task<DbPlayerRecord?> GetRecordAsync(IPlayer player, IMap map)
    {
        var sql = "select * from `PlayerRecords` where `PlayerId`=@PlayerId AND `MapId`=@MapId limit 1";
        var values = new {PlayerId = player.Id, MapId = map.Id};
        var record = await _db.QueryAsync<DbPlayerRecord, IPlayer, IMap, DbPlayerRecord>(sql,
            (record, player, map) =>
            {
                record.Player = player;
                record.Map = map;
                return record;
            }, values);

        return record?.FirstOrDefault();
    }

    public Task UpdateRecordAsync(DbPlayerRecord record) => _db.UpdateAsync(record);
    public Task InsertRecordAsync(DbPlayerRecord record) => _db.InsertAsync(record);
}
