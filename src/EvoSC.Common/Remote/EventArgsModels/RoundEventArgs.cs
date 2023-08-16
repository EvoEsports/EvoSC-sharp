namespace EvoSC.Common.Remote.EventArgsModels;

public class RoundEventArgs: EventArgs
{
    public required int Count { get; init; }
    public required int Time { get; init; }
}
