using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models;

public class Player : IPlayer
{
    public string AccountId { get; set; }
    public string NickName { get; set; }
    public string UbisoftName { get; set; }
    public string Zone { get; set; }
}
