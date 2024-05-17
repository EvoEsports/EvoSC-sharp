using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.MapQueueModule.Interfaces;

namespace EvoSC.Modules.Official.MapListModule.Controllers;

[Controller]
public class MapListManialinkController(IMapService mapService, IMapQueueService mapQueueService) : ManialinkController
{
    public async Task QueueMapAsync(string mapUid)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);
        await mapQueueService.EnqueueAsync(map);
    }
    
    public async Task DropMapAsync(string mapUid)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);
        await mapQueueService.DropAsync(map);
    }

    public Task FavoriteMapAsync(string mapUid)
    {
        Console.WriteLine("map favorited! (surely)");
        return Task.CompletedTask;
    }
}
