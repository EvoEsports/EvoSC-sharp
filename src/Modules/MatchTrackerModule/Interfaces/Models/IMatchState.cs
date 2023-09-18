using EvoSC.Modules.Official.MatchTrackerModule.Models;

namespace EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

public interface IMatchState
{
    public Guid TimelineId { get; }
    public MatchStatus Status { get; }
    public DateTime Timestamp { get; }
}
