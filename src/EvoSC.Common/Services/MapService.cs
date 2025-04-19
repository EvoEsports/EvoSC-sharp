using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Events.Arguments;
using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Util;
using GBX.NET;
using GBX.NET.Engines.Game;
using GbxRemoteNet;
using GbxRemoteNet.Structs;
using GbxRemoteNet.XmlRpc.ExtraTypes;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class MapService(IMapRepository mapRepository, ILogger<MapService> logger, IEvoScBaseConfig config,
        IPlayerManagerService playerService, IServerClient serverClient, IEventManager events)
    : IMapService
{
    public async Task<IMap?> GetMapByIdAsync(long id) => await mapRepository.GetMapByIdAsync(id);

    public async Task<IMap?> GetMapByUidAsync(string uid) => await mapRepository.GetMapByUidAsync(uid);

    public Task<IEnumerable<IMap>> GetMapsByUidAsync(IEnumerable<string> mapUids) =>
        mapRepository.GetMapsByUidAsync(mapUids);

    public async Task<IMap?> GetMapByExternalIdAsync(string id) => await mapRepository.GetMapByExternalIdAsync(id);

    public async Task<IMap> AddMapAsync(MapStream mapStream)
    {
        var mapFileName = $"{mapStream.MapMetadata.MapUid}.Map.Gbx";
        var mapFullPath = Path.Combine(config.Path.Maps, "EvoSC", mapFileName);
        var mapRelativePath = Path.Combine("EvoSC", mapFileName);

        await SaveMapFileAsync(mapStream.MapFile, mapFullPath);
        
        var existingMap = await GetMapByUidAsync(mapStream.MapMetadata.MapUid);
        if (existingMap != null)
        {
            var updatedMap = await mapRepository.UpdateMapAsync(new DbMap(existingMap)
            {
                Id = 0,
                Uid = mapStream.MapMetadata.MapUid,
                FilePath = mapRelativePath,
                Name = mapStream.MapMetadata.MapName,
                ExternalId = mapStream.MapMetadata.ExternalId,
                ExternalVersion = mapStream.MapMetadata.ExternalVersion,
                ExternalMapProvider = mapStream.MapMetadata.ExternalMapProvider,
                UpdatedAt = DateTime.Now
            });
            
            await events.RaiseAsync(MapEvent.MapUpdated, new MapUpdatedEventArgs { Map = updatedMap, OldMap = existingMap });
            
            return updatedMap;
        }
        
        var authorAccountId = PlayerUtils.IsAccountId(mapStream.MapMetadata.AuthorId)
            ? mapStream.MapMetadata.AuthorId
            : PlayerUtils.ConvertLoginToAccountId(mapStream.MapMetadata.AuthorId);
        
        var author = await playerService.GetOrCreatePlayerAsync(authorAccountId, mapStream.MapMetadata.AuthorName);
        var newMap = await mapRepository.AddMapAsync(mapStream.MapMetadata, author, mapRelativePath);
        var mapDetails = await FetchMapDetailsAsync(newMap);
        
        await mapRepository.AddMapDetailsAsync(mapDetails, newMap);
        await events.RaiseAsync(MapEvent.MapAdded, new MapEventArgs { Map = newMap });
        
        return newMap;
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
        var map = await GetMapByIdAsync(mapId);
        await mapRepository.RemoveMapAsync(mapId);

        await events.RaiseAsync(MapEvent.MapRemoved, new MapEventArgs { Map = map });
    }

    public async Task AddCurrentMapListAsync()
    {
        var maplist = await GetRemoteMapListAsync();

        foreach (var serverMap in maplist)
        {
            try
            {
                IMap? existingMap = await GetMapByUidAsync(serverMap.UId);

                if (existingMap != null)
                {
                    continue;
                }
                
                var authorAccountId = PlayerUtils.ConvertLoginToAccountId(serverMap.Author);
                var author = await playerService.GetOrCreatePlayerAsync(authorAccountId, serverMap.AuthorNickname);

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
                var map = await mapRepository.AddMapAsync(mapMeta, author, serverMap.FileName);

                var mapDetails = new ParsedMap(serverMap, map);
                await mapRepository.AddMapDetailsAsync(mapDetails, map);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to add current map to the database: {Name} ({Uid})",
                    serverMap.Name,
                    serverMap.UId);
            }
        }
    }

    public async Task<IMap> AddLocalMapAsync(string filePath)
    {
        var mapNode = GameBox.Parse<CGameCtnChallenge>(filePath);
        var authorAccountId = PlayerUtils.ConvertLoginToAccountId(mapNode.Node.AuthorLogin);
        var author = await playerService.GetOrCreatePlayerAsync(authorAccountId, mapNode.Node.AuthorNickname);
        
        var mapMeta = new MapMetadata
        {
            MapUid = mapNode.Node.MapUid,
            MapName = mapNode.Node.MapName,
            AuthorId = mapNode.Node.AuthorLogin,
            AuthorName = mapNode.Node.AuthorNickname ?? mapNode.Node.AuthorLogin,
            ExternalId = null,
            ExternalVersion = null,
            ExternalMapProvider = null
        };
        
        var map = await mapRepository.AddMapAsync(mapMeta, author, filePath);
        
        var mapDetails = new ParsedMap(new TmMapInfo
        {
            UId = mapNode.Node.MapUid,
            Name = mapNode.Node.MapName,
            FileName = filePath,
            Author = mapNode.Node.AuthorLogin,
            AuthorNickname = mapNode.Node.AuthorNickname,
            BronzeTime = mapNode.Node.TMObjective_BronzeTime?.TotalMilliseconds ?? 0,
            SilverTime = mapNode.Node.TMObjective_SilverTime?.TotalMilliseconds ?? 0,
            GoldTime = mapNode.Node.TMObjective_GoldTime?.TotalMilliseconds ?? 0,
            AuthorTime = mapNode.Node.TMObjective_AuthorTime?.TotalMilliseconds ?? 0,
            CopperPrice = mapNode.Node.Cost ?? 0,
            LapRace = mapNode.Node.TMObjective_IsLapRace ?? false,
            NbLaps = mapNode.Node.TMObjective_NbLaps ?? 0,
            NbCheckpoints = mapNode.Node.NbCheckpoints ?? 0,
            MapType = mapNode.Node.MapType,
            MapStyle = mapNode.Node.MapStyle
        }, map);
        
        await mapRepository.AddMapDetailsAsync(mapDetails, map);
        return map;
    }

    public async Task<IMap> GetOrAddCurrentMapAsync()
    {
        var currentMap = await serverClient.Remote.GetCurrentMapInfoAsync();
        var map = await GetMapByUidAsync(currentMap.UId);

        if (map != null)
        {
            return map;
        }

        var authorAccountId = PlayerUtils.ConvertLoginToAccountId(currentMap.Author);
        var mapAuthor = await playerService.GetOrCreatePlayerAsync(authorAccountId, currentMap.AuthorNickname);

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

    public async Task<IMapDetails> FetchMapDetailsAsync(IMap map)
    {
        try
        {
            var serverMap = await serverClient.Remote.GetMapInfoAsync(map.FilePath);
            return new ParsedMap(serverMap, map);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch map details for map: {File}", map.FilePath);
            throw new FileLoadException($"Failed to fetch map details for map", map.FilePath);
        }
    }

    public Task<IMap[]> GetCurrentMapListAsync() => mapRepository.GetMapsAsync();

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
    
    private async Task<List<TmMapInfo>> GetRemoteMapListAsync()
    {
        var serverMapList = await serverClient.Remote.GetMapListAsync(-1, 0);
        var calls = new List<MultiCall>();
        
        // server can have more maps than calls allowed in a multicall, so split it up
        for (var i = 0; i < serverMapList.Length; i++)
        {
            if (i % 20 == 0)
            {
                calls.Add(new MultiCall());
            }
            
            calls.Last().Add("GetMapInfo", serverMapList[i].FileName);
        }

        var maplist = new List<TmMapInfo>();
        
        foreach (var call in calls)
        {
            var mapsWithDetails = await serverClient.Remote.MultiCallAsync(call);
            
            foreach (GbxDynamicObject serverMap in mapsWithDetails)
            {
                maplist.Add(new TmMapInfo
                {
                    UId = (string)serverMap["UId"],
                    Name = (string)serverMap["Name"],
                    FileName = (string)serverMap["FileName"],
                    Author = (string)serverMap["Author"],
                    AuthorNickname = (string)serverMap["AuthorNickname"],
                    Environnement = (string)serverMap["Environnement"],
                    Mood = (string)serverMap["Mood"],
                    BronzeTime = (int)serverMap["BronzeTime"],
                    SilverTime = (int)serverMap["SilverTime"],
                    GoldTime = (int)serverMap["GoldTime"],
                    AuthorTime = (int)serverMap["AuthorTime"],
                    CopperPrice = (int)serverMap["CopperPrice"],
                    LapRace = (bool)serverMap["LapRace"],
                    NbLaps = (int)serverMap["NbLaps"],
                    NbCheckpoints = (int)serverMap["NbCheckpoints"],
                    MapType = (string)serverMap["MapType"],
                    MapStyle = (string)serverMap["MapStyle"]
                });
            }
        }

        return maplist;
    }
}
