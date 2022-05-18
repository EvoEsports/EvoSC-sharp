using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Domain.Players;

namespace EvoSC.Interfaces.Players;

public interface IPlayerService
{
    public Task AddConnectedPlayers();
    public Task ClientOnPlayerConnect(string login, bool isSpectator);
    public Task ClientOnPlayerDisconnect(string login, string reason);
    public List<Player> GetConnectedPlayers();
}
