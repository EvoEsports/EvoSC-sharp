using System;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Players;

namespace EvoSC.Core.Events.Callbacks.Args;

public class PlayerChatEventArgs : EventArgs
{
    public PlayerChatEventArgs(IServerServerChatMessage message)
    {
        Player = message.Player;
        Message = message;
    }

    public IPlayer Player { get; }
    public IServerServerChatMessage Message { get; }
}
