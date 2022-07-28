using System;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Players;

namespace EvoSC.Core.Events.Callbacks.Args;

public class PlayerConnectEventArgs : EventArgs
{
    private IPlayer Player { get; }

    public PlayerConnectEventArgs(IPlayer databasePlayer)
    {
        Player = databasePlayer;
    }
}
