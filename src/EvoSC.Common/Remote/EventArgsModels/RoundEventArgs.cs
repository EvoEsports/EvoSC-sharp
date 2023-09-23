namespace EvoSC.Common.Remote.EventArgsModels;

public class RoundEventArgs: EventArgs
{
    /// <summary>
    /// Each time a round event is called, this number is incremented by one.
    /// </summary>
    public required int Count { get; init; }

    /// <summary>
    /// Server time when the callback was sent.
    /// </summary>
    public required int Time { get; init; }
}
