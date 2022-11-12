using System.ComponentModel;
using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;
using Container = SimpleInjector.Container;

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class ExampleController : EvoScController<PlayerInteractionContext>
{
    private readonly IMySettings _settings;
    private readonly IServerClient _server;
    private readonly IChatCommandManager _chatCommands;

    public ExampleController(IMySettings settings, IChatCommandManager cmds, IServerClient server, IChatCommandManager chatCommands)
    {
        _settings = settings;
        _server = server;
        _chatCommands = chatCommands;
    }
    
    [ChatCommand("hey", "Say hey!")]
    public async Task TmxAddMap(string name)
    {
        await _server.SendChatMessage($"hello, {name}!", Context.Player);
    }

    [ChatCommand("ratemap", "Rate the current map.")]
    [CommandAlias("+++", 100)]
    [CommandAlias("++", 80)]
    [CommandAlias("+", 60)]
    [CommandAlias("-", 40)]
    [CommandAlias("--", 20)]
    [CommandAlias("---", 0)]
    public async Task RateMap(int rating)
    {
        if (rating < 0 || rating > 100)
        {
            await _server.SendChatMessage("Rating must be between 0 and 100 inclusively.", Context.Player);
        }
        else
        {
            await _server.SendChatMessage($"Your rating: {rating}");
        }
    }

    [ChatCommand("test", "Some testing.")]
    [CommandAlias("testAlias", 10)]
    public async Task TestCommand(int arg1, int arg2)
    {
        await _server.SendChatMessage($"Args: {arg1}, {arg2}", Context.Player);
    }
}
