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
    
    public ExampleController(IMySettings settings, IChatCommandManager cmds)
    {
        _settings = settings;
    }
    
    [ChatCommand("tmx add", "Add map from TMX.")]
    [CommandAlias("//addmap")]
    public Task TmxAddMap([Description("The TMX ID of the map to add.")] int mxId)
    {
        return Task.CompletedTask;
    }
}
