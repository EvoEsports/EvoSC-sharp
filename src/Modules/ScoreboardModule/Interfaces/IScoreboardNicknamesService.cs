using System.Collections.Concurrent;

namespace EvoSC.Modules.Official.ScoreboardModule.Interfaces;

public interface IScoreboardNicknamesService
{
    /// <summary>
    /// Gets the online player by login and then sets their custom nickname in the repo.
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public Task AddNicknameByLoginAsync(string login);
    
    /// <summary>
    /// Clears the nicknames repo.
    /// </summary>
    /// <returns></returns>
    public Task ClearNicknamesAsync();
    
    /// <summary>
    /// Gets all online players and sets their custom nicknames in the repo.
    /// </summary>
    /// <returns></returns>
    public Task LoadNicknamesAsync();

    /// <summary>
    /// Sends the manialink containing the nicknames in the repo.
    /// </summary>
    /// <returns></returns>
    public Task SendNicknamesManialinkAsync();
}
