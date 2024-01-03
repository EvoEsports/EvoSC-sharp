using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MapQueueModule.Events.Args;

public class MapQueueEventArgs : EventArgs
{
    public IMap QueuedMap { get; init; }
}
