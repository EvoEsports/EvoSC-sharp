using EvoSC.Common.Models;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Models;

public class ScoresMatchState : IScoresMatchState
{
    public required Guid TimelineId { get; init; }
    public required MatchStatus Status { get; init; }
    public required DateTime Timestamp { get; init; }
    public required ModeScriptSection Section { get; init; }
    public bool UseTeams { get; init; }
    public int WinnerTeam { get; init; }
    public string? WinnerPlayer { get; init; }
    public required IEnumerable<ITeamScore> Teams { get; init; }
    public required IEnumerable<IPlayerScore> Players { get; init; }
}
