using EvoSC.Common.Interfaces.Models;
using GbxRemoteNet.Structs;

namespace EvoSC.Common.Interfaces.Database.Repository;

public interface IPlayerRepository
{
    /// <summary>
    /// Get a player by their account ID.
    /// </summary>
    /// <param name="accountId">The account ID of the player.</param>
    /// <returns></returns>
    public Task<IPlayer?> GetPlayerByAccountIdAsync(string accountId);
    
    /// <summary>
    /// Add a new player to the database.
    /// </summary>
    /// <param name="accountId">The account ID of the player.</param>
    /// <param name="playerInfo">Info about the player.</param>
    /// <returns></returns>
    public Task<IPlayer> AddPlayerAsync(string accountId, TmPlayerDetailedInfo playerInfo);
    
    /// <summary>
    /// Update the last visit of a player to the current time.
    /// </summary>
    /// <param name="player">The player to update.</param>
    /// <returns></returns>
    public Task UpdateLastVisitAsync(IPlayer player);

    public Task UpdateNicknameAsync(IPlayer player, string newNickname);
}
