using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchTrackerModule.Events.EventArgObjects;

public class MatchStateTrackedEventArgs : EventArgs
{
    public required IMatchTimeline Timeline { get; init; }
    public required IMatchState State { get; init; }
}
