using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Permissions;

namespace EvoSC.Modules.Official.MatchManagerModule.Controllers;

[Controller]
public class MatchCommandsController : EvoScController<ICommandInteractionContext>
{
    private readonly IMatchControlService _matchControl;
    private readonly IServerClient _server;
    private readonly dynamic _locale;

    public MatchCommandsController(IMatchControlService matchControl, IServerClient server, Locale locale)
    {
        _matchControl = matchControl;
        _server = server;
        _locale = locale;
    }

    [ChatCommand("startmatch", "Start a match.", MatchControlPermissions.StartMatch)]
    public async Task StartMatchAsync()
    {
        var trackingId = await _matchControl.StartMatch();

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MatchStarted)
            .HavingProperties(new { TrackingId =  trackingId })
            .Comment("Match was started.");
    }
    
    [ChatCommand("endmatch", "End a match.", MatchControlPermissions.EndMatch)]
    public async Task EndMatchAsync()
    {
        var trackingId = await _matchControl.EndMatch();

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MatchEnded)
            .HavingProperties(new {TrackingId = trackingId})
            .Comment("Match was ended.");
    }
    
    [ChatCommand("restartmatch", "[Command.RestartMatch]", MatchControlPermissions.RestartMatch)]
    [CommandAlias("/resmatch", hide: true)]
    public async Task RestartMatchAsync()
    {
        await _matchControl.RestartMatchAsync();
        await _server.InfoMessageAsync(_locale.RestartedMatch(Context.Player.NickName));
        
        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.RestartMatch)
            .Comment(_locale.Audit_MatchRestarted);
    }

    [ChatCommand("endround", "[Command.EndRound]", MatchControlPermissions.EndRound)]
    public async Task EndRoundAsync()
    {
        await _matchControl.EndRoundAsync();
        await _server.InfoMessageAsync(_locale.ForcedRoundEnd(Context.Player.NickName));
        
        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.EndRound)
            .Comment(_locale.Audit_RoundEnded);
    }

    [ChatCommand("skipmap", "[Command.Skip]", MatchControlPermissions.SkipMap)]
    [CommandAlias("/skip", hide: true)]
    public async Task SkipMapAsync()
    {
        await _matchControl.SkipMapAsync();
        await _server.InfoMessageAsync(_locale.SkippedToNextMap(Context.Player.NickName));
        
        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.SkipMap)
            .Comment(_locale.Audit_MapSkipped);
    }
}
