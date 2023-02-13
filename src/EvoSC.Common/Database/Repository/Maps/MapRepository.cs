using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;
using LinqToDB;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Database.Repository.Maps;

public class MapRepository : DbRepository, IMapRepository
{
    private readonly ILogger<MapRepository> _logger;
    
    public MapRepository(IDbConnectionFactory dbConnFactory, ILogger<MapRepository> logger) : base(dbConnFactory)
    {
        _logger = logger;
    }

    public async Task<IMap?> GetMapByIdAsync(long id) => await Table<DbMap>()
        .LoadWith(t => t.DbAuthor)
        .SingleOrDefaultAsync(m => m.Id == id);

    public async Task<IMap?> GetMapByUidAsync(string uid) => await Table<DbMap>()
        .LoadWith(t => t.DbAuthor)
        .SingleOrDefaultAsync(m => m.Uid == uid);

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
            await transaction.CommitAsync();
            dbMap.Id = Convert.ToInt64(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add map");
            await transaction.RollbackAsync();
            throw;
        }

        return dbMap;
    }

    public async Task<IMap> UpdateMapAsync(long mapId, MapMetadata map)
    {
        var updatedMap = new DbMap
        {
            Id = mapId,
            Uid = map.MapUid,
            Enabled = true,
            Name = map.MapName,
            ExternalId = map.ExternalId,
            ExternalVersion = map.ExternalVersion,
            ExternalMapProvider = map.ExternalMapProvider,
            UpdatedAt = DateTime.Now
        };

        await using var transaction = await Database.BeginTransactionAsync();
        try
        {
            await Table<DbMap>()
                .Where(m => m.Id == updatedMap.Id)
                .Set(m => m.Uid, updatedMap.Uid)
                .Set(m => m.Enabled, updatedMap.Enabled)
                .Set(m => m.Name, updatedMap.Name)
                .Set(m => m.ExternalId, updatedMap.ExternalId)
                .Set(m => m.ExternalVersion, updatedMap.ExternalVersion)
                .Set(m => m.ExternalMapProvider, updatedMap.ExternalMapProvider)
                .Set(m => m.UpdatedAt, updatedMap.UpdatedAt)
                .UpdateAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to update map");
            await transaction.RollbackAsync();
            throw;
        }

        return new Map(updatedMap);
    }

    public Task RemoveMapAsync(long id) => 
        Table<DbMap>().DeleteAsync(m => m.Id == id);

}

