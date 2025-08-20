using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Interfaces;

public interface IReadyTrackerService
{
    public IEnumerable<IPlayer> ReadyPlayers { get; }
    public IEnumerable<IPlayer> Players { get; }
    public bool AllReady { get; }
    public bool Enabled { get; }
    
    public Task AddPlayerAsync(IPlayer player);
    public Task RemovePlayerAsync(IPlayer player);
    public Task AddReadyAsync(IPlayer player);
    public Task RemoveReadyAsync(IPlayer player);
    public Task ClearAsync();
    public Task EnableAsync();
    public Task DisableAsync();
}
