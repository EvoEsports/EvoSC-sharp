

using System.Threading.Tasks;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using GbxRemoteNet.Structs;
using NLog.LayoutRenderers;

namespace EvoSC.Domain.Players;

public class Player : IServerPlayer
{
    GbxRemoteClient IServerPlayer.Client => this.Client;
    private GbxRemoteClient Client { get; set; }
    
    /// <summary>
    /// Player's login UID.
    /// </summary>
    public string Login => Info.Login;
    /// <summary>
    /// Player's Ubisoft name.
    /// </summary>
    public string Name => Info.NickName;
    /// <summary>
    /// Player's Nickname as saved in the database.
    /// </summary>
    public string NickName => DbPlayer.Nickname;
    /// <summary>
    /// Extra information about the player on the server.
    /// </summary>
    public PlayerDetailedInfo? DetailedInfo { get;  private set; }
    /// <summary>
    /// Information about the player on the server.
    /// </summary>
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
    
    /// <summary>
    /// Fetch player info from the server and create a new instance.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="dbPlayer"></param>
    /// <returns></returns>
    public static async Task<Player> Create(GbxRemoteClient client, DatabasePlayer dbPlayer)
    {
        var info = await client.GetPlayerInfoAsync(dbPlayer.Login);
        var detailed = await client.GetDetailedPlayerInfoAsync(dbPlayer.Login);

        return new Player(client, dbPlayer, info, detailed);
    }

    /// <summary>
    /// Update the player model's server state.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="detailedInfo"></param>
    public void Update(PlayerInfo info, PlayerDetailedInfo? detailedInfo=null)
    {
        Info = info;

        if (detailedInfo != null)
        {
            DetailedInfo = detailedInfo;
        }
    }

    /// <summary>
    /// Update the player model's database state.
    /// </summary>
    /// <param name="dbPlayer"></param>
    public void Update(DatabasePlayer dbPlayer)
    {
        DbPlayer = dbPlayer;
    }

    /// <summary>
    /// Update the player model's server state.
    /// </summary>
    /// <param name="playerInfo"></param>
    /// <returns></returns>
    public void Update(SPlayerInfo playerInfo)
    {
        Info.Login = playerInfo.Login;
        Info.NickName = playerInfo.NickName;
        Info.PlayerId = playerInfo.PlayerId;
        Info.TeamId = playerInfo.TeamId;
        Info.SpectatorStatus = playerInfo.SpectatorStatus;
        Info.LadderRanking = playerInfo.LadderRanking;
        Info.Flags = playerInfo.Flags;

        if (DetailedInfo != null)
        {
            DetailedInfo.Login = playerInfo.Login;
            DetailedInfo.NickName = playerInfo.NickName;
            DetailedInfo.PlayerId = playerInfo.PlayerId;
            DetailedInfo.TeamId = playerInfo.TeamId;
        }
    }
}
