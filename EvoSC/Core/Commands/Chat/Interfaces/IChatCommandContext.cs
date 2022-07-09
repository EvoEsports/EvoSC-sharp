using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;

namespace EvoSC.Core.Commands.Chat.Interfaces;

public interface IChatCommandContext : ICommandContext
{
    public IServerServerChatMessage Message { get;}
}
