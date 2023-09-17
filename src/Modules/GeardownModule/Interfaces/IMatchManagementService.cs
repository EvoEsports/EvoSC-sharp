using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces;

public interface IMatchManagementService
{
    /// <summary>
    /// Set custom score for a player.
    /// </summary>
    /// <param name="player">The player to set the score for.</param>
    /// <param name="points">Number of points to set the score to.</param>
    /// <returns></returns>
    public Task SetPlayerPointsAsync(IPlayer player, int points);
    
    /// <summary>
    /// Pause the current match.
    /// </summary>
    /// <returns></returns>
    public Task PauseMatchAsync();
    
    /// <summary>
    /// Resume the current match from a paused state.
    /// </summary>
    /// <returns></returns>
    public Task UnpauseMatchAsync();
}
