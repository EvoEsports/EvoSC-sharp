using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Chat;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;

namespace EvoSC.Core.Commands.Chat.Interfaces;

public interface IChatCommandContext : ICommandContext
{
    public IServerPlayer Player { get; }
    public IServerServerChatMessage Message { get;}
}
