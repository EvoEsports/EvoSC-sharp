using EvoSC.Common.Models.Callbacks;

namespace EvoSC.Common.Remote.EventArgsModels;

public class ScoresEventArgs : EventArgs
{
    /// <summary>
    /// Section when the event happened.
    /// Known values are: "" | "EndRound" | "EndMap" | "EndMatch"
    /// </summary>
    public required string? Section { get; init; }
    /// <summary>
    /// Does the Game mode use teams
    /// </summary>
    public required bool UseTeams { get; init; }
    /// <summary>
    /// Winning team number
    /// </summary>
    public required int WinnerTeam { get; init; }
    /// <summary>
    /// Login of the player who won
    /// </summary>
    public required string? WinnerPlayer { get; init; }
    /// <summary>
    /// Team Scores
    /// </summary>
    public required IEnumerable<TeamScore?> Teams { get; init; }
    /// <summary>
    /// Player Scores
    /// </summary>
    public required IEnumerable<PlayerScore?> Players { get; init; }
}
