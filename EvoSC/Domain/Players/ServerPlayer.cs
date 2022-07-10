
using System.Threading.Tasks;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using GbxRemoteNet.Structs;

namespace EvoSC.Domain.Players;

public class ServerPlayer : IServerPlayer
{
    GbxRemoteClient IServerPlayer.Client => this.Client;
    private GbxRemoteClient Client { get; set; }
    
    public PlayerDetailedInfo DetailedInfo { get; }
    public PlayerInfo Info { get; }
    public string Login => Info.Login;
    public string Name => Info.NickName;

    public ServerPlayer(GbxRemoteClient client, PlayerInfo playerInfo, PlayerDetailedInfo? detailedInfo = null)
    {
        Info = playerInfo;
        DetailedInfo = detailedInfo;
        Client = client;
    }
    
    public static async Task<IServerPlayer> Create(GbxRemoteClient client, string login)
    {
        var info = await client.GetPlayerInfoAsync(login);
        var detailed = await client.GetDetailedPlayerInfoAsync(login);

        return new ServerPlayer(client, info, detailed);
    }
    
    public bool HasPermission(string permission)
    {
        // todo: implement this in some way?
        return false;
    }
}
