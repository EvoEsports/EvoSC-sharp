using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MapQueueModule.Events;
using EvoSC.Modules.Official.MapQueueModule.Events.Args;
using EvoSC.Modules.Official.MapQueueModule.Interfaces;
using EvoSC.Modules.Official.MapQueueModule.Interfaces.Utils.AsyncDeque;
using EvoSC.Modules.Official.MapQueueModule.Utils.AsyncDeque;

namespace EvoSC.Modules.Official.MapQueueModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MapQueueService(IEventManager events) : IMapQueueService
{
    private readonly IAsyncDeque<IMap> _mapQueue = new AsyncDeque<IMap>();

    public IReadOnlyCollection<IMap> QueuedMaps => _mapQueue.ToArray();
    public int QueuedMapsCount => _mapQueue.Count;

    public Task EnqueueAsync(IMap map)
    {
        _mapQueue.Enqueue(map);
        return events.RaiseAsync(MapQueueEvents.MapQueued, new MapQueueEventArgs { Map = map });
    }

    public async Task<IMap> DequeueNextAsync()
    {
        var map = _mapQueue.Dequeue();
        await events.RaiseAsync(MapQueueEvents.MapDequeued, new MapQueueEventArgs { Map = map });

        return map;
    }

    public Task<IMap> PeekNextAsync()
    {
        return Task.FromResult(_mapQueue.PeekFirst());
    }

    public Task DropAsync(IMap map)
    {
        var isNext = _mapQueue.PeekFirst() == map;
        _mapQueue.Drop(map);
        return events.RaiseAsync(MapQueueEvents.MapDropped, new MapQueueMapDroppedEventArgs
        {
            Map = map, 
            WasNext = isNext 
        });
    }

    public Task ClearAsync()
    {
        _mapQueue.Clear();
        return events.RaiseAsync(MapQueueEvents.QueueCleared, EventArgs.Empty);
    }
}
