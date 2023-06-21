using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Permissions;

namespace EvoSC.Modules.Official.MatchManagerModule.Controllers;

[Controller]
public class FlowControlCommands : EvoScController<CommandInteractionContext>
{
    private readonly IFlowControlService _flowControl;
    private readonly IServerClient _server;
    private readonly dynamic _locale;

    public FlowControlCommands(IFlowControlService flowControl, IServerClient server, ILocale locale)
    {
        _flowControl = flowControl;
        _server = server;
        _locale = locale;
    }

    [ChatCommand("restartmatch", "Restart the current match.", FlowControlPermissions.RestartMatch)]
    [CommandAlias("/resmatch", hide: true)]
    public async Task RestartMatchAsync()
    {
        await _flowControl.RestartMatchAsync();
        await _server.InfoMessageAsync(_locale.RestartedMatch(Context.Player.NickName));
    }

    [ChatCommand("endround", "Force end the current round.", FlowControlPermissions.EndRound)]
    public async Task EndRoundAsync()
    {
        await _flowControl.EndRoundAsync();
        await _server.InfoMessageAsync(_locale.ForcedRoundEnd(Context.Player.NickName));
    }

    [ChatCommand("skipmap", "Skip to the next map.", FlowControlPermissions.SkipMap)]
    [CommandAlias("/skip", hide: true)]
    public async Task SkipMapAsync()
    {
        await _flowControl.SkipMapAsync();
        await _server.InfoMessageAsync(_locale.SkippedToNextMap(Context.Player.NickName));
    }
}
