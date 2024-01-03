using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MapQueueModule.Interfaces;

public interface IMapQueueService
{
    public IReadOnlyCollection<IMap> QueuedMaps { get; }
    public Task EnqueueAsync(IMap map);
    public Task<IMap> DequeueNextAsync();
    public Task<IMap> PeekNextAsync();
    public Task DropAsync(IMap map);
    public Task ClearAsync();
}
