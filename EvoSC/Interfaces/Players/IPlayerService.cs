using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Domain.Players;
using GbxRemoteNet.Structs;

namespace EvoSC.Interfaces.Players;

public interface IPlayerService
{
    /// <summary>
    /// Add currently connected players to the internal cache.
    /// </summary>
    public Task AddConnectedPlayers();

    public Task ClientOnPlayerConnect(string login, bool isSpectator);

    public Task ClientOnPlayerDisconnect(string login, string reason);

    /// <summary>
    /// List of connected players.
    /// </summary>
    public List<IPlayer> ConnectedPlayers { get; }

    public Task ClientOnPlayerInfoChanged(SPlayerInfo playerInfo);

    public Task<IPlayer> GetPlayer(string login, bool refreshDb=false);
}
