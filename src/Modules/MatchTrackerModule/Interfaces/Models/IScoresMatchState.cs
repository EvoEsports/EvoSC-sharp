using EvoSC.Common.Models;

namespace EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

public interface IScoresMatchState : IMatchState
{
    public ModeScriptSection Section { get; }
    public bool UseTeams { get; }
    public int WinnerTeam { get; }
    public string? WinnerPlayer { get; }
    public IEnumerable<ITeamScore> Teams { get; }
    public IEnumerable<IPlayerScore> Players { get; }
}
