using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Interfaces.Services;

public interface IPlayerCacheService
{
    /// <summary>
    /// List of players currently on the server.
    /// </summary>
    public IEnumerable<IOnlinePlayer> OnlinePlayers { get; }

    /// <summary>
    /// Get an online player from the cache, and update the cache if not found.
    /// </summary>
    /// <param name="accountId">The account ID of the player to get.</param>
    /// <returns></returns>
    public Task<IOnlinePlayer?> GetOnlinePlayerCachedAsync(string accountId);
    
    /// <summary>
    /// Get an online player from the cache, and update the cache if not found.
    /// </summary>
    /// <param name="accountId">The account ID of the player to get.</param>
    /// <param name="forceUpdate">Force update the cache.</param>
    /// <returns></returns>
    public Task<IOnlinePlayer?> GetOnlinePlayerCachedAsync(string accountId, bool forceUpdate);

    /// <summary>
    /// Update the whole player list cache.
    /// </summary>
    /// <returns></returns>
    public Task UpdatePlayerListAsync();

    public Task UpdatePlayerAsync(IPlayer player);
}
