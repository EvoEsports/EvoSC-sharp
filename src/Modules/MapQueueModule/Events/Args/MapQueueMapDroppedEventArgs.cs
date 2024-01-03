namespace EvoSC.Modules.Official.MapQueueModule.Events.Args;

public class MapQueueMapDroppedEventArgs : MapQueueEventArgs
{
    public bool WasNext { get; init; }
}
