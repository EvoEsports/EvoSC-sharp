using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Events.Arguments;

public class PlayerJoinedEventArgs : EventArgs
{
    public required IOnlinePlayer Player { get; init; }
    public required bool IsPlayerListUpdate { get; init; }
    public required bool IsNewPlayer { get; init; }
}
