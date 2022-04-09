using System;
using System.Threading.Tasks;
using EvoSC.Core.Events.Args;

namespace EvoSC.Core.Events.Callbacks;

public class PlayerCallbacks
{
    public event EventHandler<PlayerConnectEventArgs> PlayerConnect;
    public event EventHandler<PlayerDisconnectEventArgs> PlayerDisconnect;

    public Task OnPlayerConnect(PlayerConnectEventArgs e)
    {
        PlayerConnect?.Invoke(null, e);
        return Task.CompletedTask;
    }

    public Task OnPlayerDisconnect(PlayerDisconnectEventArgs e)
    {
        PlayerDisconnect?.Invoke(null, e);
        return Task.CompletedTask;
    }
}
