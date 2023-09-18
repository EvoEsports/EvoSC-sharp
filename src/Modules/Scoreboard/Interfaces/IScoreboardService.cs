namespace EvoSC.Modules.Official.Scoreboard.Interfaces;

public interface IScoreboardService
{
    /// <summary>
    /// Sends the scoreboard manialink to all players.
    /// </summary>
    public Task ShowScoreboard();
    /// <summary>
    /// Hide the default game scoreboard.
    /// </summary>
    public Task HideNadeoScoreboard();
    /// <summary>
    /// Shows the default game scoreboard.
    /// </summary>
    public Task ShowNadeoScoreboard();
    /// <summary>
    /// Sends the manialink with additional values used by the scoreboard.
    /// </summary>
    public Task SendRequiredAdditionalInfos();
    /// <summary>
    /// Refreshes the additionally required data and sends the manialink.
    /// </summary>
    public Task LoadAndSendRequiredAdditionalInfos();
    /// <summary>
    /// Sets the current round.
    /// </summary>
    public void SetCurrentRound(int round);
}
