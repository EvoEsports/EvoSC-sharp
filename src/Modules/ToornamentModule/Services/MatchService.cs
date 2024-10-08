using System.Data;
using System.Text;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Models;
using EvoSC.Modules.EvoEsports.ToornamentModule.Settings;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using Microsoft.Extensions.Logging;
using NATS.Client.JetStream;
using ToornamentApi.Models.Api.TournamentApi;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchService(
    IAuditService auditService,
    IServerClient server,
    IManialinkManager manialinkManager,
    IToornamentSettings settings,
    IPlayerManagerService playerManagerService,
    IPlayerReadyService playerReadyService,
    IPlayerReadyTrackerService playerReadyTrackerService,
    IStateService stateService,
    IMatchSettingsService matchSettings,
    IMatchTracker matchTracker,
    IToornamentService toornamentService,
    IKeyValueStoreService keyValueStoreService,
    ILogger<MatchService> logger,
    IMatchSettingsCreatorService matchSettingsCreatorService,
    IMatchMapService matchMapService,
    IWhitelistService whitelistService,
    IMatchPlayerService matchPlayerService
) : IMatchService
{
    public async Task StartMatchAsync()
    {
        logger.LogDebug("Begin of StartMatchAsync()");
        // create a new timeline and get the tracking ID
        var matchTrackerId = await matchTracker.BeginMatchAsync();

        await matchSettings.LoadMatchSettingsAsync(stateService.MatchSettingsName, false);
        await server.Remote.RestartMapAsync();

        // disable the ready widget
        await playerReadyService.SetWidgetEnabled(false);

        await server.Chat.InfoMessageAsync("Match is live after warmup. GLHF! ");

        stateService.SetMatchStarted();

        await whitelistService.KickNonWhitelistedPlayers();

        // notify Toornament that the match has started
        await toornamentService.SetMatchGameStatusAsync(settings.AssignedMatchId, 1, MatchGameStatus.Running);

        auditService.NewInfoEvent("Toornament.StartMatch")
            .HavingProperties(new { MatchTrackingId = matchTrackerId })
            .Comment("Match was started.");

        logger.LogDebug("End of StartMatchAsync()");
    }

    public async Task EndMatchAsync(ScoresEventArgs timeline)
    {
        if (stateService.WaitingForMatchStart || stateService.IsInitialSetup || stateService.SetupFinished ||
            stateService.MatchEnded)
        {
            logger.LogDebug("Match hasn't been run or completed yet. Not updating MatchGame map on Toornament.");
            return;
        }

        logger.LogDebug("Begin of EndMatchAsync()");

        if (timeline is not { Section: ModeScriptSection.EndMatch })
        {
            throw new InvalidOperationException("Did not get a match end result to send to Toornament.");
        }

        //TODO check if match is needed to get Opponents.Players or if MatchGame includes this info as well.
        var match = await toornamentService.GetMatchAsync(settings.AssignedMatchId);
        var matchGameNumber = 1;
        var matchGame = await toornamentService.GetMatchGameAsync(settings.AssignedMatchId, matchGameNumber);

        List<OpponentInfo> toornamentScores = new List<OpponentInfo>();

        int bottomRank = match.Opponents.Length;

        foreach (var opponent in match.Opponents)
        {
            var matchOpponent = opponent;
            if (matchOpponent is not null)
            {
                PlayerScore? player = null;
                if (matchOpponent.Participant is not null)
                {
                    var tmId = matchOpponent.Participant.CustomFields["trackmania_id"];
                    if (tmId is not null)
                    {
                        player = timeline.Players.FirstOrDefault(p => p.AccountId == tmId.ToString());
                    }

                    if (player is null)
                    {
                        //try to find player by ubisoft name
                        player = timeline.Players.FirstOrDefault(p => p.Name == matchOpponent.Participant.Name);
                    }
                }

                // The player showed up in the match and has a score
                if (player is not null)
                {
                    matchOpponent.Rank = player.Rank;
                    matchOpponent.Score = player.MatchPoints;
                }

                // The player didn't show up for the match
                else
                {
                    matchOpponent.Forfeit = true;
                    matchOpponent.Score = 0;
                }

                toornamentScores.Add(matchOpponent);
            }
        }

        //override the scores in the MatchGame object
        matchGame.Opponents = toornamentScores.ToArray();
        matchGame.Status = MatchGameStatus.Completed.ToString().ToLower();
        await toornamentService.SetMatchGameResultAsync(settings.AssignedMatchId, matchGameNumber, matchGame);

        //Set GameMode back to the 1h warmup gamemode.
        await matchSettings.LoadMatchSettingsAsync(stateService.MatchSettingsName + "_warmup", false);
        await server.Remote.RestartMapAsync();

        stateService.SetMatchEnded();

        await server.Chat.SuccessMessageAsync("Match finished, thanks for playing!");
        logger.LogDebug("End of EndMatchAsync()");
    }

    public async Task SetMatchGameMapAsync()
    {
        logger.LogDebug("Begin of SetMatchGameMapAsync()");
        if (!stateService.MatchInProgress)
        {
            logger.LogDebug("Match hasn't started yet. Not updating MatchGame map on Toornament.");
            return;
        }

        var currentMap = await server.Remote.GetCurrentMapInfoAsync();

        if (currentMap is null)
        {
            logger.LogWarning("Current map could not be determined");
            return;
        }

        if (string.IsNullOrEmpty(settings.MapMachineNames))
        {
            logger.LogWarning("No map names defined in environment");
            return;
        }

        var mapMachineNames = settings.MapMachineNames.Split(',');

        var toornamentMapName = "";

        foreach (var name in mapMachineNames)
        {
            if (currentMap.Name.ToLowerInvariant().Contains(name))
            {
                toornamentMapName = name;
                break;
            }
        }

        if (!string.IsNullOrEmpty(toornamentMapName))
        {
            await toornamentService.SetMatchGameMapAsync(settings.AssignedMatchId, 1, toornamentMapName);
        }

        logger.LogDebug("End of SetMatchGameMapAsync()");
    }

    public async Task<bool> SetServerNameAsync(string name)
    {
        return await server.Remote.SetServerNameAsync(name);
    }

    public async Task ShowSetupScreenAsync(IPlayer player, string selectedTournamentId, string selectedStageId)
    {
        logger.LogDebug("Begin of ShowSetupScreenAsync()");
        if (string.IsNullOrEmpty(selectedTournamentId) && !string.IsNullOrEmpty(settings.ToornamentId) &&
            settings.ToornamentId != "EVOSC_MODULE_TOORNAMENTMODULE_")
        {
            logger.LogDebug("Using toornamentId from settings: {0}", settings.ToornamentId);
            selectedTournamentId = settings.ToornamentId;
        }

        var tournaments = await toornamentService.GetTournamentsAsync();
        List<StageInfo> stages = [];
        List<MatchInfo> matches = [];
        List<GroupInfo> groups = [];
        List<RoundInfo> rounds = [];

        if (!string.IsNullOrEmpty(selectedTournamentId))
        {
            stages = await toornamentService.GetStagesAsync(selectedTournamentId);
            if (!string.IsNullOrEmpty(selectedStageId))
            {
                matches = await toornamentService.GetMatchesAsync(selectedTournamentId, selectedStageId);
                groups = await toornamentService.GetGroupsAsync(selectedTournamentId, selectedStageId);
                rounds = await toornamentService.GetRoundsAsync(selectedTournamentId, selectedStageId);
            }
        }

        await manialinkManager.SendManialinkAsync(player, "ToornamentModule.TournamentSetupView",
            new
            {
                tournaments,
                stages,
                matches,
                groups,
                rounds
            });

        logger.LogDebug("End of ShowSetupScreenAsync()");
    }

    public async Task SetupServerAsync(IPlayer player, string tournamentId, string stageId, string matchId)
    {
        logger.LogDebug("Begin of SetupServerAsync()");
        if (string.IsNullOrEmpty(tournamentId))
        {
            logger.LogWarning("TournamentId is missing");
            throw new ArgumentNullException(nameof(tournamentId), "Toornament Id cannot be empty");
        }

        if (string.IsNullOrEmpty(stageId))
        {
            logger.LogWarning("StageId is missing");
            throw new ArgumentNullException(nameof(stageId), "Stage Id cannot be empty");
        }

        if (string.IsNullOrEmpty(matchId))
        {
            logger.LogWarning("MatchId is missing");
            throw new ArgumentNullException(nameof(matchId), "Match Id cannot be empty");
        }

        logger.LogDebug("Getting ToornamentData, StageData, MatchData, GroupData and RoundData from Toornament");
        //Get matchinfo from toornament
        var match = await toornamentService.GetMatchAsync(matchId);
        var stage = await toornamentService.GetStageAsync(stageId);
        var tournament = await toornamentService.GetTournamentAsync(tournamentId);
        var group = await toornamentService.GetGroupAsync(match.GroupId);
        var round = await toornamentService.GetRoundAsync(match.RoundId);

        settings.AssignedMatchId = matchId;

        //Get maps for this match (if not included in matchinfo response)
        //Download them from TMX or Nadeo (UID)?
        logger.LogDebug("Adding maps");
        var maps = await matchMapService.AddMapsAsync(player);

        //Create matchsettings for server
        logger.LogDebug("Creating match settings");
        var matchSettingsName =
            await matchSettingsCreatorService.CreateMatchSettingsAsync(tournament, match, stage, group, round, maps);
        logger.LogDebug("Matchsettings created with name: {0}", matchSettingsName);

        //Get participants for match from toornament
        //Whitelist players + spectators for server
        if (match.Opponents.First().Participant != null)
        {
            logger.LogDebug("Adding Players and spectators to the whitelist");
            await SetupPlayersAndSpectatorsAsync(match.Opponents);
        }

        //Apply matchsettings to server (this includes maps)
        logger.LogDebug("Loading the configured matchsetting {0}_warmup", matchSettingsName);
        await matchSettings.LoadMatchSettingsAsync(matchSettingsName + "_warmup");
        await server.Remote.SetServerNameAsync("Match#" + stage.Number + "." + group.Number + "." + round.Number + "." +
                                               match.Number);

        //Notify ServerSync module
        logger.LogDebug("Notify the ServerSync module");
        var serverName = Encoding.ASCII.GetBytes(await server.Remote.GetServerNameAsync());
        try
        {
            keyValueStoreService.CreateOrUpdateEntry(matchId, serverName);
        }
        catch (NATSJetStreamException ex)
        {
            logger.LogWarning(ex, "Retrieved exception from NATS");
            logger.LogWarning("Tried to create entry in KeyValueStore with Key {0} and Value {1}", matchId, serverName);
        }

        //Show ReadyForMatch widget (?)
        logger.LogDebug("Show ReadyForMatch widget for the players");
        await SetupReadyWidgetAsync(match.Opponents);

        logger.LogDebug("Setting Initial State in state service");
        stateService.SetInitialSetup(matchSettingsName);

        logger.LogDebug("Hiding the Setup window");
        await manialinkManager.HideManialinkAsync(player, "ToornamentModule.TournamentSetupView");

        logger.LogDebug("End of SetupServerAsync()");
    }

    public async Task FinishServerSetupAsync()
    {
        logger.LogDebug("Begin of FinishServerSetupAsync()");
        if (!stateService.IsInitialSetup)
        {
            return;
        }

        logger.LogDebug("Setting SetupFinished in state service");
        stateService.SetSetupFinished();

        logger.LogDebug("Loading the configured matchsetting {0}", stateService.MatchSettingsName);
        await matchSettings.LoadMatchSettingsAsync(stateService.MatchSettingsName + "_warmup", false);

        await whitelistService.KickNonWhitelistedPlayers();

        await server.Chat.InfoMessageAsync($"Toornament match has been set up.");

        logger.LogInformation("Toornament match has been set up.");

        logger.LogDebug("End of FinishServerSetupAsync()");
    }

    private async Task SetupPlayersAndSpectatorsAsync(OpponentInfo[] opponents)
    {
        await server.Remote.CleanGuestListAsync();
        await whitelistService.WhitelistPlayers(opponents);
        await whitelistService.WhitelistSpectators();
        await server.Remote.SetMaxPlayersAsync(opponents.Length);
    }

    private async Task SetupReadyWidgetAsync(OpponentInfo[] opponents)
    {
        var players = await matchPlayerService.GetPlayersFromOpponents(opponents);
        await playerReadyService.ResetReadyWidgetAsync(true);
        await playerReadyTrackerService.AddRequiredPlayersAsync(players);
        await playerReadyService.SetWidgetEnabled(true);

        var onlinePlayers = await playerManagerService.GetOnlinePlayersAsync();

        var filteredList = onlinePlayers.SelectMany(op => players.Where(p => p.AccountId == op.AccountId));

        foreach (var player in filteredList)
        {
            //TODO check if player is playing or spectating
            await playerReadyService.SendWidgetAsync(player);
        }
    }

    public async Task ShowConfirmSetupScreenAsync(IPlayer player, string tournamentId, string stageId, string matchId)
    {
        var match = await toornamentService.GetMatchAsync(matchId);

        if (match is not null && match.Status != "pending")
        {
            await manialinkManager.SendManialinkAsync(player, "ToornamentModule.Dialogs.MatchInProgressDialog",
                new
                {
                    tournamentId,
                    stageId,
                    matchId,
                });
        }
        else
        {
            await SetupServerAsync(player, tournamentId, stageId, matchId);
        }
    }
}
