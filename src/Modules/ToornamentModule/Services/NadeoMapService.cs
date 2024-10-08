using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.Official.MapsModule.Services;
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using Hawf.Attributes;
using Hawf.Client;
using Microsoft.Extensions.Logging;
using IMapService = EvoSC.Common.Interfaces.Services.IMapService;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Services;

//TODO Abstract this away in separate module
[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class NadeoMapService(ILogger<MxMapService> logger, IMapService mapService) : INadeoMapService
{
    public async Task<IMap?> FindAndDownloadMapAsync(string mapId)
    {
        var nadeoApi = new MapsResource();
        var mapFile = await nadeoApi.DownloadMapAsync(mapId);

        if (mapFile == null)
        {
            logger.LogDebug("Could not find any map stream for ID {MapId} on Nadeo servers", mapId);
            return null;
        }

        var mapMetadata = GetMapInfo(mapFile, mapId);
        if (mapFile.CanSeek)
        {
            mapFile.Seek(0, SeekOrigin.Begin);
        }
        var map = new MapStream(mapMetadata, mapFile);

        return await mapService.AddMapAsync(map);
    }

    private MapMetadata GetMapInfo(Stream mapFile, string mapId)
    {
        Gbx.LZO = new Lzo();
        var map = Gbx.ParseNode<CGameCtnChallenge>(mapFile);

        return new MapMetadata
        {
            MapUid = map.MapUid,
            MapName = map.MapName,
            AuthorId = map.AuthorLogin,
            AuthorName = map.AuthorNickname ?? map.AuthorLogin,
            ExternalId = mapId,
            ExternalVersion = DateTime.UtcNow,
            ExternalMapProvider = MapProviders.TrackmaniaIo
        };
    }
}

[ApiClient("https://core.trackmania.nadeo.live/maps", userAgent: "EvoSC#-Toornament-Module")]
public class MapsResource() : ApiBase<MapsResource>
{
    public Task<Stream> DownloadMapAsync(string id, CancellationToken cancelToken = default)
    {
        return WithCancelToken(cancelToken).CacheResponseFor(TimeSpan.Zero).GetStreamAsync("/{id}/file", id);
    }
}
