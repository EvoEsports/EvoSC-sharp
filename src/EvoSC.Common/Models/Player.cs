using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models;

public class Player : IPlayer
{
    public long Id { get; init; }
    public string AccountId { get; init; }
    public string NickName { get; init; }
    public string UbisoftName { get; init; }
    public string? Zone { get; init; }
    public IEnumerable<IGroup> Groups { get; }

    public Player()
    {
        Groups = new List<IGroup>();
    }

    public Player(DbPlayer dbPlayer) : this()
    {
        Id = dbPlayer.Id;
        AccountId = dbPlayer.AccountId;
        NickName = dbPlayer.NickName;
        UbisoftName = dbPlayer.UbisoftName;
        Zone = dbPlayer.Zone;
        Groups = dbPlayer.Groups;
    }
}
