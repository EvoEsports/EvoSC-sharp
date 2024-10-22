namespace EvoSC.Modules.Official.ScoreboardModule.Interfaces;

public interface IScoreboardService
{
    /// <summary>
    /// Sends the scoreboard manialink to all players.
    /// </summary>
    public Task SendScoreboardAsync();
    
    /// <summary>
    /// Hide the default game scoreboard.
    /// </summary>
    public Task HideNadeoScoreboardAsync();
    
    /// <summary>
    /// Shows the default game scoreboard.
    /// </summary>
    public Task ShowNadeoScoreboardAsync();

    /// <summary>
    /// Sets the current round number.
    /// </summary>
    /// <param name="roundNumber"></param>
    /// <returns></returns>
    public Task SetCurrentRoundAsync(int roundNumber);

    /// <summary>
    /// Sets whether warm is currently ongoing.
    /// </summary>
    /// <param name="isWarmUp"></param>
    /// <returns></returns>
    public Task SetIsWarmUpAsync(bool isWarmUp);

    /// <summary>
    /// Sends the MetaData manialink.
    /// </summary>
    /// <returns></returns>
    public Task SendMetaDataAsync();
}
