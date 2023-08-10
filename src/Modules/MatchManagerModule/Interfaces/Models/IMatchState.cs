using EvoSC.Modules.Official.MatchManagerModule.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

public interface IMatchState
{
    public Guid TimelineId { get; }
    public MatchStatus Status { get; }
    public DateTime Timestamp { get; }
}
