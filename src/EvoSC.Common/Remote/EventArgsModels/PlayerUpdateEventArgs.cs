namespace EvoSC.Common.Remote.EventArgsModels;

public class PlayerUpdateEventArgs : EventArgs
{
    public required int Time { get; init; }
    public required string Login { get; init; }
    public required string AccountId { get; init; }
}
