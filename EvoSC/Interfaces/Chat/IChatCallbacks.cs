using System;
using EvoSC.Core.Events.Callbacks.Args;

namespace EvoSC.Interfaces.Messages;

public interface IChatCallbacks
{
    public event EventHandler<PlayerChatEventArgs> PlayerChat;

    public void OnPlayerChat(PlayerChatEventArgs e);
}
