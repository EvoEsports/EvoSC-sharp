using EvoSC.Interfaces;
using EvoSC.Interfaces.Chat;
using GbxRemoteNet;

namespace EvoSC.Core.Events.GbxEventHandlers;

public class ChatGbxEventHandler : IGbxEventHandler
{
    private readonly IChatService _chatService;

    public ChatGbxEventHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public void HandleEvents(GbxRemoteClient client)
    {
        client.OnPlayerChat += _chatService.ClientOnPlayerChat;
    }
}
