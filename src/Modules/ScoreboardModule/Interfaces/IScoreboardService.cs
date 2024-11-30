namespace EvoSC.Modules.Official.ScoreboardModule.Interfaces;

public interface IScoreboardService
{
    /// <summary>
    /// Sends the scoreboard manialink to all players.
    /// </summary>
    public Task SendScoreboardAsync();

    /// <summary>
    /// Sends the MetaData manialink.
    /// </summary>
    /// <returns></returns>
    public Task SendMetaDataAsync();
    
    /// <summary>
    /// Hide the default game scoreboard.
    /// </summary>
    public Task HideNadeoScoreboardAsync();
    
    /// <summary>
    /// Shows the default game scoreboard.
    /// </summary>
    public Task ShowNadeoScoreboardAsync();
}
