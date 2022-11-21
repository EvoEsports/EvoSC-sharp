using System.Data;
using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class MapService : IMapService
{
    private DbConnection _db;
    private ILogger<MapService> _logger;
    private IEvoScBaseConfig _config;
    private IPlayerManagerService _playerService;
    private IServerClient _serverClient;

    public MapService(DbConnection db, ILogger<MapService> logger, IEvoScBaseConfig config,
        IPlayerManagerService playerService, IServerClient serverClient)
    {
        _db = db;
        _logger = logger;
        _config = config;
        _playerService = playerService;
        _serverClient = serverClient;
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
    
    public async Task<DbMap> AddMap(Stream mapStream, Map map)
    {
        var existingMap = await GetMapByUid(map.Uid);
        if (existingMap != null && MapVersionExistsInDb(existingMap, map))
        {
            // TODO: Change this with a more precise exception
            throw new DuplicateNameException("Map already exists in database");
        } 
        
        var filePath = _config.Path.Maps + $"/{map.Name}.Map.Gbx";

        await SaveMapFile(mapStream, filePath);

        DbMap dbMap;
        
        if (existingMap != null)
        {
            dbMap = await UpdateMap(map);
        }
        else
        {
            dbMap = await SaveNewMapToDb(map, filePath);
        }

        await _serverClient.Remote.InsertMapAsync($"Downloaded/test69.Map.Gbx");

        return dbMap;
    }

    public async Task<IEnumerable<DbMap>> AddMaps(List<Map> maps)
    {
        throw new NotImplementedException();
    }

    public async Task<DbMap> UpdateMap(Map map)
    {
        var dbMap = await GetMapByUid(map.Uid);

        var updatedMap = new DbMap
        {
            Id = dbMap.Id,
            Uid = dbMap.Uid,
            FilePath = dbMap.FilePath,
            Enabled = true,
            Name = map.Name,
            ManiaExchangeId = map.MxId,
            ManiaExchangeVersion = map.MxVersion,
            TrackmaniaIoId = map.TmIoId,
            TrackmaniaIoVersion = map.TmIoVersion,
            CreatedAt = dbMap.CreatedAt,
            UpdatedAt = DateTime.Now
        };

        await _db.InsertAsync(updatedMap);
        return updatedMap;
    }

    public async Task RemoveMap(string mapUid)
    {
        throw new NotImplementedException();
    }

    private static bool MapVersionExistsInDb(DbMap dbMap, Map map)
    {
        return dbMap.ManiaExchangeVersion == map.MxVersion || dbMap.TrackmaniaIoVersion == map.TmIoVersion;
    }

    private async Task SaveMapFile(Stream mapStream, string filePath)
    {
        try
        {
            var fileStream = File.Create(filePath);
            await mapStream.CopyToAsync(fileStream);
            fileStream.Close();
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Failed saving the map file to storage");
            throw;
        }
    }

    private async Task<DbMap> SaveNewMapToDb(Map map, string filePath)
    {
        var dbPlayer = await GetMapAuthor(map.AuthorId);
        var dbMap = new DbMap
        {
            Uid = map.Uid,
            Author = dbPlayer.Id,
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

    private async Task<IPlayer> GetMapAuthor(string authorId)
    {
        var dbPlayer = await _playerService.GetPlayerAsync(authorId);

        if (dbPlayer == null)
        {
            return await _playerService.CreatePlayerAsync(authorId);
        }

        return dbPlayer;
    }
}
