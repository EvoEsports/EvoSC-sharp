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
public class MatchCommandsController(IMatchControlService matchControl, IServerClient server, Locale locale)
    : EvoScController<ICommandInteractionContext>
{
    private readonly dynamic _locale = locale;

    [ChatCommand("startmatch", "Start a match.", MatchControlPermissions.StartMatch)]
    public async Task StartMatchAsync()
    {
        await matchControl.StartMatchAsync();

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MatchStarted)
            .Comment("Match was started.");
    }
    
    [ChatCommand("endmatch", "End a match.", MatchControlPermissions.EndMatch)]
    public async Task EndMatchAsync()
    {
        await matchControl.EndMatchAsync();

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.MatchEnded)
            .Comment("Match was ended.");
    }
    
    [ChatCommand("restartmatch", "[Command.RestartMatch]", MatchControlPermissions.RestartMatch)]
    [CommandAlias("/resmatch", hide: true)]
    public async Task RestartMatchAsync()
    {
        await matchControl.RestartMatchAsync();
        await server.InfoMessageAsync(_locale.RestartedMatch(Context.Player.NickName));
        
        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.RestartMatch)
            .Comment(_locale.Audit_MatchRestarted);
    }

    [ChatCommand("endround", "[Command.EndRound]", MatchControlPermissions.EndRound)]
    public async Task EndRoundAsync()
    {
        await matchControl.EndRoundAsync();
        await server.InfoMessageAsync(_locale.ForcedRoundEnd(Context.Player.NickName));
        
        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.EndRound)
            .Comment(_locale.Audit_RoundEnded);
    }

    [ChatCommand("skipmap", "[Command.Skip]", MatchControlPermissions.SkipMap)]
    [CommandAlias("/skip", hide: true)]
    public async Task SkipMapAsync()
    {
        await matchControl.SkipMapAsync();
        await server.InfoMessageAsync(_locale.SkippedToNextMap(Context.Player.NickName));
        
        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.SkipMap)
            .Comment(_locale.Audit_MapSkipped);
    }
}
