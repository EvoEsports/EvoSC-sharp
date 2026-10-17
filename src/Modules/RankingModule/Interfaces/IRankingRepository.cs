using EvoSC.Modules.Official.RankingModule.Database.Models;

namespace EvoSC.Modules.Official.RankingModule.Interfaces;

public interface IRankingRepository
{
    /// <summary>
    /// Get the stats of every player that has any recorded, including the player info.
    /// </summary>
    public Task<IReadOnlyList<DbPlayerRankingStats>> GetAllStatsAsync();

    /// <summary>
    /// Add playtime to a player, creating their stats if they have none.
    /// </summary>
    public Task AddPlaytimeAsync(long playerId, long seconds);

    /// <summary>
    /// Record that a player was ranked at the end of a map, and whether they won it.
    /// </summary>
    public Task AddMapResultAsync(long playerId, bool won);
}
