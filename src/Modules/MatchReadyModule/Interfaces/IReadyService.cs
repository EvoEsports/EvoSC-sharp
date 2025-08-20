using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Interfaces;

public interface IReadyService
{
    public IEnumerable<IPlayer> ReadyPlayers { get; }
    public IEnumerable<IPlayer> Players { get; }
    public bool Enabled { get; }
    public Task ResetAsync();
    public Task AddPlayersAsync(params IPlayer[] players);
    public Task SetPlayerReadyStatusAsync(IPlayer player, bool isReady);
    public Task EnableAsync();
    public Task DisableAsync();
}
