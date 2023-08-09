using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

public interface IPlayerScore
{
    public IPlayer Player { get; }
    public int Rank { get; }
    public int RoundPoints { get; }
    public int MapPoints { get; }
    public int MatchPoints { get; }
    public IRaceTime PreviousRaceTime { get; }
    public IRaceTime BestRaceTime { get; }
    public IRaceTime BestLapTime { get; }
    public IEnumerable<IRaceTime> BestRaceCheckpoints { get; }
    public IEnumerable<IRaceTime> PreviousRaceCheckpoints { get; }
    public IEnumerable<IRaceTime> BestLapCheckpoints { get; }
}
