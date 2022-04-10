using System;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Interfaces.Players;

namespace EvoSC.Core.Events.Callbacks;

public class PlayerCallbacks : IPlayerCallbacks
{
    public event EventHandler<PlayerConnectEventArgs> PlayerConnect;
    public event EventHandler<PlayerDisconnectEventArgs> PlayerDisconnect;

    public virtual void OnPlayerConnect(PlayerConnectEventArgs e)
    {
        PlayerConnect?.Invoke(this, e);
    }

    public virtual void OnPlayerDisconnect(PlayerDisconnectEventArgs e)
    {
        PlayerDisconnect?.Invoke(this, e);
    }
}
