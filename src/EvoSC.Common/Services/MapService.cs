using System.Data;
using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.DbAccess.Maps;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class MapService : IMapService
{
    private MapRepository _mapRepository;
    private ILogger<MapService> _logger;
    private IEvoScBaseConfig _config;
    private IPlayerManagerService _playerService;
    private IServerClient _serverClient;

    public MapService(MapRepository mapRepository, ILogger<MapService> logger, IEvoScBaseConfig config,
        IPlayerManagerService playerService, IServerClient serverClient)
    {
        _mapRepository = mapRepository;
        _logger = logger;
        _config = config;
        _playerService = playerService;
        _serverClient = serverClient;
    }
    
    public async Task<DbMap?> GetMapById(int id)
    {
        return await _mapRepository.GetMapById(id);
    }

    public async Task<DbMap?> GetMapByUid(string uid)
    {
        return await _mapRepository.GetMapByUid(uid);
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

        var author = await GetMapAuthor(map.AuthorId);
        
        if (existingMap != null)
        {
            dbMap = await _mapRepository.UpdateMap(existingMap.Id, map);
        }
        else
        {
            dbMap = await _mapRepository.AddMap(map, author, filePath);
        }

        await _serverClient.Remote.InsertMapAsync($"Downloaded/test69.Map.Gbx");

        return dbMap;
    }

    public async Task<IEnumerable<DbMap>> AddMaps(List<Map> maps)
    {
        throw new NotImplementedException();
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
