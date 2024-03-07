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

public class MapService(IMapRepository mapRepository, ILogger<MapService> logger, IEvoScBaseConfig config,
        IPlayerManagerService playerService, IServerClient serverClient)
    : IMapService
{
    public async Task<IMap?> GetMapByIdAsync(long id) => await mapRepository.GetMapByIdAsync(id);

    public async Task<IMap?> GetMapByUidAsync(string uid) => await mapRepository.GetMapByUidAsync(uid);
    public async Task<IMap?> GetMapByExternalIdAsync(string id) => await mapRepository.GetMapByExternalIdAsync(id);

    public async Task<IMap> AddMapAsync(MapStream mapStream)
    {
        var mapMetadata = mapStream.MapMetadata;
        var mapFile = mapStream.MapFile;

        IMap? existingMap = await GetMapByUidAsync(mapMetadata.MapUid);
        if (existingMap != null && MapVersionExistsInDb(existingMap, mapMetadata))
        {
            logger.LogDebug("Map with UID {MapUid} already exists in database", mapMetadata.MapUid);
            throw new DuplicateMapException($"Map with UID {mapMetadata.MapUid} already exists in database");
        }

        var fileName = $"{mapMetadata.MapUid}.Map.Gbx";
        var filePath = Path.Combine(config.Path.Maps, "EvoSC", fileName);
        var relativePath = Path.Combine("EvoSC", fileName);

        await SaveMapFileAsync(mapFile, filePath);

        var playerId = PlayerUtils.IsAccountId(mapMetadata.AuthorId)
            ? mapMetadata.AuthorId
            : PlayerUtils.ConvertLoginToAccountId(mapMetadata.AuthorId);

        var author = await playerService.GetOrCreatePlayerAsync(playerId);

        IMap map;

        if (existingMap != null)
        {
            logger.LogDebug("Updating map with ID {MapId} to the database", existingMap.Id);
            map = await mapRepository.UpdateMapAsync(existingMap.Id, mapMetadata);
        }
        else
        {
            logger.LogDebug("Adding map {Name} ({Uid}) to the database", mapMetadata.MapName, mapMetadata.MapUid);
            map = await mapRepository.AddMapAsync(mapMetadata, author, relativePath);
        }

        /* await matchSettings.EditMatchSettingsAsync(Path.GetFileNameWithoutExtension(config.Path.DefaultMatchSettings),
            builder => builder.AddMap($"EvoSC/{fileName}")); */
        
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
        await mapRepository.RemoveMapAsync(mapId);
    }

    public async Task AddCurrentMapListAsync()
    {
        var serverMapList = await serverClient.Remote.GetMapListAsync(-1, 0);

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
                var author = await playerService.GetOrCreatePlayerAsync(authorAccountId);

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

                logger.LogDebug("Adding map {Name} ({Uid}) to the database", serverMap.Name, serverMap.UId);
                await mapRepository.AddMapAsync(mapMeta, author, serverMap.FileName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to add current map to the database: {Name} ({Uid})",
                    serverMap.Name,
                    serverMap.UId);
            }
        }
    }

    public async Task<IMap> GetOrAddCurrentMapAsync()
    {
        var currentMap = await serverClient.Remote.GetCurrentMapInfoAsync();
        var map = await GetMapByUidAsync(currentMap.UId);

        if (map != null)
        {
            return map;
        }

        var mapAuthor =
            await playerService.GetOrCreatePlayerAsync(PlayerUtils.ConvertLoginToAccountId(currentMap.Author));

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

        map = await mapRepository.AddMapAsync(mapMeta, mapAuthor, currentMap.FileName);

        return map;
    }

    public async Task<IMap?> GetNextMapAsync()
    {
        var nextMap = await serverClient.Remote.GetNextMapInfoAsync();

        if (nextMap == null)
        {
            return null;
        }
        
        var map = await GetMapByUidAsync(nextMap.UId);

        return map;
    }

    public async Task<IMap?> GetCurrentMapAsync()
    {
        var currentMap = await serverClient.Remote.GetCurrentMapInfoAsync();

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
            logger.LogWarning(e, "Failed saving the map file to storage");
            throw;
        }
    }
}
