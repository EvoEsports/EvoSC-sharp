using System.Collections;

namespace EvoSC.Modules.Official.MapQueueModule.Interfaces.Utils.AsyncDeque;

public interface IAsyncDeque<TItem> : ICollection, IEnumerable<TItem>
{
    /// <summary>
    /// Add a new item to the queue.
    /// </summary>
    /// <param name="item"></param>
    public void Enqueue(TItem item);
    
    /// <summary>
    /// Get the oldest item from the queue and remove it.
    /// </summary>
    /// <returns></returns>
    public TItem Dequeue();
    
    /// <summary>
    /// Get the oldest item in the queue without removing it.
    /// </summary>
    /// <returns></returns>
    public TItem PeekFirst();
    
    /// <summary>
    /// Remove an item from the queue in any position.
    /// </summary>
    /// <param name="item"></param>
    public void Drop(TItem item);
    
    /// <summary>
    /// Remove all items in the queue.
    /// </summary>
    public void Clear();
}
