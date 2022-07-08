using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Chat;
using EvoSC.Core.Commands.Generic;
using EvoSC.Core.Commands.Generic.Attributes;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Chat;
using EvoSC.Interfaces.Commands;
using Scriban.Parsing;

namespace EvoSC.Core.Services.Commands;

public class ChatCommandsService : CommandsManager<ChatCommandGroup>, IChatCommandsService
{
    public ChatCommandsService(IServiceProvider services) : base(services)
    {
    }

    public Task OnChatMessage(IServerServerChatMessage message)
    {
        Console.WriteLine(message.Content);
        return Task.CompletedTask;
    }
}
