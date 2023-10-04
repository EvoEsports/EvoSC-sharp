using EvoSC.Common.Config.Models;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Util;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class MapService : IMapService
{
    private readonly IMapRepository _mapRepository;
    private readonly ILogger<MapService> _logger;
    private readonly IEvoScBaseConfig _config;
    private readonly IPlayerManagerService _playerService;
    private readonly IServerClient _serverClient;
    private readonly IMatchSettingsService _matchSettings;

    public MapService(IMapRepository mapRepository, ILogger<MapService> logger, IEvoScBaseConfig config,
        IPlayerManagerService playerService, IServerClient serverClient, IMatchSettingsService matchSettings)
    {
        _mapRepository = mapRepository;
        _logger = logger;
        _config = config;
        _playerService = playerService;
        _serverClient = serverClient;
        _matchSettings = matchSettings;
    }

    public async Task<IMap?> GetMapByIdAsync(long id) => await _mapRepository.GetMapByIdAsync(id);

    public async Task<IMap?> GetMapByUidAsync(string uid) => await _mapRepository.GetMapByUidAsync(uid);
    public async Task<IMap?> GetMapByExternalIdAsync(string id) => await _mapRepository.GetMapByExternalIdAsync(id);

    public async Task<IMap> AddMapAsync(MapStream mapStream)
    {
        var mapMetadata = mapStream.MapMetadata;
        var mapFile = mapStream.MapFile;

        IMap? existingMap = await GetMapByUidAsync(mapMetadata.MapUid);
        if (existingMap != null && MapVersionExistsInDb(existingMap, mapMetadata))
        {
            _logger.LogDebug("Map with UID {MapUid} already exists in database", mapMetadata.MapUid);
            throw new DuplicateMapException($"Map with UID {mapMetadata.MapUid} already exists in database");
        }

        var fileName = $"{mapMetadata.MapUid}.Map.Gbx";
        var filePath = Path.Combine(_config.Path.Maps, "EvoSC", fileName);
        var relativePath = Path.Combine("EvoSC", fileName);

        await SaveMapFileAsync(mapFile, filePath);

        var playerId = PlayerUtils.IsAccountId(mapMetadata.AuthorId)
            ? mapMetadata.AuthorId
            : PlayerUtils.ConvertLoginToAccountId(mapMetadata.AuthorId);

        var author = await _playerService.GetOrCreatePlayerAsync(playerId);

        IMap map;

        if (existingMap != null)
        {
            _logger.LogDebug("Updating map with ID {MapId} to the database", existingMap.Id);
            map = await _mapRepository.UpdateMapAsync(existingMap.Id, mapMetadata);
        }
        else
        {
            _logger.LogDebug("Adding map {Name} ({Uid}) to the database", mapMetadata.MapName, mapMetadata.MapUid);
            map = await _mapRepository.AddMapAsync(mapMetadata, author, relativePath);
        }

        await _matchSettings.EditMatchSettingsAsync(Path.GetFileNameWithoutExtension(_config.Path.DefaultMatchSettings),
            builder => builder.AddMap($"EvoSC/{fileName}"));
        
        return map;
    }

    public async Task<IEnumerable<IMap>> AddMapsAsync(List<MapStream> mapStreams)
    {
        var maps = new List<IMap>();
        foreach (var mapStream in mapStreams)
        {
            var map = await AddMapAsync(mapStream);
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

    public async Task<IMap> GetOrAddCurrentMapAsync()
    {
        var currentMap = await _serverClient.Remote.GetCurrentMapInfoAsync();
        var map = await GetMapByUidAsync(currentMap.UId);

        if (map != null)
        {
            return map;
        }

        var mapAuthor =
            await _playerService.GetOrCreatePlayerAsync(PlayerUtils.ConvertLoginToAccountId(currentMap.Author));

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

        return map;
    }

    public async Task<IMap?> GetNextMapAsync()
    {
        var nextMap = await _serverClient.Remote.GetNextMapInfoAsync();

        if (nextMap == null)
        {
            return null;
        }
        
        var map = await GetMapByUidAsync(nextMap.UId);

        return map;
    }

    public async Task<IMap?> GetCurrentMapAsync()
    {
        var currentMap = await _serverClient.Remote.GetCurrentMapInfoAsync();

        if (currentMap == null)
        {
            return null;
        }
        
        var map = await GetMapByUidAsync(currentMap.UId);

        return map;
    }

    private static bool MapVersionExistsInDb(IMap map, MapMetadata mapMetadata)
    {
        return map.ExternalVersion == mapMetadata.ExternalVersion;
    }

    private async Task SaveMapFileAsync(Stream mapStream, string filePath)
    {
        try
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            
            var fileStream = File.Create(filePath);
            await mapStream.CopyToAsync(fileStream);
            fileStream.Close();

            if (!File.Exists(filePath))
            {
                throw new InvalidOperationException("Map file creation failed. Got right permissions?");
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Failed saving the map file to storage");
            throw;
        }
    }
}
