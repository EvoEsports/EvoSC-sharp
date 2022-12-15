using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;

namespace EvoSC.Common.Models.Players;

public class OnlinePlayer : IOnlinePlayer
{
    public long Id { get; init; }
    public string AccountId { get; set; }
    public string NickName { get; set; }
    public string UbisoftName { get; set; }
    public string Zone { get; set; }
    public IEnumerable<IGroup> Groups { get; }
    public required PlayerState State { get; set; }
    public IPlayerFlags Flags { get; set; }

    public OnlinePlayer(){}

    public OnlinePlayer(IPlayer player)
    {
        Id = player.Id;
        AccountId = player.AccountId;
        NickName = player.NickName;
        UbisoftName = player.UbisoftName;
        Zone = player.Zone;
        Groups = player.Groups;
    }
}
