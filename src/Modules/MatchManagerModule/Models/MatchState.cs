using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Models;

public class MatchState : IMatchState
{
    public required Guid TimelineId { get; init; }
    public required MatchStatus Status { get; init; }
    public required DateTime Timestamp { get; init; }
}
