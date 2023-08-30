using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Interfaces;

public interface IPlayerReadyTrackerService
{
    public IEnumerable<IPlayer> ReadyPlayers { get; }
    public int RequiredPlayers { get; }
    
    public Task SetIsReadyAsync(IPlayer player, bool isReady);
    public Task SetRequiredPlayersAsync(int count);
    
    public void Reset();
}
