using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MapQueueModule.Events.Args;

public class MapQueueEventArgs : EventArgs
{
    /// <summary>
    /// The map that was added or removed from the queue.
    /// </summary>
    public IMap QueuedMap { get; init; }
}
