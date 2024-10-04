using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Interfaces;

public interface IRoundRankingService
{
    public Task ConsumeCheckpointDataAsync(CheckpointData checkpointData);

    public Task RemovePlayerCheckpointDataAsync(IOnlinePlayer player);
    public Task RemovePlayerCheckpointDataAsync(string accountId);

    public Task ClearCheckpointDataAsync();

    /// <summary>
    /// Send the widget to the players.
    /// </summary>
    /// <returns></returns>
    public Task DisplayRoundRankingWidgetAsync();

    public Task HideRoundRankingWidgetAsync();

    public bool ShouldCollectCheckpointData(string playerAccountId);

    public int GetGainedPoints(int rank);

    public Task UpdatePointsRepartitionAsync();

    public Task SetIsTimeAttackModeAsync(bool isTimeAttackMode);
    
    public Task DetectModeAsync();
}
