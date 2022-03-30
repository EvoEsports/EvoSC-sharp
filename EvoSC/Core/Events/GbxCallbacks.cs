using System;
using System.Threading.Tasks;
using EvoSC.Core.Events.Args;

namespace EvoSC.Core.Events;

public class GbxCallbacks
{
    public event EventHandler<PlayerConnectEventArgs> PlayerConnect;
    public event EventHandler<PlayerDisconnectEventArgs> PlayerDisconnect;
    public event EventHandler<PlayerChatEventArgs> PlayerChat;

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

    public Task OnPlayerChat(PlayerChatEventArgs e)
    {
        PlayerChat?.Invoke(null, e);
        return Task.CompletedTask;
    }
}
