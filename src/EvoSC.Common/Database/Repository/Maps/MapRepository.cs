using System.Data.Common;
using Castle.Core.Logging;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Repository;
using EvoSC.Common.Models;
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

    public async Task<DbMap?> GetMapById(long id)
    {
        var query = "select * from `Maps` where `Id`=@MapId limit 1";
        return await _db.QueryFirstOrDefaultAsync<DbMap>(query, new
        {
            MapId = id
        });
    }

    public async Task<DbMap?> GetMapByUid(string uid)
    {
        var query = "select * from `Maps` where `Uid`=@MapUid limit 1";
        return await _db.QueryFirstOrDefaultAsync<DbMap>(query, new
        {
            MapUid = uid
        });
    }

    public async Task<DbMap> AddMap(Map map, DbPlayer author, DbPlayer actor, string filePath)
    {
        var dbMap = new DbMap
        {
            Uid = map.Uid,
            Author = author.Id,
            FilePath = filePath,
            Enabled = true,
            Name = map.Name,
            ManiaExchangeId = map.MxId == 0 ? null : map.MxId,
            ManiaExchangeVersion = map.MxVersion,
            TrackmaniaIoId = map.TmIoId == 0 ? null : map.TmIoId,
            TrackmaniaIoVersion = map.TmIoVersion,
            AddedBy = actor.Id,
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

    public async Task<DbMap> UpdateMap(long mapId, Map map)
    {
        var updatedMap = new DbMap
        {
            Id = mapId,
            Uid = map.Uid,
            Enabled = true,
            Name = map.Name,
            ManiaExchangeId = map.MxId,
            ManiaExchangeVersion = map.MxVersion,
            TrackmaniaIoId = map.TmIoId,
            TrackmaniaIoVersion = map.TmIoVersion,
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

        return updatedMap;
    }

    public Task RemoveMap(Map map)
    {
        throw new NotImplementedException();
    }
}
