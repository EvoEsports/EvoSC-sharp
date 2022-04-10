using System;
using EvoSC.Domain.Players;

namespace EvoSC.Core.Events.Callbacks.Args;

public class PlayerConnectEventArgs: EventArgs
{
    private Player Player { get; }

    public PlayerConnectEventArgs(Player player)
    {
        Player = player;
    }
}
