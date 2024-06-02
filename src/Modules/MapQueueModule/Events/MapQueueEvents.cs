namespace EvoSC.Modules.Official.MapQueueModule.Events;

public enum MapQueueEvents
{
    /// <summary>
    /// Triggered when a map was added to the queue.
    /// </summary>
    MapQueued,
    
    /// <summary>
    /// Triggered when a map removed from the queue due to
    /// being played as the next map.
    /// </summary>
    MapDequeued,
    
    /// <summary>
    /// Triggered when a map was dropped from the queue before
    /// it is played.
    /// </summary>
    MapDropped,
    
    /// <summary>
    /// Triggered when the entire queue is cleared.
    /// </summary>
    QueueCleared
}
