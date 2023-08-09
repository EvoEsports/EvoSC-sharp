using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;

namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

public interface IScoresMatchState : IMatchState
{
    public ModeScriptSection Section { get; }
    public bool UseTeams { get; }
    public int WinnerTeam { get; }
    public string? WinnerPlayer { get; }
    public IEnumerable<ITeamScore> Teams { get; }
    public IEnumerable<IPlayerScore> Players { get; }
}
