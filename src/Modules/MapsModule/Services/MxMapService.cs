using System.Globalization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MapsModule.Interfaces;
using ManiaExchange.ApiClient;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MapsModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MxMapService(ILogger<MxMapService> logger, IMapService mapService) : IMxMapService
{
    public async Task<IMap?> FindAndDownloadMapAsync(int mxId, string? shortName, IPlayer actor)
    {
        var tmxApi = new MxTmApi("EvoSC#");
        var mapFile = await tmxApi.DownloadMapAsync(mxId, shortName);

        if (mapFile == null)
        {
            logger.LogDebug("Could not find any map stream for ID {MxId} from Trackmania Exchange", mxId);
            return null;
        }

        var mapInfoDto = await tmxApi.GetMapInfoAsync(mxId, shortName);

        if (mapInfoDto == null)
        {
            logger.LogDebug("Could not find any map info for ID {MxId} from Trackmania Exchange", mxId);
            return null;
        }

        var mapMetadata = new MapMetadata
        {
            MapUid = mapInfoDto.TrackUID,
            MapName = mapInfoDto.GbxMapName,
            AuthorId = mapInfoDto.AuthorLogin,
            AuthorName = mapInfoDto.Username,
            ExternalId = mapInfoDto.TrackID.ToString(),
            ExternalVersion = Convert.ToDateTime(mapInfoDto.UpdatedAt, NumberFormatInfo.InvariantInfo).ToUniversalTime(),
            ExternalMapProvider = MapProviders.ManiaExchange
        };

        var map = new MapStream(mapMetadata, mapFile);

        return await mapService.AddMapAsync(map);
    }
}
