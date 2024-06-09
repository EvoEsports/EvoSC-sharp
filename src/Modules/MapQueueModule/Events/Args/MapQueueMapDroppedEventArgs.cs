namespace EvoSC.Modules.Official.MapQueueModule.Events.Args;

public class MapQueueMapDroppedEventArgs : MapQueueEventArgs
{
    /// <summary>
    /// Whether the map was the next map.
    /// </summary>
    public bool WasNext { get; init; }
}
