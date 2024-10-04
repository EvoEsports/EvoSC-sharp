using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Interfaces;

public interface IRoundRankingService
{
    public Task AddCheckpointDataAsync(IOnlinePlayer player, int checkpointIndex, int checkpointTime, bool isFinish);

    public Task RemoveCheckpointDataAsync(IOnlinePlayer player, int checkpointIndex);

    public Task ClearCheckpointDataAsync();

    /// <summary>
    /// Send the widget to the players.
    /// </summary>
    /// <returns></returns>
    public Task DisplayRoundRankingWidgetAsync();

    public Task HideRoundRankingWidgetAsync();
}
