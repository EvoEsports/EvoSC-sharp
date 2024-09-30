namespace EvoSC.Common.Remote.EventArgsModels;

public class WarmUpRoundEventArgs: EventArgs
{
    /// <summary>
    /// The current warm-up round.
    /// </summary>
    public required int Current { get; init; }

    /// <summary>
    /// The total amount of warm-ups.
    /// </summary>
    public required int Total { get; init; }
}
