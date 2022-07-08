

using System.Threading.Tasks;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using GbxRemoteNet.Structs;

namespace EvoSC.Domain.Players;

public class Player : IServerPlayer
{
    GbxRemoteClient IServerPlayer.Client => this.Client;
    private GbxRemoteClient Client { get; set; }
    
    public string Login => Info.Login;
    public string Name => Info.NickName;
    public string NickName => DbPlayer.Nickname;
    
    
    public PlayerDetailedInfo? DetailedInfo { get;  private set; }
    public PlayerInfo Info { get; private set; }

    public DatabasePlayer DbPlayer { get; private set; }

    public Player(GbxRemoteClient client, DatabasePlayer dbPlayer, PlayerInfo info,
        PlayerDetailedInfo? detailedInfo = null)
    {
        Client = client;
        DbPlayer = dbPlayer;
        Info = info;
        DetailedInfo = detailedInfo;
    }
    
    public static async Task<Player> Create(GbxRemoteClient client, DatabasePlayer dbPlayer)
    {
        var info = await client.GetPlayerInfoAsync(dbPlayer.Login);
        var detailed = await client.GetDetailedPlayerInfoAsync(dbPlayer.Login);

        return new Player(client, dbPlayer, info, detailed);
    }
}
