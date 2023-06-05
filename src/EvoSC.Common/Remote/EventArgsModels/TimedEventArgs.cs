namespace EvoSC.Common.Remote.EventArgsModels;

public class TimedEventArgs: EventArgs
{
    /// <summary>
    /// GameTime of the event occurrence, usually milliseconds since dedicated server start.
    /// </summary>
    public required int Time { get; init; }
}
