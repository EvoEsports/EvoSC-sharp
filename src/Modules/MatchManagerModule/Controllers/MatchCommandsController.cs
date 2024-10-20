using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Permissions;

namespace EvoSC.Modules.Official.MatchManagerModule.Controllers;

[Controller]
public class MatchCommandsController(IMatchControlService matchControl, Locale locale)
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
        await Context.Chat.InfoMessageAsync(_locale.RestartedMatch(Context.Player.NickName));
        
        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.RestartMatch)
            .Comment(_locale.Audit_MatchRestarted);
    }

    [ChatCommand("endround", "[Command.EndRound]", MatchControlPermissions.EndRound)]
    public async Task EndRoundAsync()
    {
        await matchControl.EndRoundAsync();
        await Context.Chat.InfoMessageAsync(_locale.ForcedRoundEnd(Context.Player.NickName));
        
        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.EndRound)
            .Comment(_locale.Audit_RoundEnded);
    }

    [ChatCommand("skipmap", "[Command.Skip]", MatchControlPermissions.SkipMap)]
    [CommandAlias("/skip", hide: true)]
    public async Task SkipMapAsync()
    {
        await matchControl.SkipMapAsync();
        await Context.Chat.InfoMessageAsync(_locale.SkippedToNextMap(Context.Player.NickName));
        
        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.SkipMap)
            .Comment(_locale.Audit_MapSkipped);
    }

    [ChatCommand("setteamroundpoints", "Set the round points of a team.", MatchControlPermissions.SetTeamPoints)]
    public async Task SetRoundPointsAsync(int team, int points)
    {
        var playerTeam = team == 0 ? PlayerTeam.Team1 : PlayerTeam.Team2;
        await matchControl.SetTeamRoundPointsAsync(playerTeam, points);

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.TeamRoundPointsSet)
            .HavingProperties(new { Points = points, Team = playerTeam });

        await Context.Chat.SuccessMessageAsync($"Round points for team {playerTeam} was set to {points}.",Context.Player);
    }

    [ChatCommand("setteampoints", "Set the round, map and match points of a team.", MatchControlPermissions.SetTeamPoints)]
    public async Task SetPointsAsync(int team, int points)
    {
        var playerTeam = team == 0 ? PlayerTeam.Team1 : PlayerTeam.Team2;
        await matchControl.SetTeamPointsAsync(playerTeam, points);

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.TeamPointsSet)
            .HavingProperties(new { Points = points, Team = playerTeam });

        await server.SuccessMessageAsync(Context.Player, $"Points for team {playerTeam} was set to {points}.");
    }
    
    [ChatCommand("setteammappoints", "Set the map points of a team.", MatchControlPermissions.SetTeamPoints)]
    public async Task SetMapPointsAsync(int team, int points)
    {
        var playerTeam = team == 0 ? PlayerTeam.Team1 : PlayerTeam.Team2;
        await matchControl.SetTeamMapPointsAsync(playerTeam, points);

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.TeamMapPointsSet)
            .HavingProperties(new { Points = points, Team = playerTeam });
        
        await Context.Chat.SuccessMessageAsync($"Map points for team {playerTeam} was set to {points}.", Context.Player);
    }
    
    [ChatCommand("setteammatchpoints", "Set the match points of a team.", MatchControlPermissions.SetTeamPoints)]
    public async Task SetMatchPointsAsync(int team, int points)
    {
        var playerTeam = team == 0 ? PlayerTeam.Team1 : PlayerTeam.Team2;
        await matchControl.SetTeamMatchPointsAsync(playerTeam, points);

        Context.AuditEvent.Success()
            .WithEventName(AuditEvents.TeamMatchPointsSet)
            .HavingProperties(new { Points = points, Team = playerTeam });
        
        await Context.Chat.SuccessMessageAsync($"Match points for team {playerTeam} was set to {points}.", Context.Player);
    }

    [ChatCommand("pause", "Pause the current match.", MatchControlPermissions.PauseMatch)]
    public async Task PauseMatchAsync()
    {
        await matchControl.PauseMatchAsync();
        Context.AuditEvent.Success().WithEventName(AuditEvents.MatchPaused);
    }
    
    [ChatCommand("unpause", "Unpause the current match.", MatchControlPermissions.PauseMatch)]
    public async Task UnpauseMatchAsync()
    {
        await matchControl.UnpauseMatchAsync();
        Context.AuditEvent.Success().WithEventName(AuditEvents.MatchUnpaused);
    }
}
