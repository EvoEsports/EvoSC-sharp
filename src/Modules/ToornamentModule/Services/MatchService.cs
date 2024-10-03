using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Models;
using EvoSC.Modules.EvoEsports.ToornamentModule.Settings;
using EvoSC.Modules.Official.MapsModule.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using GbxRemoteNet;
using GbxRemoteNet.Structs;
using Microsoft.Extensions.Logging;
using NATS.Client.JetStream;
using ToornamentApi.Models.Api.TournamentApi;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchService(IAuditService auditService,
                            IServerClient server,
                            IManialinkManager manialinkManager,
                            IToornamentSettings settings,
                            IMxMapService mxMapService,
                            IMapService mapService,
                            IPlayerManagerService playerManagerService,
                            IPlayerReadyService playerReadyService,
                            IPlayerReadyTrackerService playerReadyTrackerService,
                            IStateService stateService,
                            IMatchSettingsService matchSettings,
                            IMatchTracker matchTracker,
                            IToornamentService toornamentService,
                            INadeoMapService nadeoMapService,
                            IKeyValueStoreService keyValueStoreService,
                            IPermissionManager permissionManager,
                            ILogger<MatchService> logger) : IMatchService
{
    private readonly Random _rng = new Random();

    public async Task StartMatchAsync()
    {
        logger.LogDebug("Begin of StartMatchAsync()");
        // create a new timeline and get the tracking ID
        var matchTrackerId = await matchTracker.BeginMatchAsync();

        await matchSettings.LoadMatchSettingsAsync(stateService.MatchSettingsName, false);
        await server.Remote.RestartMapAsync();

        // disable the ready widget
        await playerReadyService.SetWidgetEnabled(false);

        await server.Chat.InfoMessageAsync("Match is about to begin ...");

        stateService.SetMatchStarted();

        await KickNonWhitelistedPlayers();

        // notify Toornament that the match has started
        await toornamentService.SetMatchGameStatusAsync(settings.AssignedMatchId, 1, MatchGameStatus.Running);

        auditService.NewInfoEvent("Toornament.StartMatch")
            .HavingProperties(new { MatchTrackingId = matchTrackerId })
            .Comment("Match was started.");

        logger.LogDebug("End of StartMatchAsync()");
    }

    public async Task EndMatchAsync(ScoresEventArgs timeline)
    {
        if (stateService.WaitingForMatchStart || stateService.IsInitialSetup || stateService.SetupFinished || stateService.MatchEnded)
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
        if (string.IsNullOrEmpty(selectedTournamentId) && !string.IsNullOrEmpty(settings.ToornamentId) && settings.ToornamentId != "EVOSC_MODULE_TOORNAMENTMODULE_")
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

        await manialinkManager.SendManialinkAsync(player, "ToornamentModule.TournamentSetupView", new
        {
            tournaments = tournaments,
            stages = stages,
            matches = matches,
            groups = groups,
            rounds = rounds
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
        var maps = await AddMapsAsync(player);

        //Create matchsettings for server
        logger.LogDebug("Creating match settings");
        var matchSettingsName = await CreateMatchSettingsAsync(tournament, match, stage, group, round, maps);
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
        await server.Remote.SetServerNameAsync("Match#" + stage.Number + "." + group.Number + "." + round.Number + "." + match.Number);

        //Notify ServerSync module
        logger.LogDebug("Notify the ServerSync module");
        var serverName = Encoding.ASCII.GetBytes(await server.Remote.GetServerNameAsync());
        try
        {
            keyValueStoreService.CreateOrUpdateEntry(matchId, serverName);
        }
        catch (NATSJetStreamException ex)
        {
            logger.LogWarning(ex, "Retrieved exception from NATS when attempting to create or update entry");
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

        await KickNonWhitelistedPlayers();

        logger.LogDebug("End of FinishServerSetupAsync()");
    }

    private async Task<List<IMap?>> AddMapsAsync(IPlayer player)
    {
        logger.LogDebug("Begin of AddMapsAsync()");
        List<IMap?> maps = new List<IMap?>();
        bool allMapsOnServer = true;

        //Check if the maps are already on the server
        try
        {
            foreach (var mapUid in GetMapUids())
            {
                logger.LogDebug("Checking if map with Uid {UID} exists on the server", mapUid);
                IMap? existingMap = await mapService.GetMapByUidAsync(mapUid);
                if (existingMap == null)
                {
                    logger.LogDebug("Map with Uid {UID} was not found on the server", mapUid);
                    allMapsOnServer = false;
                }
                else
                {
                    maps.Add(existingMap);
                }
            }
        }
        catch (ArgumentNullException)
        {
            //Silently catch Exception, since we can use Nadeo Servers or TMX as backup
        }

        //Try to download maps from Nadeo Servers using the MapId
        if (!allMapsOnServer)
        {
            try
            {
                maps = await AddMapsFromNadeo(player, GetMapIds());
                // TODO: This should not check if null, but rather check if the List is empty.
                if (maps is not null)
                {
                    allMapsOnServer = true;
                }
            }
            catch (ArgumentNullException)
            {
                // TODO: This should not do it completely silent, add a a log statement.
                //Silently catch Exception, since we can use TMX as final backup
            }
        }

        //Try to download maps from TMX using tmx Ids
        if (!allMapsOnServer)
        {
            // TODO: Add logging here too, you want to understand if you end up here.
            maps = await AddMapsFromTmx(player, GetTmxIds());
            allMapsOnServer = true;
        }

        //Throw error if maps are still not found
        if (!allMapsOnServer || maps.Count == 0)
        {
            logger.LogWarning("Maps could not be found on the server, or downloaded from Nadeo servers or from TMX");
            throw new ArgumentException("Maps could not be found on the server, or downloaded from Nadeo servers or from TMX");
        }

        logger.LogDebug("End of AddMapsAsync()");
        return maps;
    }

    private async Task<List<IMap?>> AddMapsFromNadeo(IPlayer player, IEnumerable<string> mapIds)
    {
        List<IMap?> maps = new List<IMap?>();
        try
        {
            maps.AddRange(await Task.WhenAll(mapIds.Select(async m =>
            {
                try
                {
                    logger.LogDebug("Downloading map with id {0} from Nadeo servers", m);
                    return await nadeoMapService.FindAndDownloadMapAsync(m);
                }
                catch (DuplicateMapException ex)
                {
                    //Exception message is "Map with UID {MapUid} already exists in database", we need the MapUid to get the map from the server
                    var mapUid = ex.Message.Split(' ')[3];
                    return await mapService.GetMapByUidAsync(mapUid);
                }
            })));
        }
        catch (Exception)
        {
            logger.LogWarning("Failed to download map from Nadeo servers");
            var chatMessage = FormattingUtils.FormatPlayerChatMessage(player, "Failed to add map using the Nadeo servers", false);
            await server.Chat.ErrorMessageAsync(chatMessage);
            throw;
        }

        if (maps.Count() != mapIds.Count())
        {
            var chatMessage = FormattingUtils.FormatPlayerChatMessage(player, "Failed to add all maps from the Nadeo servers", false);
            await server.Chat.ErrorMessageAsync(chatMessage);
            return null;
        }
        return maps;
    }


    private async Task<List<IMap?>> AddMapsFromTmx(IPlayer player, IEnumerable<int> mapIds)
    {
        List<IMap?> maps = new List<IMap?>();
        try
        {
            maps.AddRange(await Task.WhenAll(mapIds.Select(async m =>
            {
                try
                {
                    logger.LogDebug("Downloading map with id {0} from Nadeo servers", m);
                    return await mxMapService.FindAndDownloadMapAsync(m, null, player);
                }
                catch (DuplicateMapException ex)
                {
                    //Exception message is "Map with UID {MapUid} already exists in database", we need the MapUid to get the map from the server
                    var mapUid = ex.Message.Split(' ')[3];
                    return await mapService.GetMapByUidAsync(mapUid);
                }
            })));
        }
        catch (Exception)
        {
            logger.LogWarning("Failed to download map from TMX");
            var chatMessage = FormattingUtils.FormatPlayerChatMessage(player, "Failed to add map from TMX", false);
            await server.Chat.ErrorMessageAsync(chatMessage);
            throw;
        }

        if (maps.Count() != mapIds.Count())
        {
            var chatMessage = FormattingUtils.FormatPlayerChatMessage(player, "Failed to add all maps from TMX", false);
            await server.Chat.ErrorMessageAsync(chatMessage);
            return null;
        }
        return maps;
    }

    private async Task<string> CreateMatchSettingsAsync(TournamentBasicData tournament, MatchInfo matchInfo, StageInfo stageInfo, GroupInfo groupInfo, RoundInfo roundInfo, IEnumerable<IMap> maps)
    {
        logger.LogDebug("Begin of CreateMatchSettingsAsync()");
        var settingsData = new TrackmaniaIntegrationSettingsData();
        if (settings.UseToornamentDiscipline)
        {
            //DisciplineInfo contains the trackmania matchsettings (nrOfRounds, point distribution etc)
            if (tournament != null)
            {
                logger.LogDebug("Fetching matchsettings defined in Toornament Discipline with Id {0}.", tournament.Discipline);
                DisciplineInfo? discipline = null;
                discipline = await toornamentService.GetDisciplineAsync(tournament.Discipline);
                var tmIntegration = discipline?.Features.FirstOrDefault(f => f.Name == "trackmania_integration");

                if (tmIntegration == null)
                {
                    throw new InvalidOperationException("No match discipline configuration found");
                }

                var settingsDefault = tmIntegration.Options.FirstOrDefault(o => o.Key == "settings_default").Value;
                settingsData = TrackmaniaIntegrationSettingsData.CreateFromObject(settingsDefault);
            }
        }
        else
        {
            logger.LogDebug("Using matchsettings defined in environment variable");
            if (string.IsNullOrEmpty(settings.Disciplines))
            {
                throw new ArgumentNullException(nameof(settings.Disciplines), "No disciplines defined in Environment settings");
            }
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
            var allDisciplines = JsonSerializer.Deserialize<List<TrackmaniaIntegrationSettingsData>>(settings.Disciplines, jsonSerializerOptions);
            settingsData = allDisciplines.FirstOrDefault(x => x.StageNumber == stageInfo.Number && x.GroupNumber == groupInfo.Number && x.RoundNumber == roundInfo.Number);
        }

        if (settingsData == null)
        {
            throw new ArgumentNullException("Settings are missing");
        }

        if (settingsData.Scripts.S_PointsRepartition.Equals("n-1"))
        {
            logger.LogDebug("Generating Points Repartition based on the number of Opponents in this match, which is {0} opponents", matchInfo.Opponents.Count());
            settingsData.Scripts.S_PointsRepartition = GeneratePointRepartition(matchInfo.Opponents.Count());
        }

        var name = $"tournament_{settingsData.StageNumber}_{settingsData.GroupNumber}_{settingsData.RoundNumber}";

        var mapsToAdd = new List<IMap>();

        foreach (var map in maps)
        {
            try
            {
                logger.LogDebug("Checking if all the maps are available on the server");
                var serverMap = await server.Remote.GetMapInfoAsync(map.FilePath);

                if (serverMap != null)
                {
                    mapsToAdd.Add(map);
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        if (settingsData.TracksShuffle)
        {
            logger.LogDebug("Shuffling the map order");
            mapsToAdd = Shuffle(mapsToAdd);
        }

        await CreateMatchSettings(name, settingsData, mapsToAdd);

        var warmupSettingsData = settingsData;
        warmupSettingsData.Scripts.S_WarmUpDuration = 3600;
        warmupSettingsData.Scripts.S_WarmUpNb = 1;
        await CreateMatchSettings(name + "_warmup", settingsData, mapsToAdd);

        logger.LogDebug("End of CreateMatchSettingsAsync()");
        return name;
    }

    private async Task CreateMatchSettings(string name, TrackmaniaIntegrationSettingsData settingsData, List<IMap> mapsToAdd)
    {
        try
        {
            await matchSettings.CreateMatchSettingsAsync(name, builder =>
            {
                if (settings.UseDefaultGameMode)
                {
                    var mode = settingsData.GameMode switch
                    {
                        "rounds" => DefaultModeScriptName.Rounds,
                        "cup" => DefaultModeScriptName.Cup,
                        "time_attack" => DefaultModeScriptName.TimeAttack,
                        "knockout" => DefaultModeScriptName.Knockout,
                        "laps" => DefaultModeScriptName.Laps,
                        "team" => DefaultModeScriptName.Teams,
                        _ => DefaultModeScriptName.TimeAttack
                    };
                    logger.LogDebug("Creating match settings with gamemode {0}", mode);

                    builder.WithMode(mode);
                }
                else
                {
                    if (string.IsNullOrEmpty(settings.GameMode))
                    {
                        logger.LogWarning("Custom gamemode not defined in environment!!");
                        throw new ArgumentNullException(nameof(settings.GameMode));
                    }
                    logger.LogDebug("Creating match settings with custom gamemode {0}", settings.GameMode);
                    builder.WithMode(settings.GameMode);
                }

                builder.WithModeSettings(s =>
                {
                    var type = settingsData.Scripts.GetType();
                    var properties = settingsData.Scripts.GetType().GetProperties();

                    logger.LogDebug("Creating match settings with scriptsettings: ");
                    foreach (var item in properties)
                    {
                        var value = item.GetValue(settingsData.Scripts);
                        logger.LogDebug("[{0}]: {1}", item.Name, value);
                        s[item.Name] = value;
                    }
                });

                builder.WithMaps(mapsToAdd);
                builder.WithFilter(f => f.AsRandomMapOrder(settingsData.TracksShuffle));
            });
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task SetupPlayersAndSpectatorsAsync(OpponentInfo[] opponents)
    {
        await server.Remote.CleanGuestListAsync();
        await WhitelistPlayers(opponents);
        await WhitelistSpectators();
        await server.Remote.SetMaxPlayersAsync(opponents.Length);
    }

    private async Task WhitelistPlayers(OpponentInfo[] opponents)
    {
        logger.LogDebug("Begin of WhitelistPlayers()");
        var multiCall = new MultiCall();

        var players = await GetPlayersFromOpponents(opponents);

        foreach (var player in players)
        {
            var login = PlayerUtils.ConvertAccountIdToLogin(player.AccountId);
            multiCall.Add("AddGuest", login);
        }

        await server.Remote.MultiCallAsync(multiCall);
        logger.LogDebug("End of WhitelistPlayers()");
    }
    private async Task SetupReadyWidgetAsync(OpponentInfo[] opponents)
    {
        var players = await GetPlayersFromOpponents(opponents);
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

    private async Task<List<IPlayer>> GetPlayersFromOpponents(OpponentInfo[] opponents)
    {
        List<IPlayer> players = [];
        foreach (var opponent in opponents)
        {
            if (opponent is not null)
            {
                IPlayer player = null;
                if (opponent.Participant is not null)
                {
                    var tmId = opponent.Participant.CustomFields["trackmania_id"];
                    if (tmId is not null)
                    {
                        player = await playerManagerService.GetOrCreatePlayerAsync(tmId.ToString(), opponent.Participant.Name);
                        players.Add(player);
                    }
                }

                if (player is not null && player.Groups.Count() == 0)
                {
                    await permissionManager.AddPlayerToGroupAsync(player, settings.DefaultGroupId);
                }
            }

        }

        return players;
    }

    private async Task WhitelistSpectators()
    {
        logger.LogDebug("Begin of WhitelistSpectators()");
        if (!string.IsNullOrEmpty(settings.Whitelist))
        {
            var multiCall = new MultiCall();
            foreach (var accountId in settings.Whitelist.Split(','))
            {
                var login = PlayerUtils.ConvertAccountIdToLogin(accountId);
                multiCall.Add("AddGuest", login);
            }

            await server.Remote.MultiCallAsync(multiCall);
        }
        logger.LogDebug("End of WhitelistSpectators()");
    }

    private List<int> GetTmxIds()
    {
        if (string.IsNullOrEmpty(settings.MapTmxIds))
        {
            throw new ArgumentNullException(nameof(settings.MapTmxIds), "Map TMX Ids not defined in environment settings");
        }

        var mapIds = new List<int>();

        foreach (var s in settings.MapTmxIds.Split(','))
        {
            int num;
            if (int.TryParse(s, out num))
            {
                mapIds.Add(num);
            }
        }
        return mapIds;
    }

    private List<string> GetMapUids()
    {
        if (string.IsNullOrEmpty(settings.MapUids))
        {
            throw new ArgumentNullException(nameof(settings.MapUids), "Map Uids not defined in environment settings");
        }

        return settings.MapUids.Split(',').ToList();
    }

    private List<string> GetMapIds()
    {
        if (string.IsNullOrEmpty(settings.MapIds))
        {
            throw new ArgumentNullException(nameof(settings.MapIds), "Map Ids not defined in environment settings");
        }

        return settings.MapIds.Split(',').ToList();
    }

    private string GeneratePointRepartition(int number)
    {
        if (number < 1)
        {
            return string.Empty; // Return an empty string if the number is less than 1
        }

        var countdownArray = new string[number];
        for (int i = 0; i < number; i++)
        {
            countdownArray[i] = (number - i).ToString();
        }

        return string.Join(",", countdownArray);
    }

    private List<IMap> Shuffle(List<IMap> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _rng.Next(n + 1);
            IMap value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    public async Task ForcePlayerIntoSpectate(string login)
    {
        if (!settings.UseExperimentalFeatures)
        {
            return;
        }

        if (!stateService.WaitingForMatchStart && !stateService.MatchInProgress)
        {
            // No match in progress, so players won't get put into Spectate
            return;
        }

        var accountId = PlayerUtils.ConvertLoginToAccountId(login);
        var player = await playerManagerService.GetOrCreatePlayerAsync(accountId);

        var guestList = await GetGuestListAsync();
        var whitelistedSpectates = settings.Whitelist.Split(',');

        //Player is not in the guestlist -> will get kicked
        if (guestList.FirstOrDefault(g => g.Login == login) is null)
        {
            await KickAsync(player);
        }

        //Player is in the configured whitelist -> put into spectate mode
        if (player is not null && whitelistedSpectates.Contains(accountId))
        {
            if (player.Groups.Count() == 0)
            {
                await permissionManager.AddPlayerToGroupAsync(player, settings.DefaultGroupId);
            }
            await ForceSpectatorAsync(player);
        }
    }

    private async Task KickNonWhitelistedPlayers()
    {
        if (!settings.UseExperimentalFeatures)
        {
            return;
        }
        var connectedPlayers = await server.Remote.GetPlayerListAsync();

        var guestList = await GetGuestListAsync();
        var whitelistedSpectates = settings.Whitelist.Split(',');

        foreach (var connectedPlayer in connectedPlayers)
        {
            if (connectedPlayer.IsServer())
            {
                continue;
            }
            var accountId = PlayerUtils.ConvertLoginToAccountId(connectedPlayer.Login);
            var player = await playerManagerService.GetOrCreatePlayerAsync(accountId);

            //Skip player if the player is Admin
            if (player.Groups.Any(x => x.Id == 1))
            {
                continue;
            }

            //Player is not in the guestlist -> will get kicked
            if (guestList.FirstOrDefault(g => g.Login == connectedPlayer.Login) is null)
            {
                await KickAsync(player);
            }

            //Player is in the configured whitelist -> put into spectate mode
            if (player is not null && whitelistedSpectates.Contains(accountId))
            {
                if (player.Groups.Count() == 0)
                {
                    await permissionManager.AddPlayerToGroupAsync(player, settings.DefaultGroupId);
                }
                await ForceSpectatorAsync(player);
            }
        }
    }

    private async Task KickAsync(IPlayer player)
    {
        if (await server.Remote.KickAsync(player.GetLogin(), ""))
        {
            logger.LogDebug("Kicked player {0} from server", player.UbisoftName);
        }
        else
        {
            logger.LogWarning("Failed to kick player {0} from server", player.UbisoftName);
        }
    }

    private Task ForceSpectatorAsync(IPlayer player) => server.Remote.ForceSpectatorAsync(player.GetLogin(), 1);


    private async Task<TmGuestListEntry[]> GetGuestListAsync()
    {
        var maxPlayers = await server.Remote.GetMaxPlayersAsync();
        var nrOfPlayers = 0;
        if (maxPlayers is not null)
        {
            nrOfPlayers += maxPlayers.CurrentValue;
        }

        var whitelistedSpectates = settings.Whitelist.Split(',');

        if (!string.IsNullOrEmpty(settings.Whitelist))
        {
            nrOfPlayers += whitelistedSpectates.Count();
        }

        return await server.Remote.GetGuestListAsync(nrOfPlayers, 0);
    }
}
