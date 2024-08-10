using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces;

public interface IMatchControlService
{
    public Task StartMatchAsync();

    public Task EndMatchAsync();
    
    /// <summary>
    /// End the current round.
    /// </summary>
    /// <returns></returns>
    public Task EndRoundAsync();
    
    /// <summary>
    /// Restart the current match from the start.
    /// </summary>
    /// <returns></returns>
    public Task RestartMatchAsync();
    
    /// <summary>
    /// Skip to the next map in the rotation.
    /// </summary>
    /// <returns></returns>
    public Task SkipMapAsync();

    /// <summary>
    /// Sets the round points for a team.
    /// </summary>
    /// <param name="team">Team to set the round points for.</param>
    /// <param name="points">Points to set.</param>
    /// <returns></returns>
    public Task SetTeamRoundPointsAsync(PlayerTeam team, int points);
    
    /// <summary>
    /// Sets the map points for a team.
    /// </summary>
    /// <param name="team">Team to set the map points for.</param>
    /// <param name="points">Points to set.</param>
    /// <returns></returns>
    public Task SetTeamMapPointsAsync(PlayerTeam team, int points);
    
    /// <summary>
    /// Sets the match points for a team.
    /// </summary>
    /// <param name="team">Team to set the match points for.</param>
    /// <param name="points">Points to set.</param>
    /// <returns></returns>
    public Task SetTeamMatchPointsAsync(PlayerTeam team, int points);
    
    /// <summary>
    /// Pause the current match. Only works on round-based modes.
    /// </summary>
    /// <returns></returns>
    public Task PauseMatchAsync();
    
    /// <summary>
    /// Unpause the current match. Only works on round-based modes.
    /// </summary>
    /// <returns></returns>
    public Task UnpauseMatchAsync();
}
