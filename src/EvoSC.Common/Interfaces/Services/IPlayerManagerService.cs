using EvoSC.Common.Database.Models;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Interfaces.Services;

public interface IPlayerManagerService
{
    public Task<IPlayer?> GetPlayerAsync(string accountId);
    public Task<IPlayer> GetOrCreatePlayerAsync(string accountId);
    public Task<IPlayer> CreatePlayerAsync(string accountId);
    public Task<IOnlinePlayer> GetOnlinePlayerAsync(string accountId);
    public Task<IOnlinePlayer> GetOnlinePlayerAsync(IPlayer player);
    public Task UpdateLastVisitAsync(IPlayer player);
}
