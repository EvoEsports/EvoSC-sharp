using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Models.Players;
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
        var sql = """
                select * from `PlayerRecords`
                inner join `Maps` on `PlayerRecords`.MapId=`Maps`.Id
                inner join `Players` on `PlayerRecords`.PlayerId=`Players`.Id
                where `PlayerId`=@PlayerId AND `MapId`=@MapId 
                limit 1
                """;
        
        var values = new {PlayerId = player.Id, MapId = map.Id};
        var record = await _db.QueryAsync<DbPlayerRecord, DbPlayer, DbMap, DbPlayerRecord>(sql,
            (playerRecord, dbPlayer, dbMap) =>
            {
                playerRecord.Player = new Player(dbPlayer);
                playerRecord.Map = new Map(dbMap);
                return playerRecord;
            }, values);

        return record?.FirstOrDefault();
    }

    public Task UpdateRecordAsync(DbPlayerRecord record) => _db.UpdateAsync(record);
    public Task InsertRecordAsync(DbPlayerRecord record) => _db.InsertAsync(record);
}
