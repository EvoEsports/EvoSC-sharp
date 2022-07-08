using EvoSC.Core.Commands.Chat.Interfaces;
using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Chat;
using GbxRemoteNet;

namespace EvoSC.Core.Commands.Chat;

public class ChatCommandContext : IChatCommandContext
{
    public GbxRemoteClient Client { get; }
    public DatabasePlayer DatabasePlayer { get; }
    public IServerChatMessage Message { get; }

    public ChatCommandContext(GbxRemoteClient client, DatabasePlayer databasePlayer, IServerChatMessage message)
    {
        Client = client;
        DatabasePlayer = databasePlayer;
        Message = message;
    }
}
