using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Settings;
using EvoSC.Modules.Official.MapsModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Services;

public class MatchMapService(
    ILogger<MatchMapService> logger,
    IMapService mapService,
    INadeoMapService nadeoMapService,
    IServerClient server,
    IMxMapService mxMapService,
    IToornamentSettings settings
    ) : IMatchMapService
{
    public async Task<List<IMap?>> AddMapsAsync(IPlayer player)
    {
        logger.LogDebug("Begin of AddMapsAsync()");
        List<IMap?> maps = [];
        bool allMapsOnServer = true;

        //Check if the maps are already on the server
        try
        {
            foreach (var mapUid in GetMapUids())
            {
                logger.LogDebug("Checking if map with Uid {MapUid} exists on the server", mapUid);
                IMap? existingMap = await mapService.GetMapByUidAsync(mapUid);
                if (existingMap == null)
                {
                    logger.LogDebug("Map with Uid {MapUid} was not found on the server", mapUid);
                    allMapsOnServer = false;
                }
                else
                {
                    maps.Add(existingMap);
                }
            }
        }
        catch (ArgumentNullException)
        {
            //Silently catch Exception, since we can use Nadeo Servers or TMX as backup
        }

        //Try to download maps from Nadeo Servers using the MapId
        if (!allMapsOnServer)
        {
            try
            {
                maps = await AddMapsFromNadeo(player, GetMapIds());
                if (maps is not null)
                {
                    allMapsOnServer = true;
                }
            }
            catch (ArgumentNullException)
            {
                //Silently catch Exception, since we can use TMX as final backup
            }
        }

        //Try to download maps from TMX using tmx Ids
        if (!allMapsOnServer)
        {
            maps = await AddMapsFromTmx(player, GetTmxIds());
            allMapsOnServer = true;
        }

        //Throw error if maps are still not found
        if (!allMapsOnServer || maps?.Count == 0)
        {
            logger.LogWarning("Maps could not be found on the server, or downloaded from Nadeo servers or from TMX");
            throw new ArgumentException(
                "Maps could not be found on the server, or downloaded from Nadeo servers or from TMX");
        }

        logger.LogDebug("End of AddMapsAsync()");
        return maps;
    }

    public async Task<List<IMap?>> AddMapsFromNadeo(IPlayer player, IEnumerable<string> mapIds)
    {
        List<IMap?> maps = new List<IMap?>();
        try
        {
            maps.AddRange(await Task.WhenAll(mapIds.Select(async m =>
            {
                try
                {
                    logger.LogDebug("Downloading map with id {0} from Nadeo servers", m);
                    return await nadeoMapService.FindAndDownloadMapAsync(m);
                }
                catch (DuplicateMapException ex)
                {
                    //Exception message is "Map with UID {MapUid} already exists in database", we need the MapUid to get the map from the server
                    var mapUid = ex.Message.Split(' ')[3];
                    return await mapService.GetMapByUidAsync(mapUid);
                }
            })));
        }
        catch (Exception)
        {
            logger.LogWarning("Failed to download map from Nadeo servers");
            await server.Chat.ErrorMessageAsync("Failed to add map using the Nadeo servers");
            throw;
        }

        if (maps.Count == mapIds.Count())
        {
            return maps;
        }

        await server.Chat.ErrorMessageAsync("Failed to add all maps from the Nadeo servers");
        return null;

    }


    public async Task<List<IMap?>> AddMapsFromTmx(IPlayer player, IEnumerable<int> mapIds)
    {
        List<IMap?> maps = [];
        try
        {
            maps.AddRange(await Task.WhenAll(mapIds.Select(async m =>
            {
                try
                {
                    logger.LogDebug("Downloading map with id {NadeoId} from Nadeo servers", m);
                    return await mxMapService.FindAndDownloadMapAsync(m, null, player);
                }
                catch (DuplicateMapException ex)
                {
                    //Exception message is "Map with UID {MapUid} already exists in database", we need the MapUid to get the map from the server
                    var mapUid = ex.Message.Split(' ')[3];
                    return await mapService.GetMapByUidAsync(mapUid);
                }
            })));
        }
        catch (Exception)
        {
            logger.LogWarning("Failed to download map from TMX");
            await server.Chat.ErrorMessageAsync("Failed to add map from TMX");
            throw;
        }

        if (maps.Count() != mapIds.Count())
        {
            await server.Chat.ErrorMessageAsync("Failed to add all maps from TMX");
            return null;
        }

        return maps;
    }

    private List<int> GetTmxIds()
    {
        if (string.IsNullOrEmpty(settings.MapTmxIds))
        {
            throw new ArgumentNullException(nameof(settings.MapTmxIds),
                @"Map TMX Ids not defined in environment settings");
        }

        var mapIds = new List<int>();

        foreach (var s in settings.MapTmxIds.Split(','))
        {
            int num;
            if (int.TryParse(s, out num))
            {
                mapIds.Add(num);
            }
        }

        return mapIds;
    }

    private List<string> GetMapUids()
    {
        if (string.IsNullOrEmpty(settings.MapUids))
        {
            throw new ArgumentNullException(nameof(settings.MapUids), @"Map Uids not defined in environment settings");
        }

        return settings.MapUids.Split(',').ToList();
    }

    private List<string> GetMapIds()
    {
        if (string.IsNullOrEmpty(settings.MapIds))
        {
            throw new ArgumentNullException(nameof(settings.MapIds), @"Map Ids not defined in environment settings");
        }

        return settings.MapIds.Split(',').ToList();
    }
}
