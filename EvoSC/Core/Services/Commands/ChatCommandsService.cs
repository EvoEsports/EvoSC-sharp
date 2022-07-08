using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using EvoSC.Attributes;
using EvoSC.Domain.Commands;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Commands;

namespace EvoSC.Core.Services.Commands;

public class ChatCommandsService : IChatCommandsService
{
    private readonly Dictionary<string, Command> _commands = new();

    public Task ClientOnPlayerChatCommand(Player player, Command command)
    {
        throw new NotImplementedException();
    }

    public void RegisterCommands(Type type)
    {
        throw new NotImplementedException();
    }

    public void UnregisterCommands(Type type)
    {
        throw new NotImplementedException();
    }
}
