using System.ComponentModel;
using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
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

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class ExampleController : EvoScController<IControllerContext>
{
    private readonly IMySettings _settings;
    private readonly IServerClient _server;
    
    public ExampleController(IMySettings settings, IChatCommandManager cmds, IServerClient server)
    {
        _settings = settings;
        _server = server;
    }
    
    [ChatCommand("hey", "Add map from TMX.")]
    [CommandAlias("//addmap")]
    public async Task TmxAddMap([Description("The TMX ID of the map to add.")] string mxId)
    {
        await _server.SendChatMessage($"hello, {mxId}!");
    }
}
