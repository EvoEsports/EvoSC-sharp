namespace EvoSC.Common.Remote.EventArgsModels;

public class PlayerUpdateEventArgs : TimedEventArgs
{
    public required string Login { get; init; }
    public required string AccountId { get; init; }
}
