using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Interfaces;

public interface IPlayerReadyTrackerService
{
    public IEnumerable<IPlayer> ReadyPlayers { get; }
    public List<IPlayer> RequiredPlayers { get; }
    public bool MatchStarted { get; }
    
    public Task SetIsReadyAsync(IPlayer player, bool isReady);
    public Task AddRequiredPlayerAsync(IPlayer player);
    public Task AddRequiredPlayersAsync(IEnumerable<IPlayer> players);
    
    public void Reset();
    public void Reset(bool resetRequired);
    public void SetMatchStarted(bool started);
}
