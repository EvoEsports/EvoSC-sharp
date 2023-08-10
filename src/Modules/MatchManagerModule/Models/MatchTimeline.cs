using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Models;

public class MatchTimeline : IMatchTimeline
{
    public Guid TimelineId { get; } = Guid.NewGuid();
    public List<IMatchState> States { get; init; } = new();
}
