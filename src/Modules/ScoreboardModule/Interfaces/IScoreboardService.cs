using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.ScoreboardModule.Interfaces;

public interface IScoreboardService
{
    /// <summary>
    /// Sends the scoreboard manialink to all players.
    /// </summary>
    public Task ShowScoreboardToAllAsync();
    
    /// <summary>
    /// Sends the scoreboard manialink to a specific players.
    /// </summary>
    public Task ShowScoreboardAsync(IPlayer playerLogin);
    
    /// <summary>
    /// Hide the default game scoreboard.
    /// </summary>
    public Task HideNadeoScoreboardAsync();
    
    /// <summary>
    /// Shows the default game scoreboard.
    /// </summary>
    public Task ShowNadeoScoreboardAsync();
    
    /// <summary>
    /// Sends the manialink with additional values used by the scoreboard.
    /// </summary>
    public Task SendRequiredAdditionalInfoAsync();
    
    /// <summary>
    /// Refreshes the additionally required data and sends the manialink.
    /// </summary>
    public Task LoadAndSendRequiredAdditionalInfoAsync();
    
    /// <summary>
    /// Sets the current round.
    /// </summary>
    public void SetCurrentRound(int round);
}
