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
        var map = await maps.GetCurrentMapAsync();

        try
        {
            await mapQueue.DropAsync(map);
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Failed to drop map '{MapUid}' from the queue", args.Map.Uid);
            
            if (mapQueue.QueuedMapsCount > 0)
            {
                var next = await mapQueue.PeekNextAsync();
                await server.Remote.ChooseNextMapAsync(next.FilePath);
            }
        }
    }

    [Subscribe(MapQueueEvents.MapQueued)]
    public async Task OnMapQueuedAsync(object sender, MapQueueEventArgs args)
    {
        logger.LogDebug("Queued map {Name}, number of queued maps: {Count}", args.Map.Name,
            mapQueue.QueuedMapsCount);
        
        if (mapQueue.QueuedMapsCount == 1)
        {
            await server.Remote.ChooseNextMapAsync(args.Map.FilePath);
        }

        await server.Chat.InfoMessageAsync($"Map queued: {args.Map.Name}");
    }

    [Subscribe(MapQueueEvents.MapDropped)]
    public async Task OnMapDroppedAsync(object sender, MapQueueMapDroppedEventArgs args)
    {
        if (mapQueue.QueuedMapsCount > 0)
        {
            var next = await mapQueue.PeekNextAsync();
            await server.Remote.ChooseNextMapAsync(next.FilePath);
        }
        
        if (!args.WasNext)
        {
            await server.Chat.InfoMessageAsync($"Map removed from queue: {args.Map.Name}");
        }
    }
}
