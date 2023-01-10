using System.Data;
using System.Data.Common;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using RepoDb;
using RepoDb.Interfaces;

namespace EvoSC.Common.Database.Repository.Maps;

public class MapRepository : EvoScDbRepository<DbMap>, IMapRepository
{
    private readonly ILogger<MapRepository> _logger;
    public MapRepository(ILogger<MapRepository> logger, IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
        _logger = logger;
    }

    public async Task<IMap?> GetMapByIdAsync(long id)
    {
        var where = new QueryGroup(new QueryField("Id", id));
        var statement = new QueryBuilder()
            .Clear()
            .Select()
            .FieldsFrom(FieldCache.Get<DbMap>(), DatabaseSetting)
            .From()
            .TableNameFrom("Maps", DatabaseSetting)
            .WhereFrom(where, DatabaseSetting)
            .End()
            .GetString();
    
        var maps = await Database.QueryAsync<DbMap>(e => e.Id == id);
        return maps.FirstOrDefault();
    }

    public async Task<IMap?> GetMapByUidAsync(string uid)
    {
        var where = new QueryGroup(new QueryField("Uid", uid));
        var statement = new QueryBuilder()
            .Clear()
            .Select()
            .FieldsFrom(Fields, DatabaseSetting)
            .From()
            .TableNameFrom("Maps", DatabaseSetting)
            .WhereFrom(where, DatabaseSetting)
            .End()
            .GetString();

        var maps = await Database.QueryAsync<DbMap>(e => e.Uid == uid);
        return maps.FirstOrDefault();
    }
    
    public async Task<IMap> AddMapAsync(MapMetadata mapMetadata, IPlayer author, string filePath)
    {
        var dbMap = new DbMap
        {
            Uid = mapMetadata.MapUid,
            Author = author,
            AuthorId = author.Id,
            FilePath = filePath,
            Enabled = true,
            Name = mapMetadata.MapName,
            ExternalId = mapMetadata.ExternalId,
            ExternalVersion = mapMetadata.ExternalVersion,
            ExternalMapProvider = mapMetadata.ExternalMapProvider,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        
        await using var transaction = await Database.BeginTransactionAsync();
        try
        {
            await Database.InsertAsync(dbMap, transaction: transaction);
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogDebug(e, "Failed to add map.");
            await transaction.RollbackAsync();
            throw;
        }
        
        return dbMap;
    }

    public async Task<IMap> UpdateMapAsync(long mapId, MapMetadata mapMetadata)
    {
        var updatedMap = new DbMap
        {
            Id = mapId,
            Uid = mapMetadata.MapUid,
            Enabled = true,
            Name = mapMetadata.MapName,
            ExternalVersion = mapMetadata.ExternalVersion,
            UpdatedAt = DateTime.Now
        };

        await using var transaction = await Database.BeginTransactionAsync();
        try
        {
            await Database.UpdateAsync(updatedMap, transaction: transaction);
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogDebug(e, "Failed to update map.");
            await transaction.RollbackAsync();
            throw;
        }

        return new Map(updatedMap) ;
    }

    public async Task RemoveMapAsync(long id) =>
        await Database.DeleteAsync("Maps", id);
}
