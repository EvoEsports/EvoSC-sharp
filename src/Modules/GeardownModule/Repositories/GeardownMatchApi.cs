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
            .PostJsonAsync<GdMatchResult>("/api/matches/results");

    public Task<GdMatchResult?> CreateTimeResultAsync(int matchId, string result, string nickname, string mapId) =>
        WithAccessToken()
            .WithJsonBody(new { matchId, result, nickname, mapId })
            .PostJsonAsync<GdMatchResult>("/api/matches/time_results");

    public Task<GdMatchToken?> AssignServerAsync(int matchId, string name, string serverId, string? serverPassword) =>
        WithAccessToken()
            .WithJsonBody(new { serverId, password = serverPassword, name })
            .PostJsonAsync<GdMatchToken>("/api/matches/game_server/{matchId}", matchId);

    public Task<GdMatch?> GetMatchDataByTokenAsync(string matchToken) =>
        WithAccessToken()
            .GetJsonAsync<GdMatch>("/api/matches/evo_token/{matchToken}", matchToken);

    public Task OnEndMatchAsync(string matchToken) =>
        WithAccessToken()
            .WithJsonBody(new { matchToken })
            .PostStringAsync("/v1/matches/on_end_match");

    public Task OnStartMatchAsync(string matchToken) =>
        WithAccessToken()
            .WithJsonBody(new { matchToken })
            .PostStringAsync("/v1/matches/on_start_match");

    public Task AddResultsAsync(int matchId, IEnumerable<GdResult> results) =>
        WithAccessToken()
            .WithJsonBody(new { matchId = matchId, results = results })
            .PostStringAsync("/api/console/dedicated_controllers/results");
}
