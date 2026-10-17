namespace EvoSC.Modules.Official.RankingModule.Models;

/// <param name="Rank">Position in the ranking, starting at 1.</param>
/// <param name="Total">Number of ranked players.</param>
/// <param name="AccountId">Account ID of the player.</param>
/// <param name="NickName">Nickname of the player.</param>
/// <param name="Score">Ranking score between 0 and 100.</param>
/// <param name="Playtime">Total time spent on the server.</param>
/// <param name="MapsPlayed">Number of maps the player was ranked in.</param>
/// <param name="MapsWon">Number of maps the player won.</param>
public record PlayerRank(
    int Rank,
    int Total,
    string AccountId,
    string NickName,
    double Score,
    TimeSpan Playtime,
    int MapsPlayed,
    int MapsWon)
{
    /// <summary>
    /// Plain win rate, wins divided by maps played.
    /// </summary>
    public double WinRate => MapsPlayed == 0 ? 0 : (double)MapsWon / MapsPlayed;

    /// <summary>
    /// Chat color of static text.
    /// </summary>
    public const string TextColor = "$0cc";

    /// <summary>
    /// Wrap a value in white, restoring the text color afterwards.
    /// </summary>
    public static string Highlight(object value) => $"$<$fff{value}$>";

    /// <summary>
    /// Score as a whole number.
    /// </summary>
    public string ScoreText => Math.Round(Score).ToString("0");

    /// <summary>
    /// Playtime in days, hours and minutes, leaving out the units that are zero, e.g. "10d, 5m".
    /// </summary>
    public string PlaytimeText
    {
        get
        {
            var parts = new List<string>();

            if (Playtime.Days > 0)
            {
                parts.Add($"{(int)Playtime.TotalDays}d");
            }

            if (Playtime.Hours > 0)
            {
                parts.Add($"{Playtime.Hours}h");
            }

            if (Playtime.Minutes > 0)
            {
                parts.Add($"{Playtime.Minutes}m");
            }

            return parts.Count == 0 ? "0m" : string.Join(", ", parts);
        }
    }

    /// <summary>
    /// One-line chat description of the score, playtime and wins, with the values highlighted.
    /// </summary>
    public string Summary() =>
        $"{TextColor}{Highlight(ScoreText)} pts, {Highlight(PlaytimeText)} played, " +
        $"{Highlight(MapsWon)}/{Highlight(MapsPlayed)} wins ({Highlight(WinRate.ToString("P0"))})";
}
