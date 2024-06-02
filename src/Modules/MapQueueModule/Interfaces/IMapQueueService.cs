using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MapQueueModule.Interfaces;

public interface IMapQueueService
{
    /// <summary>
    /// List of maps that has been added to the queue, ordered by
    /// oldest to newest.
    /// </summary>
    public IReadOnlyCollection<IMap> QueuedMaps { get; }
    
    /// <summary>
    /// Number of maps in the queue.
    /// </summary>
    public int QueuedMapsCount { get; }
    
    /// <summary>
    /// Add a map to the queue.
    /// </summary>
    /// <param name="map">Map to be added.</param>
    /// <returns></returns>
    public Task EnqueueAsync(IMap map);
    
    /// <summary>
    /// Get the oldest map from the queue and remove it.
    /// </summary>
    /// <returns></returns>
    public Task<IMap> DequeueNextAsync();
    
    /// <summary>
    /// Get the oldest map from the queue without removing it.
    /// </summary>
    /// <returns></returns>
    public Task<IMap> PeekNextAsync();
    
    /// <summary>
    /// Remove a map from the queue in any position.
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public Task DropAsync(IMap map);
    
    /// <summary>
    /// Remove all maps from the queue.
    /// </summary>
    /// <returns></returns>
    public Task ClearAsync();
}
