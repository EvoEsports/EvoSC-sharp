using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchTrackerModule.Events.EventArgObjects;

public class MatchEndedEventArgs : EventArgs
{
    public required IMatchTimeline Timeline { get; init; }
}
