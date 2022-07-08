using EvoSC.Core.Commands.Chat.Interfaces;
using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Chat;
using GbxRemoteNet;

namespace EvoSC.Core.Commands.Chat;

public class ChatCommandContext : IChatCommandContext
{
    public GbxRemoteClient Client { get; }
    public Player Player { get; }
    public IServerChatMessage Message { get; }

    public ChatCommandContext(GbxRemoteClient client, Player player, IServerChatMessage message)
    {
        Client = client;
        Player = player;
        Message = message;
    }
}
