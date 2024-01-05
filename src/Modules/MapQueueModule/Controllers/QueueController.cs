using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Modules.Official.MapQueueModule.Events;
using EvoSC.Modules.Official.MapQueueModule.Events.Args;
using EvoSC.Modules.Official.MapQueueModule.Interfaces;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MapQueueModule.Controllers;

[Controller]
public class QueueController(IMapQueueService mapQueue, IServerClient server, IMapService maps, ILogger<QueueController> logger) : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMapAsync(object sender, MapGbxEventArgs args)
    {
        var currentMap = await maps.GetCurrentMapAsync();

        try
        {
            await mapQueue.DropAsync(currentMap);
        }
        catch (Exception ex)
        {
            // if map isn't in the queue, we just ignore it
        } 
        
        if (mapQueue.QueuedMapsCount > 0)
        {
            var next = await mapQueue.PeekNextAsync();
            await server.Remote.ChooseNextMapAsync(next.FilePath);
        }
    }

    [Subscribe(MapQueueEvents.MapQueued)]
    public async Task OnMapQueuedAsync(object sender, MapQueueEventArgs args)
    {
        logger.LogDebug("Queued map {Name}, number of queued maps: {Count}", args.QueuedMap.Name,
            mapQueue.QueuedMapsCount);
        
        if (mapQueue.QueuedMapsCount == 1)
        {
            await server.Remote.ChooseNextMapAsync(args.QueuedMap.FilePath);
        }
    }

    [Subscribe(MapQueueEvents.MapDropped)]
    public async Task OnMapDroppedAsync(object sender, MapQueueMapDroppedEventArgs args)
    {
        if (args.WasNext)
        {
            var next = await mapQueue.PeekNextAsync();
            await server.Remote.ChooseNextMapAsync(next.FilePath);
        }
    }
}
