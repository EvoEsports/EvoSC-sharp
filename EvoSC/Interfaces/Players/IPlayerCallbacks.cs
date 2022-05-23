using System;
using EvoSC.Core.Events.Callbacks.Args;

namespace EvoSC.Interfaces.Players;

public interface IPlayerCallbacks
{
    public event EventHandler<PlayerConnectEventArgs> PlayerConnect;

    public event EventHandler<PlayerDisconnectEventArgs> PlayerDisconnect;

    public void OnPlayerConnect(PlayerConnectEventArgs e);

    public void OnPlayerDisconnect(PlayerDisconnectEventArgs e);
}
