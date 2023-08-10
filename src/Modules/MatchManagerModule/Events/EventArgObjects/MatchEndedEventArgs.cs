using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Events.EventArgObjects;

public class MatchEndedEventArgs : EventArgs
{
    public required IMatchTimeline Timeline { get; init; }
}
