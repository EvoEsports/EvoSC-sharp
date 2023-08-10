using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Events.EventArgObjects;

public class MatchStateTrackedEventArgs : EventArgs
{
    public required IMatchTimeline Timeline { get; init; }
    public required IMatchState State { get; init; }
}
