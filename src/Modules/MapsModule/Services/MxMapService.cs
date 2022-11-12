using EvoSC.Common.Clients;
using EvoSC.Common.Clients.Dtos;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Modules.Official.Maps.Interfaces;

namespace EvoSC.Modules.Official.Maps.Services;

public class MxMapService: IMxMapService
{
    private readonly MxClient _mxClient;
    private readonly IMapService _mapService;
    
    public MxMapService(MxClient mxClient, IMapService mapService)
    {
        _mxClient = mxClient;
        _mapService = mapService;
    }
    
    public async Task FindAndDownloadMap(int mxId, string? shortName)
    {
        Stream mapStream = await _mxClient.GetMapAsync(mxId, shortName);
        MxMapInfoDto mapInfoDto = await _mxClient.GetMapInfoAsync(mxId, shortName);

        var map = new Map
        {
            Uid = mapInfoDto.TrackUid,
            Name = mapInfoDto.GbxMapName
        };

        await _mapService.AddMap(mapStream, map);
    }
}
