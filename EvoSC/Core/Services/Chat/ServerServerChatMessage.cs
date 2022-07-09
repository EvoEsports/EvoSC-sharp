using System;
using System.Threading.Tasks;
using EvoSC.Core.Helpers;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;

namespace EvoSC.Core.Services.Chat;

public class ServerServerChatMessage : IServerServerChatMessage
{
    public IServerPlayer Player { get; }
    public string Content { get; }
    public int PlayerServerId { get; }
    /// <summary>
    /// Whether the message is an attempted command.
    /// </summary>
    public bool IsCommand { get; }
    /// <summary>
    /// Whether the message came from the server itself.
    /// </summary>
    public bool IsServer { get; }

    private GbxRemoteClient _client;

    public ServerServerChatMessage(GbxRemoteClient client, IServerPlayer player, string content, int playerServerId)
    {
        Player = player;
        Content = content;
        PlayerServerId = playerServerId;
        IsCommand = content.StartsWith('/');
        _client = client;
    }

    public Task ReplyAsync(string message)
    {
        if (IsServer)
        {
            throw new InvalidOperationException("Cannot reply to server");
        }

        return _client.ChatSendServerMessageToIdAsync(message, PlayerServerId);
    }
    
    public Task ReplyAsync(ChatMessage message) =>
        ReplyAsync(message.Render());
}
