namespace EvoSC.Common.Remote.EventArgsModels;

public class RoundEventArgs : EventArgs
{
    /// <summary>
    /// The round when the event occured.
    /// </summary>
    public required int Round { get; init; }
}
