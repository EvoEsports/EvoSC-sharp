using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Modules.Attributes;
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

    public async Task<IMap?> FindAndDownloadMap(int mxId, string? shortName, IPlayer actor)
    {
        var tmxApi = new MxTmApi("EvoSC#");
        var mapFile = await tmxApi.DownloadMapAsync(mxId, shortName);

        if (mapFile == null)
        {
            _logger.LogDebug($"Could not find any map stream for ID {mxId} from Trackmania Exchange.");
            return null;
        }

        var mapInfoDto = await tmxApi.GetMapInfoAsync(mxId, shortName);

        if (mapInfoDto == null)
        {
            _logger.LogDebug($"Could not find any map info for ID {mxId} from Trackmania Exchange.");
            return null;
        }

        var mapMetadata = new MapMetadata(
            mapInfoDto.TrackUID,
            mapInfoDto.GbxMapName,
            mapInfoDto.AuthorLogin,
            mapInfoDto.Username,
            mapInfoDto.MapID,
            Convert.ToDateTime(mapInfoDto.UpdatedAt),
            false,
            true
        );

        var map = new MapStream(mapMetadata, mapFile);

        return await _mapService.AddMap(map, actor);
    }
}
