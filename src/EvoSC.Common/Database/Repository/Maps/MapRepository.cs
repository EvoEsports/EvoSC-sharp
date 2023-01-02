using System.Data.Common;
using Castle.Core.Logging;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;

using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Repository;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Maps;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Database.Repository.Maps;

public class MapRepository : IMapRepository
{
    private readonly ILogger<MapRepository> _logger;
    private readonly DbConnection _db;

    public MapRepository(ILogger<MapRepository> logger, DbConnection db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<IMap?> GetMapById(long id)
    {
        var query = "select * from `Maps` where `Id`=@MapId limit 1";
        return await _db.QueryFirstOrDefaultAsync<DbMap>(query, new
        {
            MapId = id
        });
    }

    public async Task<IMap?> GetMapByUid(string uid)
    {
        var query = "select * from `Maps` where `Uid`=@MapUid limit 1";
        return await _db.QueryFirstOrDefaultAsync<DbMap>(query, new
        {
            MapUid = uid
        });
    }
    
    public async Task<IMap> AddMap(MapMetadata mapMetadata, IPlayer author, string filePath)
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
        
        await using var transaction = await _db.BeginTransactionAsync();
        try
        {
            await _db.InsertAsync(dbMap, transaction: transaction);
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

    public async Task<IMap> UpdateMap(long mapId, MapMetadata mapMetadata)
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

        await using var transaction = await _db.BeginTransactionAsync();
        try
        {
            await _db.UpdateAsync(updatedMap, transaction: transaction);
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

    public async Task RemoveMap(long id)
    {
        var query = "delete from `Maps` where `Id`=@MapId";
        
        await using var transaction = await _db.BeginTransactionAsync();
        try
        {
            await _db.QueryFirstOrDefaultAsync<DbMap>(query, new { MapId = id }, transaction);
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed removing map with ID {id}.");
            await transaction.RollbackAsync();
        }
    }
}
