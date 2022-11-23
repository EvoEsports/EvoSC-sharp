using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Repository;
using EvoSC.Common.Models;

namespace EvoSC.Common.Database.DbAccess.Maps;

public class MapRepository : IMapRepository
{
    private readonly DbConnection _db;

    public MapRepository(DbConnection db)
    {
        _db = db;
    }

    public async Task<DbMap?> GetMapById(int id)
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

    public async Task<DbMap> AddMap(Map map, DbPlayer author, string filePath)
    {
        var dbMap = new DbMap
        {
            Uid = map.Uid,
            Author = author.Id,
            FilePath = filePath,
            Enabled = true,
            Name = map.Name,
            ManiaExchangeId = map.MxId,
            ManiaExchangeVersion = map.MxVersion,
            TrackmaniaIoId = map.TmIoId,
            TrackmaniaIoVersion = map.TmIoVersion,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        
        await _db.InsertAsync(dbMap);
        return dbMap;
    }

    public async Task<DbMap> UpdateMap(int mapId, Map map)
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

        await _db.UpdateAsync(updatedMap);
        return updatedMap;
    }

    public Task RemoveMap(Map map)
    {
        throw new NotImplementedException();
    }
}
