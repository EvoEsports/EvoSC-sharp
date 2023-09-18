namespace EvoSC.Modules.Official.MatchTrackerModule.Events.EventArgObjects;

public class MatchStartedEventArgs : EventArgs
{
    public required Guid TimelineId { get; init; }
}
