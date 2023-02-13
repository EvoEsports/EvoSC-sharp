using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Interfaces.Services;

public interface IPlayerManagerService
{
    /// <summary>
    /// Get a player by their account ID.
    /// </summary>
    /// <param name="accountId">Account ID of the player.</param>
    /// <returns></returns>
    public Task<IPlayer?> GetPlayerAsync(string accountId);
    
    /// <summary>
    /// Get a player by their account ID. If the player does not
    /// exist in the database, create the entry.
    /// </summary>
    /// <param name="accountId">Account ID of the player.</param>
    /// <returns></returns>
    public Task<IPlayer> GetOrCreatePlayerAsync(string accountId);
    
    /// <summary>
    /// Create a new entry for a player in the database.
    /// </summary>
    /// <param name="accountId">Account ID of the player.</param>
    /// <returns></returns>
    public Task<IPlayer> CreatePlayerAsync(string accountId);

    /// <summary>
    /// Create a player with a name.
    /// </summary>
    /// <param name="accountId">The account ID of the player.</param>
    /// <param name="name">The name of the player.</param>
    /// <returns></returns>
    public Task<IPlayer> CreatePlayerAsync(string accountId, string? name);
    
    /// <summary>
    /// Get a player that is currently playing on the server.
    /// </summary>
    /// <param name="accountId">Account ID of the player.</param>
    /// <returns></returns>
    public Task<IOnlinePlayer> GetOnlinePlayerAsync(string accountId);
    
    /// <summary>
    /// Get a player that is currently playing on the server.
    /// </summary>
    /// <param name="player">Information about the player to get.</param>
    /// <returns>Returns an IOnlinePlayer instance of the player.</returns>
    public Task<IOnlinePlayer> GetOnlinePlayerAsync(IPlayer player);
    
    /// <summary>
    /// Update the last visited column of a player in the database.
    /// </summary>
    /// <param name="player">The player to update.</param>
    /// <returns></returns>
    public Task UpdateLastVisitAsync(IPlayer player);

    /// <summary>
    /// Get all online players.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<IOnlinePlayer>> GetOnlinePlayersAsync();

    /// <summary>
    /// Find a player by their nickname, this uses pattern search
    /// and you don't need to provide the full nickname.
    /// </summary>
    /// <param name="nickname">The search pattern to look for.</param>
    /// <returns>A collection of players sorted by best match.</returns>
    public Task<IEnumerable<IOnlinePlayer>> FindOnlinePlayerAsync(string nickname);
}
