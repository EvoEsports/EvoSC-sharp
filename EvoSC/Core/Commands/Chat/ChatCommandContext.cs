using EvoSC.Core.Commands.Chat.Interfaces;
using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using Microsoft.AspNetCore.Hosting.Server;

namespace EvoSC.Core.Commands.Chat;

public class ChatCommandContext : IChatCommandContext
{
    public GbxRemoteClient Client { get; }
    public IPlayer Player { get; }

    public IServerServerChatMessage Message { get; }

    public ChatCommandContext(GbxRemoteClient client, IServerServerChatMessage message)
    {
        Client = client;
        Player = message.Player;
        Message = message;
    }
}
