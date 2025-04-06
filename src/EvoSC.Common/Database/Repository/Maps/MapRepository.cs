﻿using System.Globalization;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Exceptions.DatabaseExceptions;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;
using LinqToDB;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Database.Repository.Maps;

public class MapRepository(IDbConnectionFactory dbConnFactory, ILogger<MapRepository> logger)
    : DbRepository(dbConnFactory), IMapRepository
{
    public async Task<IMap?> GetMapByIdAsync(long id) => await Table<DbMap>()
        .LoadWith(t => t.DbAuthor)
        .LoadWith(t => t.DbDetails)
        .SingleOrDefaultAsync(m => m.Id == id);

    public async Task<IMap[]> GetMapsAsync() => await Table<DbMap>()
        .LoadWith(m => m.DbAuthor)
        .LoadWith(m => m.DbDetails)
        .ToArrayAsync();

    public async Task<IMap?> GetMapByUidAsync(string uid) => await Table<DbMap>()
        .LoadWith(t => t.DbAuthor)
        .LoadWith(t => t.DbDetails)
        .SingleOrDefaultAsync(m => m.Uid == uid);

    public async Task<IMap?> GetMapByExternalIdAsync(string id) => await Table<DbMap>()
        .LoadWith(t => t.DbAuthor)
        .LoadWith(t => t.DbDetails)
        .SingleOrDefaultAsync(m => m.ExternalId == id);

    public async Task<IEnumerable<IMap>> GetMapsByUidAsync(IEnumerable<string> mapUids) => await Table<DbMap>()
        .LoadWith(t => t.DbAuthor)
        .LoadWith(t => t.DbDetails)
        .Where(m => mapUids.Contains(m.Uid))
        .ToArrayAsync();

    public async Task<IMap> AddMapAsync(MapMetadata map, IPlayer author, string filePath)
    {
        var dbMap = new DbMap
        {
            Uid = map.MapUid,
            DbAuthor = new DbPlayer(author),
            AuthorId = author.Id,
            FilePath = filePath,
            Enabled = true,
            Name = map.MapName,
            ExternalId = map.ExternalId,
            ExternalVersion = map.ExternalVersion,
            ExternalMapProvider = map.ExternalMapProvider,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await using var transaction = await Database.BeginTransactionAsync();
        try
        {
            var id = await Database.InsertWithIdentityAsync(dbMap);
            await transaction.CommitTransactionAsync();
            dbMap.Id = Convert.ToInt64(id, CultureInfo.InvariantCulture);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed adding map with UID {MapMapUid} to the database", map.MapUid);
            await transaction.RollbackTransactionAsync();
            throw new EvoScDatabaseException($"Failed adding map with UID {map.MapUid} to the database", e);
        }

        return dbMap;
    }

    public async Task<IMapDetails> AddMapDetailsAsync(IMapDetails mapDetails, IMap map)
    {
        var dbMapDetails = new DbMapDetails
        {
            AuthorTime = mapDetails.AuthorTime,
            GoldTime = mapDetails.GoldTime,
            SilverTime = mapDetails.SilverTime,
            BronzeTime = mapDetails.BronzeTime,
            MapId = (int)map.Id,
            Environment = mapDetails.Environment,
            Mood = mapDetails.Mood,
            Cost = mapDetails.Cost,
            MultiLap = mapDetails.MultiLap,
            LapCount = mapDetails.LapCount,
            MapStyle = mapDetails.MapStyle,
            MapType = mapDetails.MapType,
            CheckpointCount = mapDetails.CheckpointCount,
            DbMap = new DbMap(map)
        };

        await using var transaction = await Database.BeginTransactionAsync();

        try
        {
            await Database.InsertAsync(dbMapDetails);
            await transaction.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed adding map details for map ID {MapId}", map.Id);
            await transaction.RollbackTransactionAsync();
            throw new EvoScDatabaseException($"Failed adding map details for map ID {map.Id}");
        }

        return dbMapDetails;
    }

    public async Task<IMap> UpdateMapAsync(DbMap map)
    {
        await using var transaction = await Database.BeginTransactionAsync();
        
        try
        {
            await Database.UpdateAsync(map);
            await transaction.CommitTransactionAsync();
            
            return new Map(map);
        }
        catch (Exception ex)
        {
            await transaction.RollbackTransactionAsync();
            
            logger.LogError(ex, "Failed to update map with UID {MapMapUid}", map.Uid);
            throw new EvoScDatabaseException($"Failed to update map with UID {map.Uid}", ex);
        }
    }

    public Task RemoveMapAsync(long id) => 
        Table<DbMap>().DeleteAsync(m => m.Id == id);
}

