using System.Dynamic;
using System.Runtime.Serialization;
using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Audit;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Audit;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Util.Auditing;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Permissions;

namespace EvoSC.Modules.Official.MatchManagerModule.Controllers;

[Controller]
public class FlowControlCommands : EvoScController<CommandInteractionContext>
{
    private readonly IFlowControlService _flowControl;
    private readonly IServerClient _server;
    private readonly IAuditService _audit;

    public FlowControlCommands(IFlowControlService flowControl, IServerClient server, IAuditService audit)
    {
        _flowControl = flowControl;
        _server = server;
        _audit = audit;
    }

    [ChatCommand("restartmatch", "Restart the current match.", FlowControlPermissions.RestartMatch)]
    [CommandAlias("/resmatch", hide: true)]
    public async Task RestartMatchAsync()
    {
        await _flowControl.RestartMatchAsync();
        await _server.InfoMessageAsync($"{Context.Player.NickName} restarted the match.");
    }

    [ChatCommand("endround", "Force end the current round.", FlowControlPermissions.EndRound)]
    public async Task EndRoundAsync()
    {
        await _flowControl.EndRoundAsync();
        await _server.InfoMessageAsync($"{Context.Player.NickName} forced the round to end.");
    }

    [ChatCommand("skipmap", "Skip to the next map.", FlowControlPermissions.SkipMap)]
    [CommandAlias("/skip", hide: true)]
    public async Task SkipMapAsync()
    {
        await _flowControl.SkipMapAsync();
        await _server.InfoMessageAsync($"{Context.Player.NickName} skipped to the next map.");
    }
}
