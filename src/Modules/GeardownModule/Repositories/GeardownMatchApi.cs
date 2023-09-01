using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;
using EvoSC.Modules.Evo.GeardownModule.Models;
using EvoSC.Modules.Evo.GeardownModule.Models.API;
using EvoSC.Modules.Evo.GeardownModule.Settings;
using Hawf.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EvoSC.Modules.Evo.GeardownModule.Repositories;

[ApiClient]
public class GeardownMatchApi : GeardownApiBase<GeardownMatchApi>, IGeardownMatchApi
{
    
    
    /* private GeardownHttpClient _client;

    private static string s_matchToken = "";

    public GeardownMatchApi(GeardownHttpClient geardownHttpClient)
    {
        _client = geardownHttpClient;
    }

    public async Task<List<GdParticipant>> GetParticipants(int matchId)
    {
        var response = await _client.Get("/v1/matches/participants", new[]
        {
            new KeyValuePair<string, string>("matchId", matchId.ToString())
        });

        return JsonConvert.DeserializeObject<List<GdParticipant>>(response);
    }

    public async Task<bool> UpdateStatus(int matchId, MatchStatus statusId)
    {
        var response = "";

        try
        {
            response = await _client.Put("/v1/matches/" + matchId + "/status/" + statusId, new object { });
        }
        catch (Exception)
        {
            return false;
        }

        dynamic data = JObject.Parse(response);

        if (data.message)
        {
            return true;
        }

        return false;
    }

    public async Task<GdMatch> UpdateParticipants(int matchId, List<GdParticipant> participants)
    {
        var response = await _client.Put("/v1/matches/participants", new object[]
        {
            new KeyValuePair<string, int>("matchId", matchId),
            new KeyValuePair<string, List<GdParticipant>>("matchId", participants),
        });

        return JsonConvert.DeserializeObject<GdMatch>(response);
    }

    public async Task<GdMatchResult> CreateMatchResult(int matchId, bool isTotalResult, int participantId, string result,
        bool pending)
    {
        var response = await _client.Post("/v1/matches/results", new object[]
        {
            new KeyValuePair<string, int>("matchId", matchId),
            new KeyValuePair<string, bool>("isTotalResult", isTotalResult),
            new KeyValuePair<string, int>("participantId", participantId),
            new KeyValuePair<string, string>("result", result),
            new KeyValuePair<string, bool>("pending", pending),
        });

        return JsonConvert.DeserializeObject<GdMatchResult>(response);
    }

    public async Task<GdMatchResult> CreateTimeResult(int matchId, string result, string nickname, string mapId)
    {
        var response = await _client.Post("/api/matches/time_results", new object[]
        {
            new KeyValuePair<string, int>("match_id", matchId),
            new KeyValuePair<string, string>("time", result),
            new KeyValuePair<string, string>("nickname", nickname),
        });

        return JsonConvert.DeserializeObject<GdMatchResult>(response);
    }

    public async Task<GdGameServer> AddGameServer(int matchId, string name, bool pending, string serverLink)
    {
        var response = await _client.Post("/v1/matches/game_servers", new object[]
        {
            new KeyValuePair<string, int>("matchId", matchId),
            new KeyValuePair<string, string>("name", name),
            new KeyValuePair<string, bool>("pending", pending),
            new KeyValuePair<string, string>("serverLink", serverLink),
        });

        return JsonConvert.DeserializeObject<GdGameServer>(response);
    }

    public async Task<GdMatch> getMatchDataByToken(string matchToken)
    {
        s_matchToken = matchToken;
        var response = await _client.Get("/api/matches/evo_token/" + matchToken, null);
        System.Console.WriteLine(response);
        return JsonConvert.DeserializeObject<GdMatch>(response);
    }

    public void OnEndRound(ScoresEventArgs args)
    {
        if (s_matchToken == "")
        {
            return;
        }

        var request = new GdOnEndRoundRequest();
        request.matchToken = s_matchToken;
        request.eventData = args;

        _client.Post("/v1/matches/on_end_round/", request);
    }

    public void OnEndMatch()
    {
        if (s_matchToken == "")
        {
            return;
        }

        var request = new GdOnEndMapRequest();
        request.matchToken = s_matchToken;

        _client.Post("/v1/matches/on_end_map/", request);
        s_matchToken = "";
    }

    public void OnStartMatch(string join)
    {
        if (s_matchToken == "")
        {
            return;
        }

        var request = new GdOnStartMatchRequest();
        request.matchToken = s_matchToken;
        request.join = join;

        _client.Post("/v1/matches/on_start_match/", request);
    } */
    public GeardownMatchApi(IGeardownSettings settings) : base(settings)
    {
    }

    public Task<IEnumerable<GdParticipant>?> GetParticipantsAsync(int matchId) =>
        WithAccessToken()
            .WithJsonBody(new { matchId })
            .GetJsonAsync<IEnumerable<GdParticipant>>("/v1/matches/participants");

    public Task UpdateStatusAsync(int matchId, MatchStatus statusId) =>
        WithAccessToken()
            .PutStringAsync("/v1/matches/{matchId}/status/{statusId}", matchId, statusId);

    public Task<GdMatch?> UpdateParticipantsAsync(int matchId, IEnumerable<GdParticipant> participants) =>
        WithAccessToken()
            .WithJsonBody(new { matchId, participants })
            .PutJsonAsync<GdMatch>("/v1/matches/participants");

    public Task<GdMatchResult?> CreateMatchResultAsync(int matchId, bool isTotalResult, int participantId,
        string result, bool pending) =>
        WithAccessToken()
            .WithJsonBody(new { matchId, isTotalResult, participantId, result, pending })
            .PostJsonAsync<GdMatchResult>("/matches/results");

    public Task<GdMatchResult?> CreateTimeResultAsync(int matchId, string result, string nickname, string mapId) =>
        WithAccessToken()
            .WithJsonBody(new { matchId, result, nickname, mapId })
            .PostJsonAsync<GdMatchResult>("/matches/time_results");

    public Task<GdGameServer?> AddGameServerAsync(int matchId, string name, bool pending, string serverLink) =>
        WithAccessToken()
            .WithJsonBody(new { matchId, name, pending, serverLink })
            .PostJsonAsync<GdGameServer>("/v1/matches/game_servers");

    public Task<GdMatch?> GetMatchDataByTokenAsync(string matchToken) =>
        WithAccessToken()
            .GetJsonAsync<GdMatch>("/matches/evo_token/{matchToken}", matchToken);

    public Task OnEndRoundAsync(string matchToken, ScoresEventArgs eventData) =>
        WithAccessToken()
            .WithJsonBody(new { matchToken, eventData })
            .PostStringAsync("/v1/matches/on_end_round");

    public Task OnEndMatchAsync(string matchToken) =>
        WithAccessToken()
            .WithJsonBody(new { matchToken })
            .PostStringAsync("/v1/matches/on_end_map");

    public Task OnStartMatchAsync(string matchToken, string join) =>
        WithAccessToken()
            .WithJsonBody(new { matchToken, join })
            .PostStringAsync("/v1/matches/on_start_match");

    public Task AddResultsAsync(int matchId, IEnumerable<GdResult> results) =>
        WithAccessToken()
            .WithJsonBody(new { matchId = matchId, results = results })
            .PostStringAsync("/console/dedicated_controllers/results");
}
