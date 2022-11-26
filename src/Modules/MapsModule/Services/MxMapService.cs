using EvoSC.Common.Clients;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Common.Util;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.Maps.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Maps.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MxMapService: IMxMapService
{
    private readonly ILogger<MxMapService> _logger;
    private readonly MxClient _mxClient;
    private readonly IMapService _mapService;
    
    public MxMapService(ILogger<MxMapService> logger, MxClient mxClient, IMapService mapService)
    {
        _logger = logger;
        _mxClient = mxClient;
        _mapService = mapService;
    }
    
    public async Task<Map?> FindAndDownloadMap(int mxId, string? shortName, IPlayer actor)
    {
        var mapStream = await _mxClient.GetMapAsync(mxId, shortName);
        
        if (mapStream == null)
        {
            _logger.LogDebug($"Could not find any map stream for ID {mxId} from Trackmania Exchange.");
            return null;
        }
        
        var mapInfoDto = await _mxClient.GetMapInfoAsync(mxId, shortName);

        var map = new Map
        {
            Uid = mapInfoDto.TrackUid,
            Name = mapInfoDto.GbxMapName,
            AuthorId = PlayerUtils.ConvertLoginToAccountId(mapInfoDto.AuthorLogin),
            AuthorName = mapInfoDto.Username,
            MxId = mxId
        };

        return await _mapService.AddMap(new MapObject(map, mapStream), actor);
    }
}
