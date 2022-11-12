using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;

namespace EvoSC.Common.Models;

public class OnlinePlayer : IOnlinePlayer
{
    public string AccountId { get; set; }
    public string NickName { get; set; }
    public string UbisoftName { get; set; }
    public string Zone { get; set; }
    public PlayerState State { get; set; }
}
