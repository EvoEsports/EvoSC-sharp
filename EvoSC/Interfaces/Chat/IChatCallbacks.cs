using System;
using EvoSC.Core.Events.Callbacks.Args;

namespace EvoSC.Interfaces.Chat;

public interface IChatCallbacks
{
    public event EventHandler<PlayerChatEventArgs> PlayerChat;

    public void OnPlayerChat(PlayerChatEventArgs e);
}
