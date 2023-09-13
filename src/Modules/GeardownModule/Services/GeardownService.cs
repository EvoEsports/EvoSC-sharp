using System.Text.Json;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Services;
using EvoSC.Modules.Evo.GeardownModule.Models;
using EvoSC.Modules.Evo.GeardownModule.Models.API;
using EvoSC.Modules.Evo.GeardownModule.Settings;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using MatchStatus = EvoSC.Modules.Official.MatchTrackerModule.Models.MatchStatus;

namespace EvoSC.Modules.Evo.GeardownModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class GeardownService : IGeardownService
{
    private readonly IMatchTracker _matchTracker;
    private readonly IGeardownSetupService _setupService;
    private readonly IGeardownApiService _geardownApi;
    private readonly IAuditService _audits;
    private readonly IServerClient _server;
    private readonly IPlayerReadyService _playerReadyService;
    private readonly IGeardownSettings _settings;
    private readonly IGeardownSetupStateService _setupState;

    public GeardownService(IMatchTracker matchTracker, IGeardownSetupService setupService,
        IGeardownApiService geardownApi, IAuditService auditService, IServerClient server,
        IPlayerReadyService playerReadyService, IGeardownSettings settings, IGeardownSetupStateService setupState)
    {
        _matchTracker = matchTracker;
        _setupService = setupService;
        _geardownApi = geardownApi;
        _audits = auditService;
        _server = server;
        _playerReadyService = playerReadyService;
        _settings = settings;
        _setupService = setupService;
        _setupState = setupState;
    }

    public async Task SetupServerAsync(int matchId)
    {
        var (match, token) = await _setupService.InitialSetupAsync(matchId);
        
        _audits.NewInfoEvent("Geardown.ServerSetup")
            .HavingProperties(new { Match = match, MatchToken = token })
            .Comment("Server was setup through geardown.");
    }

    public Task FinishServerSetupAsync() => _setupService.FinishSetupAsync();

    public async Task StartMatchAsync()
    {
        var matchTrackerId = await _matchTracker.BeginMatchAsync();
        await _server.Remote.RestartMapAsync();
        
        await _playerReadyService.SetWidgetEnabled(false);

        await _server.InfoMessageAsync("Match is about to begin ...");
        
        var matchState = JsonSerializer.Deserialize<GeardownMatchState>(_settings.MatchState);

        if (matchState == null)
        {
            throw new InvalidOperationException("Failed to obtain current match state from settings.");
        }
        
        _setupState.SetMatchStarted();
        await _geardownApi.Matches.OnStartMatchAsync(matchState.MatchToken);
        
        _audits.NewInfoEvent("Geardown.StartMatch")
            .HavingProperties(new { MatchTrackingId = matchTrackerId })
            .Comment("Match was started.");
    }

    public async Task EndMatchAsync(IMatchTimeline timeline)
    {
        await _playerReadyService.SetWidgetEnabled(false);
        await SendResultsAsync(timeline);

        await _server.SuccessMessageAsync("Match finished, thanks for playing!");
    }

    public MatchStatus GetMatchStatus()
    {
        return _matchTracker.Status;
    }

    private async Task SendResultsAsync(IMatchTimeline timeline)
    {
        var matchState = JsonSerializer.Deserialize<GeardownMatchState>(_settings.MatchState);

        if (matchState?.Match?.participants?.FirstOrDefault()?.user?.tm_account_id == null)
        {
            throw new InvalidOperationException("The match state is invalid, failed to send results.");
        }
        
        var results = timeline.States.LastOrDefault(s => s.Status == MatchStatus.Running) as IScoresMatchState;

        if (results == null || results.Section != ModeScriptSection.EndMatch)
        {
            throw new InvalidOperationException("Did not get a match end result to send to geardown.");
        }

        await _geardownApi.Matches.AddResultsAsync((int)matchState.Match.id,
        results.Players.Select(r => new GdResult
        {
            nickname = r.Player.UbisoftName, 
            score = r.MatchPoints
        }));

        await _geardownApi.Matches.OnEndMatchAsync(matchState.MatchToken);
    }
}
