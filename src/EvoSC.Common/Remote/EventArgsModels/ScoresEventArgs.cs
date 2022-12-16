using EvoSC.Common.Models.Callbacks;

namespace EvoSC.Common.Remote.EventArgsModels;

public class ScoresEventArgs : EventArgs
{
    public required string? Section { get; init; }
    public required bool UseTeams { get; init; }
    public required int WinnerTeam { get; init; }
    public required string? WinnerPlayer { get; init; }
    public required IEnumerable<TeamScore?> Teams { get; init; }
    public required IEnumerable<PlayerScore?> Players { get; init; }
}
