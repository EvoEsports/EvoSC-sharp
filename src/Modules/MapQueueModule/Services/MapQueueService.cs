using System.Collections.Concurrent;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MapQueueModule.Interfaces;

namespace EvoSC.Modules.Official.MapQueueModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MapQueueService(IEventManager events, IServerClient server, IMapService maps) : IMapQueueService
{
    private Queue<IMap> _mapQueue = new();
    private object _mapQueueLock = new();

    public IReadOnlyCollection<IMap> QueuedMaps
    {
        get
        {
            lock (_mapQueueLock)
            {
                return _mapQueue;
            }
        }
    }

    public async Task EnqueueAsync(IMap map)
    {
        lock (_mapQueueLock)
        {
            _mapQueue.Enqueue(map);
        }
    }

    public async Task<IMap> DequeueNextAsync()
    {
        if (QueuedMaps.Count == 0)
        {
            throw new InvalidOperationException("There are no more maps to dequeue");
        }

        IMap? nextMap;
        
        lock (_mapQueueLock)
        {
            nextMap = _mapQueue.Dequeue();
        }

        if (nextMap == null)
        {
            throw new InvalidOperationException("Failed to get next map from the queue");
        }

        return nextMap;
    }

    public Task<IMap> PeekNextAsync()
    {
        if (QueuedMaps.Count == 0)
        {
            throw new InvalidOperationException("There are no maps in the queue");
        }
        
        lock (_mapQueueLock)
        {
            return Task.FromResult(_mapQueue.Peek());
        }
    }

    public Task DropAsync(IMap map)
    {
        lock (_mapQueueLock)
        {
            
        }
        
        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        lock (_mapQueueLock)
        {
            _mapQueue.Clear();
        }
        
        return Task.CompletedTask;
    }
}
