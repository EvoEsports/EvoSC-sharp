namespace EvoSC.Common.Remote.EventArgsModels;

public class PlayerUpdateEventArgs : EventArgs
{
    public int Time { get; init; }
    public string Login { get; init; }
    public string AccountId { get; init; }
}
