using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;

namespace EvoSC.Common.Models;

public class OnlinePlayer : IOnlinePlayer
{
    public required long Id { get; init; }
    public string AccountId { get; set; }
    public string NickName { get; set; }
    public string UbisoftName { get; set; }
    public string Zone { get; set; }
    public IEnumerable<IGroup> Groups { get; }
    public PlayerState State { get; set; }
}
