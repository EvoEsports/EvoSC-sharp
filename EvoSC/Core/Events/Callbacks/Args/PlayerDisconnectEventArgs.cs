using System;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Players;

namespace EvoSC.Core.Events.Callbacks.Args;

public class PlayerDisconnectEventArgs : EventArgs
{
    public IPlayer Player { get; }

    public string Reason { get; }

    public PlayerDisconnectEventArgs(IPlayer player, string reason)
    {
        Player = player;
        Reason = reason;
    }
}
