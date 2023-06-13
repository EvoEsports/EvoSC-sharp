namespace EvoSC.Common.Remote.EventArgsModels;

public class PlayerUpdateEventArgs : TimedEventArgs
{
    /// <summary>
    /// Login of the player
    /// </summary>
    public required string Login { get; init; }
    /// <summary>
    /// WebServices account ID of the player
    /// </summary>
    public required string AccountId { get; init; }
}
