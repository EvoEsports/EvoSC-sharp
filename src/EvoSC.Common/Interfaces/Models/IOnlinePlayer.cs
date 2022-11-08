using EvoSC.Common.Interfaces.Models.Enums;

namespace EvoSC.Common.Interfaces.Models;

public interface IOnlinePlayer : IPlayer
{
    public PlayerState State { get; }
}
