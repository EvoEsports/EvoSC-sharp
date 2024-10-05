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
    public Task SendRoundRankingWidgetAsync();

    public Task HideRoundRankingWidgetAsync();

    public bool ShouldCollectCheckpointData(string playerAccountId);

    public int GetGainedPointsForRank(int rank);

    public string? GetTeamAccentColor(PlayerTeam playerTeam);
    
    public PlayerTeam GetWinnerTeam();

    public Task UpdatePointsRepartitionAsync();

    public Task SetIsTimeAttackModeAsync(bool isTimeAttackMode);
    
    public Task DetectModeAsync();
    
    public Task FetchAndCacheTeamInfoAsync();

    public void SetGainedPointsOnResult(List<CheckpointData> checkpoints);
    
    public void SetAccentColorsOnResult(List<CheckpointData> checkpoints);
}
