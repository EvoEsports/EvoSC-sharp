using System.Globalization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.Maps.Interfaces;
using ManiaExchange.ApiClient;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Maps.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MxMapService : IMxMapService
{
    private readonly ILogger<MxMapService> _logger;
    private readonly IMapService _mapService;

    public MxMapService(ILogger<MxMapService> logger, IMapService mapService)
    {
        _logger = logger;
        _mapService = mapService;
    }

    public async Task<IMap?> FindAndDownloadMapAsync(int mxId, string? shortName, IPlayer actor)
    {
        var tmxApi = new MxTmApi("EvoSC#");
        var mapFile = await tmxApi.DownloadMapAsync(mxId, shortName);

        if (mapFile == null)
        {
            _logger.LogDebug("Could not find any map stream for ID {MxId} from Trackmania Exchange", mxId);
            return null;
        }

        var mapInfoDto = await tmxApi.GetMapInfoAsync(mxId, shortName);

        if (mapInfoDto == null)
        {
            _logger.LogDebug("Could not find any map info for ID {MxId} from Trackmania Exchange", mxId);
            return null;
        }

        var mapMetadata = new MapMetadata
        {
            MapUid = mapInfoDto.TrackUID,
            MapName = mapInfoDto.GbxMapName,
            AuthorId = mapInfoDto.AuthorLogin,
            AuthorName = mapInfoDto.Username,
            ExternalId = mapInfoDto.MapID.ToString(),
            ExternalVersion = Convert.ToDateTime(mapInfoDto.UpdatedAt, NumberFormatInfo.InvariantInfo).ToUniversalTime(),
            ExternalMapProvider = MapProviders.ManiaExchange
        };

        var map = new MapStream(mapMetadata, mapFile);

        return await _mapService.AddMapAsync(map);
    }
}
