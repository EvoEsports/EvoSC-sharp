using System.Collections.Concurrent;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Interfaces;

public interface IRoundRankingStateService
{
    /// <summary>
    /// Returns the currently set points repartition.
    /// </summary>
    /// <returns></returns>
    public Task<PointsRepartition> GetPointsRepartitionAsync();
    
    /// <summary>
    /// Sets a new points repartition.
    /// </summary>
    /// <param name="pointsRepartitionString"></param>
    /// <returns></returns>
    public Task UpdatePointsRepartitionAsync(string pointsRepartitionString);

    /// <summary>
    /// Returns the repository containing all collected checkpoints for the ongoing round.
    /// </summary>
    /// <returns></returns>
    public Task<CheckpointsRepository> GetRepositoryAsync();

    /// <summary>
    /// Sets or updates the checkpoint data for the given account ID.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="checkpointData"></param>
    /// <returns></returns>
    public Task UpdateRepositoryEntryAsync(string accountId, CheckpointData checkpointData);

    /// <summary>
    /// Returns the checkpoint data for the given account ID.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public Task RemoveRepositoryEntryAsync(string accountId);

    /// <summary>
    /// Removes all entries from the repository.
    /// </summary>
    /// <returns></returns>
    public Task ClearRepositoryAsync();

    /// <summary>
    /// Returns the currently set team colors.
    /// </summary>
    /// <returns></returns>
    public Task<ConcurrentDictionary<PlayerTeam, string>> GetTeamColorsAsync();
    
    /// <summary>
    /// Overwrites the current team colors with new values.
    /// </summary>
    /// <param name="team1Color"></param>
    /// <param name="team2Color"></param>
    /// <param name="teamUnknownColor"></param>
    /// <returns></returns>
    public Task SetTeamColorsAsync(string team1Color, string team2Color, string teamUnknownColor);

    /// <summary>
    /// Updates whether teams mode is active.
    /// </summary>
    /// <param name="isTeamsMode"></param>
    /// <returns></returns>
    public Task SetIsTeamsModeAsync(bool isTeamsMode);

    /// <summary>
    /// Updates whether time attack mode is active.
    /// </summary>
    /// <param name="isTimeAttackMode"></param>
    /// <returns></returns>
    public Task SetIsTimeAttackModeAsync(bool isTimeAttackMode);

    /// <summary>
    /// Returns whether teams mode is active.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetIsTeamsModeAsync();

    /// <summary>
    /// Returns whether time attack mode is active.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetIsTimeAttackModeAsync();
}
