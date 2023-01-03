using EvoSC.Common.Interfaces.Models;
using GbxRemoteNet.Structs;

namespace EvoSC.Common.Interfaces.Repository;

public interface IPlayerRepository
{
    public Task<IPlayer?> GetPlayerByAccountIdAsync(string accountId);
    public Task<IPlayer> AddPlayerAsync(string accountId, TmPlayerDetailedInfo playerInfo);
    public Task UpdateLastVisitAsync(IPlayer player);
    
}
