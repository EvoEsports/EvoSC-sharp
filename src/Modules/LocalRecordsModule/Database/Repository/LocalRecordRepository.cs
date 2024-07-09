using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Repository;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Modules.Official.LocalRecordsModule.Config;
using EvoSC.Modules.Official.LocalRecordsModule.Database.Models;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Database;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;
using LinqToDB;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.LocalRecordsModule.Database.Repository;

[Service]
public class LocalRecordRepository(
    IDbConnectionFactory dbConnFactory,
    IPlayerRecordsRepository recordsRepository,
    ILocalRecordsSettings settings,
    ILogger<LocalRecordRepository> logger)
    : DbRepository(dbConnFactory), ILocalRecordRepository
{
    public async Task<IEnumerable<DbLocalRecord>> GetLocalRecordsOfMapByIdAsync(long mapId) =>
        await NewLoadAll()
            .Where(r => r.DbMap.Id == mapId)
            .OrderBy(r => r.Position)
            .ToArrayAsync();

    public async Task<DbLocalRecord?> AddOrUpdateRecordAsync(IMap map, IPlayerRecord record)
    {
        // check if old record is better or equal, and dont bother updating if so
        var oldRecord = await GetRecordOfPlayerInMapAsync(record.Player, map);

        if (oldRecord != null && oldRecord.Record.CompareTo(record) <= 0)
        {
            logger.LogDebug("Player has old record that is better or equal");
            return oldRecord;
        }

        var worstRecord = await NewLoadAll()
            .Where(r => r.DbMap.Id == map.Id)
            .OrderByDescending(r => r.Position)
            .FirstOrDefaultAsync();

        if (worstRecord != null && worstRecord.Position >= settings.MaxRecordsPerMap && worstRecord.Record.CompareTo(record) < 0)
        {
            logger.LogDebug("player got a new record that is worse than the worst local record of the map");
            return null;
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

        var updated = await RecalculatePositionsOfMapAsync(map);
        var updatedRecord = updated.FirstOrDefault(r => r.Id == localRecord.Id);
        
        logger.LogDebug("player got a new local record");
        return updatedRecord;
    }

    public async Task AddRecordsAsync(IMap map, IEnumerable<IPlayerRecord> records)
    {
        var transaction = await Database.BeginTransactionAsync();

        try
        {
            foreach (var record in records)
            {
                await Database.InsertWithIdentityAsync(new DbLocalRecord
                {
                    MapId = map.Id,
                    RecordId = record.Id,
                    Position = 0,
                    DbMap = new DbMap(map),
                    DbRecord = new DbPlayerRecord(record)
                });
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }

        await transaction.CommitTransactionAsync();
        await RecalculatePositionsOfMapAsync(map);
    }

    public async Task<DbLocalRecord[]> RecalculatePositionsOfMapAsync(IMap map)
    {
        var locals = await GetLocalRecordsOfMapByIdAsync(map.Id);
        var sorted = locals.OrderBy(r => r.DbRecord.Score).ToArray();
        var updated = new List<DbLocalRecord>();
        var remove = new List<DbLocalRecord>();

        var maxRecords = settings.MaxRecordsPerMap;

        for (var i = 0; i < sorted.Length; i++)
        {
            if (i > maxRecords)
            {
                remove.Add(sorted[i]);
                continue;
            }

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

            foreach (var toRemove in remove)
            {
                await Database.DeleteAsync(toRemove);
            }

            await transaction.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackTransactionAsync();
        }

        return updated.ToArray();
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

    public Task DeleteRecordsAsync(IMap map) => Table<DbLocalRecord>()
        .Where(r => r.MapId == map.Id)
        .DeleteAsync();

    private ILoadWithQueryable<DbLocalRecord, IPlayer> NewLoadAll() => Table<DbLocalRecord>()
        .LoadWith(r => r.DbMap)
        .LoadWith(r => r.DbRecord)
        .ThenLoad(r => r.DbPlayer);
}
