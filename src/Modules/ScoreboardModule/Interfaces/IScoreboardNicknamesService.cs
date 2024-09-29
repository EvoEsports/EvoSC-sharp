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
    
    /// <summary>
    /// Converts the nickname repo to a ManiaScript array.
    /// </summary>
    /// <param name="nicknameMap"></param>
    /// <returns></returns>
    public string ToManiaScriptArray(Dictionary<string, string> nicknameMap);

    /// <summary>
    /// Converts an entry of the nickname repo to a ManiaScript array entry.
    /// </summary>
    /// <param name="loginNickname"></param>
    /// <returns></returns>
    public string ToManiaScriptArrayEntry(KeyValuePair<string, string> loginNickname);

    /// <summary>
    /// Escapes a nickname to be safely inserted into a XMl comment.
    /// </summary>
    /// <param name="nickname"></param>
    /// <returns></returns>
    public string EscapeNickname(string nickname);
}
