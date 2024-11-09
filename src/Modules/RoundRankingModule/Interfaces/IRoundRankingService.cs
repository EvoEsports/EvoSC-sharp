using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Interfaces;

public interface IRoundRankingService
{
    /// <summary>
    /// Handle new CheckpointData entry
    /// </summary>
    /// <param name="checkpointData"></param>
    /// <returns></returns>
    public Task ConsumeCheckpointDataAsync(CheckpointData checkpointData);

    /// <summary>
    /// Removes the checkpoint data of the player with the given account ID.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public Task RemovePlayerCheckpointDataAsync(string accountId);

    /// <summary>
    /// Clears all checkpoint data.
    /// </summary>
    /// <returns></returns>
    public Task ClearCheckpointDataAsync();

    /// <summary>
    /// Send the round ranking widget to the players.
    /// </summary>
    /// <returns></returns>
    public Task SendRoundRankingWidgetAsync();

    /// <summary>
    /// Hides the round ranking widget from all players.
    /// </summary>
    /// <returns></returns>
    public Task HideRoundRankingWidgetAsync();

    /// <summary>
    /// Gets the current points repartition value and caches it.
    /// </summary>
    /// <returns></returns>
    public Task LoadPointsRepartitionFromSettingsAsync();

    /// <summary>
    /// Sets TimeAttack mode active/inactive.
    /// </summary>
    /// <param name="isTimeAttackMode"></param>
    /// <returns></returns>
    public Task SetIsTimeAttackModeAsync(bool isTimeAttackMode);

    /// <summary>
    /// Detects if team mode is active and caches the result.
    /// </summary>
    /// <returns></returns>
    public Task DetectIsTeamsModeAsync();
    
    /// <summary>
    /// Gets the latest team infos and caches them.
    /// </summary>
    /// <returns></returns>
    public Task FetchAndCacheTeamInfoAsync();
}
