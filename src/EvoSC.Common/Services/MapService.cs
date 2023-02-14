using System.Data;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Repository.Maps;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
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

    public async Task<IMap?> GetMapByIdAsync(long id) => await _mapRepository.GetMapByIdAsync(id);

    public async Task<IMap?> GetMapByUidAsync(string uid) => await _mapRepository.GetMapByUidAsync(uid);

    public async Task<IMap> AddMapAsync(MapStream mapStream)
    {
        var mapMetadata = mapStream.MapMetadata;
        var mapFile = mapStream.MapFile;

        IMap? existingMap = await GetMapByUidAsync(mapMetadata.MapUid);
        if (existingMap != null && MapVersionExistsInDb(existingMap, mapMetadata))
        {
            // TODO: #79 Expand Map module with more accurate exceptions https://github.com/EvoTM/EvoSC-sharp/issues/79
            _logger.LogDebug("Map with UID {MapUid} already exists in database.", mapMetadata.MapUid);
            throw new DuplicateNameException($"Map with UID {mapMetadata.MapUid} already exists in database");
        }

        var fileName = $"{mapMetadata.MapName}.Map.Gbx";
        var filePath = Path.Combine(_config.Path.Maps, "/EvoSC");

        await SaveMapFileAsync(mapFile, filePath, fileName);

        var playerId = PlayerUtils.IsAccountId(mapMetadata.AuthorId)
            ? mapMetadata.AuthorId
            : PlayerUtils.ConvertLoginToAccountId(mapMetadata.AuthorId);

        var author = await GetMapAuthorAsync(playerId, mapMetadata.AuthorName);

        IMap map;

        if (existingMap != null)
        {
            try
            {
                _logger.LogDebug("Updating map with ID {MapId} to the database", existingMap.Id);
                map = await _mapRepository.UpdateMapAsync(existingMap.Id, mapMetadata);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Something went wrong while trying to update map with ID {MapId} to the database",
                    existingMap.Id);
                throw;
            }
        }
        else
        {
            try
            {
                _logger.LogDebug("Adding map to the database");
                map = await _mapRepository.AddMapAsync(mapMetadata, author, filePath);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, $"Something went wrong while trying to add a map to the database");
                throw;
            }
        }

        await _serverClient.Remote.InsertMapAsync($"EvoSC/{fileName}");

        return map;
    }

    public async Task<IEnumerable<IMap>> AddMapsAsync(List<MapStream> mapObjects)
    {
        var maps = new List<IMap>();
        foreach (var mapObject in mapObjects)
        {
            var map = await AddMapAsync(mapObject);
            maps.Add(map);
        }

        return maps;
    }

    public async Task RemoveMapAsync(long mapId)
    {
        await _mapRepository.RemoveMapAsync(mapId);
    }

    public async Task AddCurrentMapListAsync()
    {
        var serverMapList = await _serverClient.Remote.GetMapListAsync(-1, 0);

        foreach (var serverMap in serverMapList)
        {
            try
            {
                IMap? existingMap = await GetMapByUidAsync(serverMap.UId);

                if (existingMap != null)
                {
                    continue;
                }

                var authorAccountId = PlayerUtils.ConvertLoginToAccountId(serverMap.Author);
                var author = await _playerService.GetOrCreatePlayerAsync(authorAccountId);

                var mapMeta = new MapMetadata
                {
                    MapUid = serverMap.UId,
                    MapName = serverMap.Name,
                    AuthorId = serverMap.Author,
                    AuthorName = serverMap.AuthorNickname,
                    ExternalId = null,
                    ExternalVersion = null,
                    ExternalMapProvider = null
                };

                _logger.LogDebug("Adding map {Name} ({Uid}) to the database", serverMap.Name, serverMap.UId);
                await _mapRepository.AddMapAsync(mapMeta, author, serverMap.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add current map to the database: {Name} ({Uid})", 
                    serverMap.Name,
                    serverMap.UId);
            }
        }
    }

    private static bool MapVersionExistsInDb(IMap map, MapMetadata mapMetadata)
    {
        return map.ExternalVersion == mapMetadata.ExternalVersion;
    }

    private async Task SaveMapFileAsync(Stream mapStream, string filePath, string fileName)
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
            _logger.LogWarning(e, "Failed saving the map file to storage");
            throw;
        }
    }

    private async Task<IPlayer> GetMapAuthorAsync(string authorId, string? name)
    {
        var dbPlayer = await _playerService.GetPlayerAsync(authorId);

        if (dbPlayer == null)
        {
            return await _playerService.CreatePlayerAsync(authorId, name);
        }

        return dbPlayer;
    }
    
    public async Task<IMap> GetOrAddCurrentMapAsync()
    {
        var currentMap = await _serverClient.Remote.GetCurrentMapInfoAsync();
        var map = await GetMapByUidAsync(currentMap.UId);
        
        if (map == null)
        {
            var mapAuthor = await _playerService.GetOrCreatePlayerAsync(PlayerUtils.ConvertLoginToAccountId(currentMap.Author));

            var mapMeta = new MapMetadata
            {
                MapUid = currentMap.UId,
                MapName = currentMap.Name,
                AuthorId = mapAuthor.AccountId,
                AuthorName = mapAuthor.NickName,
                ExternalId = currentMap.UId,
                ExternalVersion = null,
                ExternalMapProvider = null
            };

            map = await _mapRepository.AddMapAsync(mapMeta, mapAuthor, currentMap.FileName);
        }

        return map;
    }
}
