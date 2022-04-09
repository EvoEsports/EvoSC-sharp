using System.Threading.Tasks;

namespace EvoSC.Interfaces.Player;

public interface IPlayerService
{
    public Task ClientOnPlayerConnect(string login, bool isspectator);
    public Task ClientOnPlayerDisconnect(string login, string reason);
}
