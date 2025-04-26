using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Callbacks;

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
    /// Sets the round, map and match points for a team.
    /// </summary>
    /// <param name="team">Team to set the round points for.</param>
    /// <param name="points">Points to set.</param>
    /// <returns></returns>
    public Task SetTeamPointsAsync(PlayerTeam team, int points);

    /// <summary>
    /// Sets the round points for a team.
    /// </summary>
    /// <param name="team">Team to set the round points for.</param>
    /// <param name="newRoundPoints">Points to set.</param>
    /// <returns></returns>
    public Task SetTeamRoundPointsAsync(PlayerTeam team, int newRoundPoints);
    
    /// <summary>
    /// Sets the map points for a team.
    /// </summary>
    /// <param name="team">Team to set the map points for.</param>
    /// <param name="newMapPoints">Points to set.</param>
    /// <returns></returns>
    public Task SetTeamMapPointsAsync(PlayerTeam team, int newMapPoints);
    
    /// <summary>
    /// Sets the match points for a team.
    /// </summary>
    /// <param name="team">Team to set the match points for.</param>
    /// <param name="newMatchPoints">Points to set.</param>
    /// <returns></returns>
    public Task SetTeamMatchPointsAsync(PlayerTeam team, int newMatchPoints);

    /// <summary>
    /// Retrieves the current score for the given player team.
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    public Task<TeamScore> GetTeamScoreAsync(PlayerTeam team);

    /// <summary>
    /// Sets the round, map and match points for a team.
    /// </summary>
    /// <param name="team"></param>
    /// <param name="roundPoints"></param>
    /// <param name="mapPoints"></param>
    /// <param name="matchPoints"></param>
    /// <returns></returns>
    public Task UpdateTeamScoreAsync(PlayerTeam team, int roundPoints, int mapPoints, int matchPoints);
    
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

    /// <summary>
    /// Request the scores from the current match.
    /// </summary>
    /// <returns></returns>
    public Task RequestScoresAsync();
}
