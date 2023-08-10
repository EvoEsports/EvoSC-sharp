namespace EvoSC.Modules.Official.MatchManagerModule.Events.EventArgObjects;

public class MatchStartedEventArgs : EventArgs
{
    public required Guid TimelineId { get; init; }
}
