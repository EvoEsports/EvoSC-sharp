using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Chat;
using GbxRemoteNet;

namespace EvoSC.Core.Commands.Chat.Interfaces;

public interface IChatCommandContext : ICommandContext
{
    public Player Player { get; }
    public IServerChatMessage Message { get;}
}
