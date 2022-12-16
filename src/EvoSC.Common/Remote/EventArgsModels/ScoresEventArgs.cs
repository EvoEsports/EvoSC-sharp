using EvoSC.Common.Models.Callbacks;

namespace EvoSC.Common.Remote.EventArgsModels;

public class ScoresEventArgs : EventArgs
{
    public string? Section { get; init; }
    public bool UseTeams { get; init; }
    public int WinnerTeam { get; init; }
    public string? WinnerPlayer { get; init; }
    public IEnumerable<TeamScore?> Teams { get; init; }
    public IEnumerable<PlayerScore?> Players { get; init; }
}
