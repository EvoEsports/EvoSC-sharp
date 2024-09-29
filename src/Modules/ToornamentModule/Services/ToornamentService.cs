using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Models;
using EvoSC.Modules.EvoEsports.ToornamentModule.Settings;
using Microsoft.Extensions.Logging;
using ToornamentApi;
using ToornamentApi.Auth;
using ToornamentApi.Models.Api.TournamentApi;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class ToornamentService : IToornamentService
{
    private readonly IToornamentSettings _settings;
    private readonly IServerClient _serverClient;
    private readonly ILogger<IToornamentService> _logger;
    public ToornamentService(IServerClient serverClient, IToornamentSettings settings, ILogger<IToornamentService> logger)
    {
        _settings = settings;
        _serverClient = serverClient;
        _logger = logger;

        if (string.IsNullOrEmpty(_settings.ApiKey) || string.IsNullOrEmpty(_settings.ClientId) ||
            string.IsNullOrEmpty(_settings.ClientSecret))
            throw new ArgumentNullException("Missing apikey, clientid or clientsecret to connect to Toornament");
    }

    public async Task<List<TournamentBasicData>?> GetTournamentsAsync()
    {
        try
        {
            _logger.LogInformation("Begin of GetTournamentsAsync()");
            var toornamentClient = ToornamentApiClient.Create(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);

            var tournaments = new List<TournamentBasicData>();

            //Get First Page
            var pagedTournaments = await toornamentClient.Tournament.Tournaments.GetAsync(sort: "scheduled_desc");

            if (pagedTournaments.Tournaments is not null)
            {
                tournaments.AddRange(pagedTournaments.Tournaments);

                int secondPageStart = 50;
                int secondPageEnd = 99;
                int pageSize = 50;
                //Get Next Pages
                for (; secondPageStart < pagedTournaments.Total; secondPageStart += pageSize, secondPageEnd += pageSize)
                {
                    pagedTournaments = await toornamentClient.Tournament.Tournaments.GetAsync(pageStart: secondPageStart, pageEnd: secondPageEnd, sort: "scheduled_desc");

                    if (pagedTournaments.Tournaments is not null)
                    {
                        tournaments.AddRange(pagedTournaments.Tournaments);
                    }
                }
            }
            _logger.LogInformation("End of GetTournamentsAsync()");
            return tournaments;
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetTournaments", "<none>");
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<TournamentBasicData?> GetTournamentAsync(string tournamentId)
    {
        try
        {
            _logger.LogInformation("Begin of GetTournamentAsync()");
            var toornamentClient = ToornamentApiClient.Create(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);

            _logger.LogInformation("End of GetTournamentsAsync()");
            return await toornamentClient.Tournament.Tournaments.GetTournamentAsync(tournamentId);
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetTournament", tournamentId);
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<DisciplineInfo?> GetDisciplineAsync(string disciplineId)
    {
        try
        {
            _logger.LogInformation("Begin of GetDisciplineAsync()");
            var toornamentClient = ToornamentApiClient.Create(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);

            _logger.LogInformation("End of GetDisciplineAsync()");
            return await toornamentClient.Tournament.Disciplines.GetDisciplineAsync(disciplineId);
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetDiscipline", disciplineId);
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<List<StageInfo>?> GetStagesAsync(string tournamentId)
    {
        try
        {
            _logger.LogInformation("End of GetStagesAsync()");
            var toornamentClient = ToornamentApiClient.Create(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);

            var stages = new List<StageInfo>();

            //Get First Page
            var pagedStages = await toornamentClient.Structure.Stages.GetAsync(tournamentIds: [tournamentId]);

            if (pagedStages.Stages is not null)
            {
                stages.AddRange(pagedStages.Stages);

                int secondPageStart = 50;
                int secondPageEnd = 99;
                int pageSize = 50;
                //Get Next Pages
                for (; secondPageStart < pagedStages.Total; secondPageStart += pageSize, secondPageEnd += pageSize)
                {
                    pagedStages = await toornamentClient.Structure.Stages.GetAsync(pageStart: secondPageStart, pageEnd: secondPageEnd, tournamentIds: [tournamentId]);

                    if (pagedStages.Stages is not null)
                    {
                        stages.AddRange(pagedStages.Stages);
                    }
                }
            }
            _logger.LogInformation("End of GetStagesAsync()");
            return stages;
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetStages", tournamentId);
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<StageInfo?> GetStageAsync(string stageId)
    {
        try
        {
            _logger.LogInformation("Begin of GetStageAsync()");
            var toornamentClient = ToornamentApiClient.Create(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);
            var stage = await toornamentClient.Structure.Stages.GetStageAsync(stageId);

            _logger.LogInformation("End of GetStageAsync()");
            return stage;
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetStage", stageId);
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<List<MatchInfo>?> GetMatchesAsync(string tournamentId, string stageId)
    {
        try
        {
            _logger.LogInformation("Begin of GetMatchesAsync()");
            //We need the OrganizerResult scope to access the Matches api.
            var matchScope = Scope.OrganizerView | Scope.OrganizerAdmin | Scope.OrganizerResult | Scope.OrganizerParticipant | Scope.OrganizerRegistration | Scope.OrganizerPermission | Scope.OrganizerFile;
            var toornamentAuth = new ToornamentAuth(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);
            await toornamentAuth.RequestAccessAsync(matchScope);

            var toornamentClient = new ToornamentApiClient(toornamentAuth);

            var matches = new List<MatchInfo>();

            //Get First Page
            var pagedMatches = await toornamentClient.Match.Matches.GetAsync(stageIds: [stageId], tournamentIds: [tournamentId]);

            if (pagedMatches.Matches is not null)
            {
                matches.AddRange(pagedMatches.Matches);

                int secondPageStart = 50;
                int secondPageEnd = 99;
                int pageSize = 50;
                //Get Next Pages
                for (; secondPageStart < pagedMatches.Total; secondPageStart += pageSize, secondPageEnd += pageSize)
                {
                    pagedMatches = await toornamentClient.Match.Matches.GetAsync(pageStart: secondPageStart, pageEnd: secondPageEnd, stageIds: [stageId], tournamentIds: [tournamentId]);

                    if (pagedMatches.Matches is not null)
                    {
                        matches.AddRange(pagedMatches.Matches);
                    }
                }
            }

            _logger.LogInformation("End of GetMatchesAsync()");
            return matches;
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetMatches", tournamentId, stageId);
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<MatchInfo?> GetMatchAsync(string matchId)
    {
        try
        {
            _logger.LogInformation("Begin of GetMatchAsync()");
            //We need the OrganizerResult scope to access the Matches api.
            var matchScope = Scope.OrganizerView | Scope.OrganizerAdmin | Scope.OrganizerResult | Scope.OrganizerParticipant | Scope.OrganizerRegistration | Scope.OrganizerPermission | Scope.OrganizerFile;
            var toornamentAuth = new ToornamentAuth(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);
            await toornamentAuth.RequestAccessAsync(matchScope);

            var toornamentClient = new ToornamentApiClient(toornamentAuth);
            var match = await toornamentClient.Match.Matches.GetMatchAsync(matchId);

            _logger.LogInformation("End of GetMatchAsync()");
            return match;
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetMatch", matchId);
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<List<MatchGameInfo>?> GetMatchGamesAsync(string matchId)
    {
        try
        {
            _logger.LogInformation("Begin of GetMatchGamesAsync()");
            //We need the OrganizerResult scope to access the Matches api.
            var matchScope = Scope.OrganizerView | Scope.OrganizerAdmin | Scope.OrganizerResult | Scope.OrganizerParticipant | Scope.OrganizerRegistration | Scope.OrganizerPermission | Scope.OrganizerFile;
            var toornamentAuth = new ToornamentAuth(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);
            await toornamentAuth.RequestAccessAsync(matchScope);

            var toornamentClient = new ToornamentApiClient(toornamentAuth);
            var matchgames = await toornamentClient.Match.MatchGames.GetAsync(matchId);

            _logger.LogInformation("End of GetMatchGamesAsync()");
            return matchgames.ToList();
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetMatchGames", matchId);
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<MatchGameInfo?> GetMatchGameAsync(string matchId, int gameNumber)
    {
        try
        {
            _logger.LogInformation("Begin of GetMatchGameAsync()");
            //We need the OrganizerResult scope to access the Matches api.
            var matchScope = Scope.OrganizerView | Scope.OrganizerAdmin | Scope.OrganizerResult | Scope.OrganizerParticipant | Scope.OrganizerRegistration | Scope.OrganizerPermission | Scope.OrganizerFile;
            var toornamentAuth = new ToornamentAuth(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);
            await toornamentAuth.RequestAccessAsync(matchScope);

            var toornamentClient = new ToornamentApiClient(toornamentAuth);

            _logger.LogInformation("End of GetMatchGameAsync()");
            return await toornamentClient.Match.MatchGames.GetMatchGameAsync(matchId, gameNumber);
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetMatchGame", matchId, gameNumber.ToString());
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<MatchGameInfo?> SetMatchGameStatusAsync(string matchId, int gameNumber, MatchGameStatus status)
    {
        try
        {
            _logger.LogInformation("Begin of SetMatchGameStatusAsync()");
            //We need the OrganizerResult scope to access the Matches api.
            var matchScope = Scope.OrganizerView | Scope.OrganizerAdmin | Scope.OrganizerResult | Scope.OrganizerParticipant | Scope.OrganizerRegistration | Scope.OrganizerPermission | Scope.OrganizerFile;
            var toornamentAuth = new ToornamentAuth(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);
            await toornamentAuth.RequestAccessAsync(matchScope);

            var toornamentClient = new ToornamentApiClient(toornamentAuth);
            var currentGame = await toornamentClient.Match.MatchGames.GetMatchGameAsync(matchId, gameNumber);

            if (currentGame != null)
            {
                currentGame.Status = status.ToString().ToLower();
                //Give a temp score to 1 player to apply the match status
                if (currentGame.Opponents is not null)
                {
                    currentGame.Opponents.First().Score = 0;
                    _logger.LogInformation("End of SetMatchGameStatusAsync()");
                    return await toornamentClient.Match.MatchGames.UpdateMatchGameAsync(matchId, gameNumber, currentGame);
                }
            }
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "SetMatchGameStatus", matchId, gameNumber.ToString());
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<MatchGameInfo?> SetMatchGameResultAsync(string matchId, int gameNumber, MatchGameInfo gameInfo)
    {
        try
        {
            _logger.LogInformation("Begin of SetMatchGameResultAsync()");
            //We need the OrganizerResult scope to access the Matches api.
            var matchScope = Scope.OrganizerView | Scope.OrganizerAdmin | Scope.OrganizerResult | Scope.OrganizerParticipant | Scope.OrganizerRegistration | Scope.OrganizerPermission | Scope.OrganizerFile;
            var toornamentAuth = new ToornamentAuth(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);
            await toornamentAuth.RequestAccessAsync(matchScope);

            var toornamentClient = new ToornamentApiClient(toornamentAuth);

            _logger.LogInformation("End of SetMatchGameResultAsync()");
            return await toornamentClient.Match.MatchGames.UpdateMatchGameAsync(matchId, gameInfo.Number, gameInfo);
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "SetMatchGameResult", matchId, gameNumber.ToString());
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<List<GroupInfo>?> GetGroupsAsync(string tournamentId, string stageId)
    {
        try
        {
            _logger.LogInformation("Begin of GetGroupsAsync()");
            var toornamentClient = ToornamentApiClient.Create(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);
            var groups = new List<GroupInfo>();

            //Get First Page
            var pagedGroups = await toornamentClient.Structure.Groups.GetAsync(stageIds: [stageId], tournamentIds: [tournamentId]);

            if (pagedGroups.Groups is not null)
            {
                groups.AddRange(pagedGroups.Groups);

                int secondPageStart = 50;
                int secondPageEnd = 99;
                int pageSize = 50;
                //Get Next Pages
                for (; secondPageStart < pagedGroups.Total; secondPageStart += pageSize, secondPageEnd += pageSize)
                {
                    pagedGroups = await toornamentClient.Structure.Groups.GetAsync(pageStart: secondPageStart, pageEnd: secondPageEnd, stageIds: [stageId], tournamentIds: [tournamentId]);

                    if (pagedGroups.Groups is not null)
                    {
                        groups.AddRange(pagedGroups.Groups);
                    }
                }
            }

            _logger.LogInformation("End of GetGroupsAsync()");
            return groups;

        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetGroups", tournamentId, stageId);
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<GroupInfo?> GetGroupAsync(string groupId)
    {
        try
        {
            _logger.LogInformation("Begin of GetGroupAsync()");
            var toornamentClient = ToornamentApiClient.Create(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);
            var group = await toornamentClient.Structure.Groups.GetGroupAsync(groupId);

            _logger.LogInformation("End of GetGroupAsync()");
            return group;
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetGroup", groupId);
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<List<RoundInfo>?> GetRoundsAsync(string tournamentId, string stageId)
    {
        try
        {
            _logger.LogInformation("Begin of GetRoundsAsync()");
            var toornamentClient = ToornamentApiClient.Create(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);
            var rounds = new List<RoundInfo>();

            //Get First Page
            var pagedRounds = await toornamentClient.Structure.Rounds.GetAsync(stageIds: [stageId], tournamentIds: [tournamentId]);

            if (pagedRounds.Rounds is not null)
            {
                rounds.AddRange(pagedRounds.Rounds);

                int secondPageStart = 50;
                int secondPageEnd = 99;
                int pageSize = 50;
                //Get Next Pages
                for (; secondPageStart < pagedRounds.Total; secondPageStart += pageSize, secondPageEnd += pageSize)
                {
                    pagedRounds = await toornamentClient.Structure.Rounds.GetAsync(pageStart: secondPageStart, pageEnd: secondPageEnd, stageIds: [stageId], tournamentIds: [tournamentId]);

                    if (pagedRounds.Rounds is not null)
                    {
                        rounds.AddRange(pagedRounds.Rounds);
                    }
                }
            }

            _logger.LogInformation("End of GetRoundsAsync()");
            return rounds;
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetRounds", tournamentId, stageId);
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<RoundInfo?> GetRoundAsync(string roundId)
    {
        try
        {
            _logger.LogInformation("Begin of GetRoundAsync()");
            var toornamentClient = ToornamentApiClient.Create(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);
            var round = await toornamentClient.Structure.Rounds.GetRoundAsync(roundId);
            _logger.LogInformation("End of GetRoundAsync()");
            return round;
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "GetRound", roundId);
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    public async Task<MatchGameInfo?> SetMatchGameMapAsync(string matchId, int gameNumber, string mapName)
    {
        try
        {
            _logger.LogInformation("Begin of SetMatchGameMapAsync()");
            //We need the OrganizerResult scope to access the Matches api.
            var matchScope = Scope.OrganizerView | Scope.OrganizerAdmin | Scope.OrganizerResult | Scope.OrganizerParticipant | Scope.OrganizerRegistration | Scope.OrganizerPermission | Scope.OrganizerFile;
            var toornamentAuth = new ToornamentAuth(_settings.ApiKey, _settings.ClientId, _settings.ClientSecret);
            await toornamentAuth.RequestAccessAsync(matchScope);

            var toornamentClient = new ToornamentApiClient(toornamentAuth);
            var currentGame = await toornamentClient.Match.MatchGames.GetMatchGameAsync(matchId, gameNumber);

            if (currentGame != null)
            {
                if (currentGame.Properties.ContainsKey("track"))
                {
                    currentGame.Properties.Remove("track");
                }
                currentGame.Properties.Add("track", mapName);
                return await toornamentClient.Match.MatchGames.UpdateMatchGameAsync(matchId, gameNumber, currentGame);
            }
        }
        catch (Hawf.Client.Exceptions.HawfResponseException ex)
        {
            await HandleHawfException(ex, "MatchGameMap", matchId, gameNumber.ToString());
        }
        catch (Exception)
        {
            throw;
        }
        return null;
    }

    private async Task HandleHawfException(Hawf.Client.Exceptions.HawfResponseException ex, string functionName, string matchId, string? gameNumber = null)
    {
        await _serverClient.Chat.ErrorMessageAsync($"Failed to set {functionName} from Toornament. Error: {ex.Message}");
        _logger.LogWarning("Failed to call the {0} endpoint in Toornament. Retrieved statuscode: {1}", functionName, ex.StatusCode);
        if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            if (gameNumber is not null)
            {
                _logger.LogWarning("The {0} with Id {1} and number {2} was not found.", functionName, matchId, gameNumber);
            }
            else
            {
                _logger.LogWarning("The {0} with Id {1} was not found.", functionName, matchId);
            }
        }

        if (ex.Response.RequestMessage is not null)
        {
            _logger.LogWarning("Requested url: {0}", ex.Response.RequestMessage.RequestUri);
            _logger.LogWarning("With header: {0}", ex.Response.RequestMessage.Headers.Range);

            if (_settings.SensitiveLogging)
            {
                _logger.LogWarning("With Bearer token: {0}", ex.Response.RequestMessage.Headers.Authorization);
            }

            if (ex.Response.RequestMessage.Content is not null && ex.Response.RequestMessage.Method != HttpMethod.Get)
            {
                _logger.LogWarning("With Body: {0}", await ex.Response.RequestMessage.Content.ReadAsStringAsync());
            }
        }
    }
}
