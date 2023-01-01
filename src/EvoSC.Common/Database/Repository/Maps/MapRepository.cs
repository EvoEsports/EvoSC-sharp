using System.Data;
using System.Data.Common;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Repository;
using EvoSC.Common.Models.Maps;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using RepoDb;
using RepoDb.Interfaces;

namespace EvoSC.Common.Database.Repository.Maps;

public class MapRepository : DbRepository<SqlConnection>, IMapRepository
{
    private readonly ILogger<MapRepository> _logger;
    private readonly DbConnection _db;

    private IDbSetting _dbSetting;
    private IEnumerable<Field> _fields;

    public MapRepository(ILogger<MapRepository> logger, DbConnection db) : base(db.ConnectionString)
    {
        _logger = logger;
        _db = db;
        _dbSetting = DbSettingMapper.Get(_db);
        _fields = FieldCache.Get<DbMap>();
    }

    public async Task<IMap?> GetMapByIdAsync(long id)
    {
        var where = new QueryGroup(new QueryField("Id", id));
        var statement = new QueryBuilder()
            .Clear()
            .Select()
            .FieldsFrom(_fields, _dbSetting)
            .From()
            .TableNameFrom("Maps", _dbSetting)
            .WhereFrom(where, _dbSetting)
            .End()
            .GetString();
        
        return await _db.ExecuteQueryAsync(statement).Result.FirstOrDefault();
    }

    public async Task<IMap?> GetMapByUidAsync(string uid)
    {
        var where = new QueryGroup(new QueryField("Uid", uid));
        var statement = new QueryBuilder()
            .Clear()
            .Select()
            .FieldsFrom(_fields, _dbSetting)
            .From()
            .TableNameFrom("Maps", _dbSetting)
            .WhereFrom(where, _dbSetting)
            .End()
            .GetString();
        
        return await _db.ExecuteQueryAsync(statement).Result.FirstOrDefault();
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

    public async Task RemoveMapAsync(long id) =>
        await _db.DeleteAsync("Maps", id);
}
