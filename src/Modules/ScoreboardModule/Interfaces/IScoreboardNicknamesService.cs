namespace EvoSC.Modules.Official.ScoreboardModule.Interfaces;

public interface IScoreboardNicknamesService
{
    public Task AddNicknameByLoginAsync(string login);
    
    public Task ClearNicknamesAsync();
    
    public Task LoadNicknamesAsync();

    public Task SendNicknamesManialinkAsync();
    
    public string ToManiaScriptArray(Dictionary<string, string> nicknameMap);

    public string ToManiaScriptArrayEntry(KeyValuePair<string, string> loginNickname);

    public string EscapeNickname(string nickname);
}
