using System;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Interfaces.Messages;

namespace EvoSC.Core.Events.Callbacks;

public class ChatCallbacks : IChatCallbacks
{
    public event EventHandler<PlayerChatEventArgs> PlayerChat;

    public void OnPlayerChat(PlayerChatEventArgs e)
    {
        PlayerChat?.Invoke(this, e);
    }
}
