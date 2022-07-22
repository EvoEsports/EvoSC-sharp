using EvoSC.Interfaces;
using EvoSC.Interfaces.Messages;
using GbxRemoteNet;

namespace EvoSC.Core.Events.GbxEventHandlers;

public class ChatGbxEventHandler : IGbxEventHandler
{
    private readonly IChatService _chatService;
    private readonly IChatCommandsService _commandsService;

    public ChatGbxEventHandler(IChatService chatService, IChatCommandsService commandsService)
    {
        _chatService = chatService;
        _commandsService = commandsService;
    }

    public void HandleEvents(GbxRemoteClient client)
    {
        client.OnPlayerChat += _chatService.ClientOnPlayerChat;
        _chatService.ServerChatMessage += _commandsService.OnChatMessage;
    }
}
