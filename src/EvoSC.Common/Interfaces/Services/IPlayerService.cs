using EvoSC.Common.Database.Models.Player;

namespace EvoSC.Common.Interfaces.Services;

public interface IPlayerService
{
    /// <summary>
    /// Get a player by it's database id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<DbPlayer?> GetPlayerById(long id);
    
    /// <summary>
    /// Get a player by it's login.
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public Task<DbPlayer?> GetPlayerByLogin(string login);
    
    /// <summary>
    /// Create a new player in the database and return the new player object.
    /// </summary>
    /// <param name="login"></param>
    /// <param name="ubisoftName"></param>
    /// <param name="zone"></param>
    /// <returns></returns>
    public Task<DbPlayer> NewPlayer(string login, string ubisoftName, string? zone);
    
    /// <summary>
    /// Create a new player in the database and return the new player object.
    /// </summary>
    /// <param name="login"></param>
    /// <param name="ubisoftName"></param>
    /// <returns></returns>
    public Task<DbPlayer> NewPlayer(string login, string ubisoftName);
    
    /// <summary>
    /// Update a player's database entry.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public Task<bool> UpdatePlayer(DbPlayer player);
    
    /// <summary>
    /// Delete a player from the database.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public Task<bool> DeletePlayer(DbPlayer player);
}
