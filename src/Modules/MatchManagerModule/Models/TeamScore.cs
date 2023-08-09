using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Models;

public class TeamScore : ITeamScore
{
    public required int TeamId { get; init; }
    public string TeamName { get; init; }
    public int RoundPoints { get; init; }
    public int MapPoints { get; init; }
    public int MatchPoints { get; init; }
}
