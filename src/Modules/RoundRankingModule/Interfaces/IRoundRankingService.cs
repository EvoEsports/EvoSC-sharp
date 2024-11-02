using EvoSC.Common.Interfaces.Models;
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
    
    /// <summary>
    /// Determines the winning team based on the given checkpoint data.
    /// </summary>
    /// <param name="checkpoints"></param>
    /// <returns></returns>
    public PlayerTeam GetWinnerTeam(List<CheckpointData> checkpoints);

    /// <summary>
    /// Determines whether the winner team should be displayed in the widget.
    /// </summary>
    /// <param name="checkpoints"></param>
    /// <returns></returns>
    public bool ShouldShowWinnerTeam(List<CheckpointData> checkpoints);

    /// <summary>
    /// Traverses the checkpoint data list and sets the gained points on each entry.
    /// </summary>
    /// <param name="checkpoints"></param>
    public void SetGainedPointsOnResult(List<CheckpointData> checkpoints);
    
    /// <summary>
    /// Traverses the checkpoint data, calculates and sets the time differences to the leading player.
    /// </summary>
    /// <param name="checkpoints"></param>
    public void CalculateAndSetTimeDifferenceOnResult(List<CheckpointData> checkpoints);
    
    /// <summary>
    /// Traverses the checkpoint data and sets the accent color on each entry.
    /// </summary>
    /// <param name="checkpoints"></param>
    public void SetGainedPointsBackgroundColorsOnResult(List<CheckpointData> checkpoints);
}
