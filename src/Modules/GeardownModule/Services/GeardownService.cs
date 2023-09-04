using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Services;
using EvoSC.Modules.Evo.GeardownModule.Models;
using EvoSC.Modules.Evo.GeardownModule.Models.API;
using EvoSC.Modules.Evo.GeardownModule.Settings;
using EvoSC.Modules.Evo.GeardownModule.Util;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using GbxRemoteNet;
using ManiaExchange.ApiClient;
using Microsoft.Extensions.Logging;
using MatchStatus = EvoSC.Modules.Official.MatchTrackerModule.Models.MatchStatus;

namespace EvoSC.Modules.Evo.GeardownModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class GeardownService : IGeardownService
{
    private readonly IMapService _maps;
    private readonly IMatchSettingsService _matchSettings;
    private readonly IGeardownApiService _geardownApi;
    private readonly IServerClient _server;
    private readonly IGeardownSettings _settings;
    private readonly IPlayerReadyService _playerReadyService;
    private readonly IMatchTracker _matchTracker;
    private readonly IAuditService _audits;
    private readonly IPlayerReadyTrackerService _playerReadyTracker;
    private readonly ILogger<GeardownService> _logger;
    private readonly IPlayerManagerService _players;

    public GeardownService(IGeardownApiService geardownApi, IMapService maps, IMatchSettingsService matchSettings,
        IServerClient server, IGeardownSettings settings, IPlayerReadyService playerReadyService,
        IPlayerReadyTrackerService playerReadyTracker, IMatchTracker matchTracker, IAuditService audits,
        ILogger<GeardownService> logger, IPlayerManagerService players)
    {
        _geardownApi = geardownApi;
        _maps = maps;
        _matchSettings = matchSettings;
        _server = server;
        _settings = settings;
        _playerReadyService = playerReadyService;
        _matchTracker = matchTracker;
        _audits = audits;
        _playerReadyTracker = playerReadyTracker;
        _logger = logger;
        _players = players;
    }

    public async Task SetupServerAsync(int matchId)
    {
        var token = await AssignServerToMatchAsync(matchId);

        if (token == null)
        {
            throw new InvalidOperationException($"Failed to assign server to match {matchId}. Token returned is null.");
        }
        
        var match = await GetMatchInfoAsync(token.EvoScToken);
        var maps = await GetMatchMapsAsync(match);
        var matchSettingsName = await CreateMatchSettingsAsync(match, maps);

        _settings.MatchState = JsonSerializer.Serialize(new GeardownMatchState
        {
            Match = match,
            MatchToken = token.EvoScToken
        });

        var players = (await GetParticipantPlayersAsync(match)).ToArray();

        if (!players.Any())
        {
            throw new InvalidOperationException($"No participants found for match {match.id} ({match.name}).");
        }
        
        await SetupPlayersAndSpectatorsAsync(players);
        await _matchSettings.LoadMatchSettingsAsync(matchSettingsName);
        await _playerReadyService.ResetReadyWidgetAsync(true);
        await _playerReadyTracker.AddRequiredPlayersAsync(players);

        _audits.NewInfoEvent("Geardown.ServerSetup")
            .HavingProperties(new { Match = match, MatchToken = token.EvoScToken })
            .Comment("Server was setup through geardown.");
    }

    public async Task StartMatchAsync()
    {
        var matchTrackerId = await _matchTracker.BeginMatchAsync();
        await _server.Remote.RestartMapAsync();
        
        _audits.NewInfoEvent("Geardown.StartMatch")
            .HavingProperties(new { MatchTrackingId = matchTrackerId })
            .Comment("Match was started.");
    }

    public async Task SendResultsAsync(string matchToken, IMatchTimeline argsTimeline)
    {
        var matchState = JsonSerializer.Deserialize<GeardownMatchState>(_settings.MatchState);

        if (matchState?.Match?.participants?.FirstOrDefault()?.user?.tm_account_id == null)
        {
            throw new InvalidOperationException("The match state is invalid, failed to send results.");
        }
        
        var results = argsTimeline.States.LastOrDefault(s => s.Status == MatchStatus.Running) as IScoresMatchState;

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
        
        /* foreach (var player in results.Players)
        {
            var participant =
                matchState.Match.participants.FirstOrDefault(p => p.user.tm_account_id == player.Player.AccountId);
            
            

            try
            {
                await _geardownApi.Matches.CreateMatchResultAsync((int)matchState.Match.id, true, (int)participant.id,
                    player.MatchPoints.ToString(), true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send match results to geardown");
            }
        } */
    }

    private async Task SetupPlayersAndSpectatorsAsync(IEnumerable<IPlayer> players)
    {
        await _server.Remote.CleanGuestListAsync();
        await WhitelistPlayers(players);
        await WhitelistSpectators();
        await _server.Remote.SetMaxPlayersAsync(players.Count());
    }

    private async Task<IMap[]> GetMatchMapsAsync(GdMatch? match)
    {
        var maps = (await GetMapsAsync(match)).ToArray();

        if (!maps.Any())
        {
            throw new InvalidOperationException("Did not find any maps for this match.");
        }

        return maps;
    }

    private async Task<IMap> DownloadAndAddMap(GdMapPoolOrder mapPoolMap)
    {
        var tmxApi = new MxTmApi("EvoSC# Geardown Integration");
        var tmxId = (long?)mapPoolMap.mx_map_id ?? 0;

        var mapInfo = await tmxApi.GetMapInfoAsync(tmxId);

        if (mapInfo == null)
        {
            throw new InvalidOperationException(
                $"Failed to get map info for the map with pool ID {mapPoolMap.map_pool_id} and TMX ID {tmxId}.");
        }
        
        var mapFile = await tmxApi.DownloadMapAsync(tmxId, "");

        if (mapFile == null)
        {
            throw new InvalidOperationException(
                $"Failed to download the map with pool ID {mapPoolMap.map_pool_id} and TMX ID {tmxId}.");
        }

        var mapMetaInfo = new MapMetadata
        {
            MapUid = mapInfo.TrackUID,
            MapName = mapInfo.GbxMapName,
            AuthorId = mapInfo.AuthorLogin,
            AuthorName = mapInfo.Username,
            ExternalId = mapInfo.TrackID.ToString(),
            ExternalVersion = Convert.ToDateTime(mapInfo.UpdatedAt, NumberFormatInfo.InvariantInfo).ToUniversalTime(),
            ExternalMapProvider = MapProviders.ManiaExchange
        };

        var mapStream = new MapStream(mapMetaInfo, mapFile);
        return await _maps.AddMapAsync(mapStream);
    }
    
    private async Task<IEnumerable<IMap>> GetMapsAsync(GdMatch match)
    {
        if (match.map_pool_orders == null || match.map_pool_orders.Count == 0)
        {
            throw new InvalidOperationException("No maps found for this match. Did you add them on geardown or forgot to give them an order?");
        }
        
        var maps = new List<IMap>();
        
        foreach (var mapPoolMap in match.map_pool_orders)
        {
            if (mapPoolMap.mx_map_id == null)
            {
                throw new InvalidOperationException($"The map with pool ID {mapPoolMap.map_pool_id} does not have a TMX id.");
            }

            var map = await _maps.GetMapByExternalIdAsync(mapPoolMap.mx_map_id.ToString() ?? "") ??
                      await DownloadAndAddMap(mapPoolMap);

            maps.Add(map);
        }

        return maps;
    }

    private async Task<string> CreateMatchSettingsAsync(GdMatch match, IEnumerable<IMap> maps)
    {
        if (match.formats == null || match.formats.Count == 0)
        {
            throw new InvalidOperationException("No formats has been assigned to this match, did you assign one to the match at geardown?");
        }
        
        var format = match.formats.First();
        var name = $"geardown_{format.id}";
        await _matchSettings.CreateMatchSettingsAsync(name, builder =>
        {
            var mode = format.type_id switch
            {
                FormatType.Rounds => DefaultModeScriptName.Rounds,
                FormatType.Cup => DefaultModeScriptName.Cup,
                FormatType.TimeAttack => DefaultModeScriptName.TimeAttack,
                FormatType.Knockout => DefaultModeScriptName.Knockout,
                FormatType.Laps => DefaultModeScriptName.Laps,
                FormatType.Team => DefaultModeScriptName.Teams,
                _ => DefaultModeScriptName.TimeAttack
            };

            builder.WithFilter(f => f.AsRandomMapOrder(true));
            builder.WithMode(mode);
            builder.WithModeSettings(s =>
            {
                if (format.match_settings == null)
                {
                    return;
                }
                
                foreach (var setting in format.match_settings)
                {
                    s[setting.key] = MatchSettingsTypeUtils.ConvertToCorrectType(setting.key, setting.value).GetAwaiter().GetResult();
                }
            });

            builder.WithMaps(maps);
            //builder.AddMap("Campaigns/CurrentQuarterly/Spring 2023 - 01.Map.Gbx");
        });

        return name;
    }

    private async Task WhitelistPlayers(IEnumerable<IPlayer> players)
    {
        var multiCall = new MultiCall();
        
        foreach (var player in players)
        {
            multiCall.Add("AddGuest", player.GetLogin());
        }

        await _server.Remote.MultiCallAsync(multiCall);
    }

    private async Task WhitelistSpectators()
    {
        var multiCall = new MultiCall();
        foreach (var accountId in _settings.Whitelist.Split(','))
        {
            var login = PlayerUtils.ConvertAccountIdToLogin(accountId);
            // await _server.Remote.AddGuestAsync(login);
            multiCall.Add("AddGuest", login);
        }

        await _server.Remote.MultiCallAsync(multiCall);
    }

    private async Task<GdMatch?> GetMatchInfoAsync(string matchToken)
    {
        var match = await _geardownApi.Matches.GetMatchDataByTokenAsync(matchToken);

        if (match == null)
        {
            throw new InvalidOperationException("Failed to fetch match from geardown API.");
        }

        return match;
    }

    private async Task<GdMatchToken?> AssignServerToMatchAsync(int matchId)
    {
        var password = await _server.Remote.GetServerPasswordAsync();
        var name = await _server.Remote.GetServerNameAsync() ?? "Server";
        var mainServerPlayer = await _server.Remote.GetMainServerPlayerInfoAsync(0);

        if (string.IsNullOrEmpty(password))
        {
            password = null;
        }

        return await _geardownApi.Matches.AssignServerAsync(matchId, name, mainServerPlayer.Login, password);
    }

    private async Task<IEnumerable<IPlayer>> GetParticipantPlayersAsync(GdMatch match)
    {
        if (match.participants == null)
        {
            throw new InvalidOperationException("No participants found for this match.");
        }

        var players = new List<IPlayer>();
        
        foreach (var participant in match.participants)
        {
            if (participant.user == null)
            {
                throw new InvalidOperationException(
                    $"Participant with ID {participant.id} has no user account attached. Are they a real geardown user?");
            }

            if (string.IsNullOrEmpty(participant.user.tm_account_id))
            {
                throw new InvalidOperationException(
                    $"Participant with ID {participant.id} (name: {participant.user.nickname}) has no TM account ID.");
            }
            
            var player = await _players.GetOrCreatePlayerAsync(participant.user.tm_account_id);
            players.Add(player);
        }

        return players;
    }
}
