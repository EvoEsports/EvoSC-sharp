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
    /// Determines whether the checkpoint data repository can collect another player.
    /// Should return false if max entries is reached.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public bool ShouldCollectCheckpointData(string accountId);

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
    /// Gets the latest team infos and caches them.
    /// </summary>
    /// <returns></returns>
    public Task FetchAndCacheTeamInfoAsync();

    /// <summary>
    /// Returns the accent color for the given team.
    /// </summary>
    /// <param name="winnerTeam"></param>
    /// <param name="playerTeam"></param>
    /// <returns></returns>
    public string? GetTeamAccentColor(PlayerTeam winnerTeam, PlayerTeam playerTeam);
    
    /// <summary>
    /// Determines the winning team based on the given checkpoint data.
    /// </summary>
    /// <param name="checkpoints"></param>
    /// <returns></returns>
    public PlayerTeam GetWinnerTeam(List<CheckpointData> checkpoints);

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
