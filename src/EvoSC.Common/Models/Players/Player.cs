using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models.Players;

public class Player : IPlayer
{
    public long Id { get; init; }
    public string AccountId { get; init; }
    public string NickName { get; init; }
    public string UbisoftName { get; init; }
    public string? Zone { get; init; }
    public IPlayerSettings Settings { get; set; }
    public IEnumerable<IGroup> Groups { get; set; }
    public IGroup? DisplayGroup { get; set; }

    public Player()
    {
    }

    public Player(DbPlayer dbPlayer) : this()
    {
        Id = dbPlayer.Id;
        AccountId = dbPlayer.AccountId;
        NickName = dbPlayer.NickName;
        UbisoftName = dbPlayer.UbisoftName;
        Zone = dbPlayer.Zone;
        Settings = dbPlayer.Settings;
        Groups = dbPlayer.Groups;
        DisplayGroup = dbPlayer.DisplayGroup;
    }
    
    public bool Equals(IPlayer? other) => other != null && AccountId.Equals(other.AccountId, StringComparison.Ordinal);
}
