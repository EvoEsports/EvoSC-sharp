using System.Data;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Repository.Maps;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class MapService : IMapService
{
    private readonly MapRepository _mapRepository;
    private readonly ILogger<MapService> _logger;
    private readonly IEvoScBaseConfig _config;
    private readonly IPlayerManagerService _playerService;
    private readonly IServerClient _serverClient;

    public MapService(MapRepository mapRepository, ILogger<MapService> logger, IEvoScBaseConfig config,
        IPlayerManagerService playerService, IServerClient serverClient)
    {
        _mapRepository = mapRepository;
        _logger = logger;
        _config = config;
        _playerService = playerService;
        _serverClient = serverClient;
    }

    public async Task<DbMap?> GetMapById(int id) => await _mapRepository.GetMapById(id);

    public async Task<DbMap?> GetMapByUid(string uid) => await _mapRepository.GetMapByUid(uid);

    public async Task<Map> AddMap(MapObject mapObject, IPlayer actor)
    {
        var map = mapObject.Map;
        var mapStream = mapObject.MapStream;

        DbMap? existingMap = await GetMapByUid(map.Uid);
        if (existingMap != null && MapVersionExistsInDb(existingMap, map))
        {
            // TODO: Change this with a more precise exception
            _logger.LogDebug($"Map with UID {map.Uid} already exists in database.");
            throw new DuplicateNameException($"Map with UID {map.Uid} already exists in database");
        }

        var fileName = $"{map.Name}.Map.Gbx";
        var filePath = _config.Path.Maps + $"/EvoSC";

        await SaveMapFile(mapStream, filePath, fileName);
        
        var author = await GetMapAuthor(map.AuthorId);
        
        if (existingMap != null)
        {
            try
            {
                _logger.LogDebug($"Updating map with ID {existingMap.Id} to the database.");
                await _mapRepository.UpdateMap(existingMap.Id, map);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, $"Something went wrong while trying to update " +
                                      $"map with ID {existingMap.Id} to the database.");
                throw;
            }
        }
        else
        {
            try
            {
                _logger.LogDebug($"Adding map to the database.");
                await _mapRepository.AddMap(map, author, actor, filePath);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, $"Something went wrong while trying to add a map" +
                                   $" to the database.");
                throw;
            }
        }

        await _serverClient.Remote.InsertMapAsync($"EvoSC/{fileName}");

        return map;
    }

    public async Task<IEnumerable<Map>> AddMaps(List<MapObject> mapObjects, IPlayer actor)
    {
        var maps = new List<Map>();
        foreach (var mapObject in mapObjects)
        {
            var map = await AddMap(mapObject, actor);
            maps.Add(map);
        }

        return maps;
    }

    public async Task RemoveMap(string mapUid)
    {
        throw new NotImplementedException();
    }

    private static bool MapVersionExistsInDb(DbMap dbMap, Map map)
    {
        return dbMap.ManiaExchangeVersion == map.MxVersion || dbMap.TrackmaniaIoVersion == map.TmIoVersion;
    }

    private async Task SaveMapFile(Stream mapStream, string filePath, string fileName)
    {
        try
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            
            var fileStream = File.Create(filePath + $"/{fileName}");
            await mapStream.CopyToAsync(fileStream);
            fileStream.Close();
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Failed saving the map file to storage.");
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
