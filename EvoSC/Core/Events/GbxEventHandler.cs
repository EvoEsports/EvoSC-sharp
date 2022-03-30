using EvoSC.Core.Events.Args;
using GbxRemoteNet;

namespace EvoSC.Core.Events;

public class GbxEventHandler
{
    private readonly GbxCallbacks _callbacks = new GbxCallbacks();
    
    public void HandleEvents(GbxRemoteClient client)
    {
        client.OnPlayerConnect += (login, spectator) => _callbacks.OnPlayerConnect(new PlayerConnectEventArgs(login, spectator));
        client.OnPlayerDisconnect += (login, reason) => _callbacks.OnPlayerDisconnect(new PlayerDisconnectEventArgs(login, reason));
        client.OnPlayerChat += (uid, login, text, cmd) => _callbacks.OnPlayerChat(new PlayerChatEventArgs(uid, login, text, cmd));
    }
}
