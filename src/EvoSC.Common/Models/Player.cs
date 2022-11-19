using EvoSC.Common.Database.Models;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models;

public class Player : IPlayer
{
    public required long Id { get; init; }
    public required string AccountId { get; init; }
    public required string NickName { get; init; }
    public required string UbisoftName { get; init; }
    public string? Zone { get; init; }
    public IEnumerable<IGroup> Groups { get; }

    public Player()
    {
        Groups = new List<IGroup>();
    }

    public Player(DbPlayer dbPlayer) : this()
    {
        Id = dbPlayer.Id;
    }
}
