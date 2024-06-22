using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Repository;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Modules.Official.LocalRecordsModule.Database.Models;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Database;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;
using LinqToDB;

namespace EvoSC.Modules.Official.LocalRecordsModule.Database.Repository;

[Service]
public class LocalRecordRepository(IDbConnectionFactory dbConnFactory, IPlayerRecordsRepository recordsRepository)
    : DbRepository(dbConnFactory), ILocalRecordRepository
{
    public async Task<IEnumerable<DbLocalRecord>> GetLocalRecordsOfMapByIdAsync(long mapId) =>
        await NewLoadAll()
            .Where(r => r.DbMap.Id == mapId)
            .OrderBy(r => r.Position)
            .ToArrayAsync();

    public async Task<DbLocalRecord> AddOrUpdateRecordAsync(IMap map, IPlayerRecord record)
    {
        // check if old record is better or equal, and dont bother updating if so
        var oldRecord = await GetRecordOfPlayerInMapAsync(record.Player, map);

        if (oldRecord != null && oldRecord.Record.CompareTo(record) <= 0)
        {
            return oldRecord;
        }

        var localRecord = new DbLocalRecord
        {
            MapId = map.Id,
            RecordId = record.Id,
            Position = 0,
            DbMap = new DbMap(map),
            DbRecord = new DbPlayerRecord(record)
        };
        
        var transaction = await Database.BeginTransactionAsync();

        try
        {
            // remove old record and add the new one
            if (oldRecord != null)
            {
                await Database.DeleteAsync(oldRecord);
            }
            
            var id = await Database.InsertWithIdentityAsync(localRecord);
            localRecord.Id = Convert.ToInt64(id);

            await transaction.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
        
        await RecalculatePositionsOfMapAsync(map);
        return localRecord;
    }

    public async Task RecalculatePositionsOfMapAsync(IMap map)
    {
        var locals = await GetLocalRecordsOfMapByIdAsync(map.Id);
        var sorted = locals.OrderBy(r => r.DbRecord.Score).ToArray();
        var updated = new List<DbLocalRecord>();
        
        for (var i = 0; i < sorted.Length; i++)
        {
            var newPos = i + 1;
            
            // don't update records that dont need it
            if (sorted[i].Position == newPos)
            {
                continue;
            }
            
            sorted[i].Position = newPos;
            updated.Add(sorted[i]);
        }

        var transaction = await Database.BeginTransactionAsync();

        try
        {
            foreach (var record in updated)
            {
                await Database.UpdateAsync(record);
            }
            
            await transaction.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackTransactionAsync();
        }
    }

    public async Task<IEnumerable<DbLocalRecord>> GetRecordsByPlayerAsync(IPlayer player) =>
        await NewLoadAll()
            .Where(r => r.DbRecord.DbPlayer.Id == player.Id)
            .ToArrayAsync();

    public async Task<DbLocalRecord?> GetRecordOfPlayerInMapAsync(IPlayer player, IMap map) =>
        await NewLoadAll()
            .FirstOrDefaultAsync(r => r.DbRecord.DbPlayer.Id == player.Id);

    public async Task DeleteRecordAsync(IPlayer player, IMap map)
    {
        var transaction = await Database.BeginTransactionAsync();

        try
        {
            await Table<DbLocalRecord>().DeleteAsync(r => r.MapId == map.Id && r.DbRecord.Player.Id == player.Id);
            await recordsRepository.DeleteRecordAsync(player, map);
        }
        catch (Exception ex)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task DeleteRecordAsync(ILocalRecord localRecord)
    {
        var transaction = await Database.BeginTransactionAsync();

        try
        {
            await Database.DeleteAsync(localRecord);
            await recordsRepository.DeleteRecordAsync(localRecord.Record);
        }
        catch (Exception ex)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    }

    private ILoadWithQueryable<DbLocalRecord, IPlayer> NewLoadAll() => Table<DbLocalRecord>()
        .LoadWith(r => r.DbMap)
        .LoadWith(r => r.DbRecord)
        .ThenLoad(r => r.DbPlayer);
}
