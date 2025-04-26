using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchTrackerModule.Models;

public class MatchTimeline : IMatchTimeline
{
    public Guid TimelineId { get; init; } = Guid.NewGuid();
    public List<IMatchState> States { get; init; } = new();
}
