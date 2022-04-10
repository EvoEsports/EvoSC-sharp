using System;
using EvoSC.Domain.Players;

namespace EvoSC.Core.Events.Callbacks.Args;

public class PlayerDisconnectEventArgs : EventArgs
{
    public Player Player { get; }
    public string Reason { get; }

    public PlayerDisconnectEventArgs(Player player, string reason)
    {
        Player = player;
        Reason = reason;
    }
}
