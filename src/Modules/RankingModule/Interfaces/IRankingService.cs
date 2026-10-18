using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.RankingModule.Models;

namespace EvoSC.Modules.Official.RankingModule.Interfaces;

public interface IRankingService
{
    /// <summary>
    /// Begin counting playtime for a player.
    /// </summary>
    public void StartSession(string accountId);

    /// <summary>
    /// Tell a player who just joined their rank and playtime.
    /// </summary>
    public Task SendWelcomeMessageAsync(string accountId);

    /// <summary>
    /// Stop counting playtime for a player and save what has been counted.
    /// </summary>
    public Task EndSessionAsync(string accountId);

    /// <summary>
    /// Begin counting playtime for everyone who is online right now.
    /// </summary>
    public Task StartSessionsForOnlinePlayersAsync();

    /// <summary>
    /// Save playtime of all running sessions without ending them.
    /// </summary>
    public Task SaveAllSessionsAsync();

    /// <summary>
    /// Save and end all running sessions.
    /// </summary>
    public Task EndAllSessionsAsync();

    /// <summary>
    /// Handle new scores from the game mode. Outside of TimeAttack, the map is counted as played for everyone ranked
    /// and as won for the winner once the end-of-map scores arrive. In TimeAttack the scores are only remembered, as
    /// they are sent throughout the map, and counted when the map switches by <see cref="CommitMapResultAsync"/>.
    /// </summary>
    public Task HandleScoresAsync(ScoresEventArgs scores);

    /// <summary>
    /// Count the latest remembered TimeAttack scores of the map that just ended. Does nothing if there are none.
    /// </summary>
    public Task CommitMapResultAsync();

    /// <summary>
    /// Get all players ordered by rank, best first.
    /// </summary>
    public Task<IReadOnlyList<PlayerRank>> GetRankingAsync();

    /// <summary>
    /// Get the rank of a single player, or null if they have no stats yet.
    /// </summary>
    public Task<PlayerRank?> GetPlayerRankAsync(string accountId);

    /// <summary>
    /// Show the ranking window to a player, with their own rank highlighted or pinned at the bottom.
    /// </summary>
    public Task ShowRankingAsync(IPlayer player);
}
