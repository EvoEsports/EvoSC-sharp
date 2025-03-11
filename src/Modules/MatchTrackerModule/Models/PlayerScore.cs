using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchTrackerModule.Models;

public class PlayerScore : IPlayerScore
{
    public required string AccountId { get; init; }
    public required string UbisoftName { get; init; }
    public int Rank { get; init; }
    public int RoundPoints { get; init; }
    public int MapPoints { get; init;}
    public int MatchPoints { get; init; }
    public IRaceTime PreviousRaceTime { get; init; }
    public IRaceTime BestRaceTime { get; init; }
    public IRaceTime BestLapTime { get; init; }
    public IEnumerable<IRaceTime> BestRaceCheckpoints { get; init; }
    public IEnumerable<IRaceTime> PreviousRaceCheckpoints { get; init; }
    public IEnumerable<IRaceTime> BestLapCheckpoints { get; init; }
}
