using System.Data;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Repository.Maps;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Util;
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

    public async Task<IMap?> GetMapById(long id) => await _mapRepository.GetMapById(id);

    public async Task<IMap?> GetMapByUid(string uid) => await _mapRepository.GetMapByUid(uid);

    public async Task<IMap> AddMap(MapStream mapStream)
    {
        var mapMetadata = mapStream.MapMetadata;
        var mapFile = mapStream.MapFile;

        IMap? existingMap = await GetMapByUid(mapMetadata.MapUid);
        if (existingMap != null && MapVersionExistsInDb(existingMap, mapMetadata))
        {
            // TODO: Change this with a more precise exception
            _logger.LogDebug("Map with UID {MapUid} already exists in database.", mapMetadata.MapUid);
            throw new DuplicateNameException($"Map with UID {mapMetadata.MapUid} already exists in database");
        }

        var fileName = $"{mapMetadata.MapName}.Map.Gbx";
        var filePath = Path.Combine(_config.Path.Maps, "/EvoSC");

        await SaveMapFile(mapFile, filePath, fileName);

        var author = await GetMapAuthor(PlayerUtils.IsAccountId(mapMetadata.AuthorId)
            ? mapMetadata.AuthorId
            : PlayerUtils.ConvertLoginToAccountId(mapMetadata.AuthorId));

        IMap map;

        if (existingMap != null)
        {
            try
            {
                _logger.LogDebug("Updating map with ID {MapId} to the database.", existingMap.Id);
                map = await _mapRepository.UpdateMap(existingMap.Id, mapMetadata);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Something went wrong while trying to update " +
                                      "map with ID {MapId} to the database.", existingMap.Id);
                throw;
            }
        }
        else
        {
            try
            {
                _logger.LogDebug($"Adding map to the database.");
                map = await _mapRepository.AddMap(mapMetadata, author, filePath);
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

    public async Task<IEnumerable<IMap>> AddMaps(List<MapStream> mapObjects)
    {
        var maps = new List<IMap>();
        foreach (var mapObject in mapObjects)
        {
            var map = await AddMap(mapObject);
            maps.Add(map);
        }

        return maps;
    }

    public async Task RemoveMap(long mapId)
    {
        await _mapRepository.RemoveMap(mapId);
    }

    private static bool MapVersionExistsInDb(IMap map, MapMetadata mapMetadata)
    {
        return map.ExternalVersion == mapMetadata.ExternalVersion;
    }

    private async Task SaveMapFile(Stream mapStream, string filePath, string fileName)
    {
        try
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var fileStream = File.Create(Path.Combine(filePath, $"/{fileName}"));
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
