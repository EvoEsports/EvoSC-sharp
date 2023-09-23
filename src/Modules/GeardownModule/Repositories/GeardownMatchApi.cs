using EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;
using EvoSC.Modules.Evo.GeardownModule.Models.API;
using EvoSC.Modules.Evo.GeardownModule.Settings;
using Flurl;
using Flurl.Http;
using Flurl.Util;

namespace EvoSC.Modules.Evo.GeardownModule.Repositories;

public class GeardownMatchApi : IGeardownMatchApi
{
    private readonly IGeardownSettings _settings;
    
    public GeardownMatchApi(IGeardownSettings settings)
    {
        _settings = settings;
    }

    /* public Task<GdMatchToken?> AssignServerAsync(int matchId, string name, string serverId, string? serverPassword) =>
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
            .PostStringAsync("/api/console/dedicated_controllers/results"); */
    public Task<GdMatchToken?> AssignServerAsync(int matchId, string name, string serverId, string? serverPassword)
    {
        return $"https://tourneyapi.evoesports.gg/api/matches/game_server/{matchId}"
            .SetQueryParam("token", _settings.ApiAccessToken)
            .PostJsonAsync(new { serverId, password = serverPassword, name })
            .ReceiveJson<GdMatchToken?>();
    }

    public Task<GdMatch?> GetMatchDataByTokenAsync(string matchToken)
    {
        return $"https://tourneyapi.evoesports.gg/api/matches/evo_token/{matchToken}"
            .WithHeader("Content-Type", "application/json")            .WithHeader("Content-Type", "application/json")
            .WithHeader("User-Agent", "EvoSC")
            .WithHeader("Connection", "keep-alive")
            .WithHeader("Accept-Encoding", "gzip, deflate, br")
            .SetQueryParam("token", _settings.ApiAccessToken)
            .GetAsync()
            .ReceiveJson<GdMatch?>();
    }

    public Task OnEndMatchAsync(string matchToken)
    {
        return $"https://tourneyapi.evoesports.gg/v1/matches/on_end_match"
            .WithHeader("Content-Type", "application/json")            .WithHeader("Content-Type", "application/json")
            .WithHeader("User-Agent", "EvoSC")
            .WithHeader("Connection", "keep-alive")
            .WithHeader("Accept-Encoding", "gzip, deflate, br")
            .SetQueryParam("token", _settings.ApiAccessToken)
            .PostAsync();
    }

    public Task OnStartMatchAsync(string matchToken)
    {
        return $"https://tourneyapi.evoesports.gg/v1/matches/on_start_match"
            .WithHeader("Content-Type", "application/json")            .WithHeader("Content-Type", "application/json")
            .WithHeader("User-Agent", "EvoSC")
            .WithHeader("Connection", "keep-alive")
            .WithHeader("Accept-Encoding", "gzip, deflate, br")
            .SetQueryParam("token", _settings.ApiAccessToken)
            .PostAsync();
    }

    public Task AddResultsAsync(int matchId, IEnumerable<GdResult> results)
    {
        return $"https://tourneyapi.evoesports.gg/api/matches/game_server/{matchId}"
            .WithHeader("Content-Type", "application/json")            .WithHeader("Content-Type", "application/json")
            .WithHeader("User-Agent", "EvoSC")
            .WithHeader("Connection", "keep-alive")
            .WithHeader("Accept-Encoding", "gzip, deflate, br")
            .SetQueryParam("token", _settings.ApiAccessToken)
            .PostJsonAsync(new { matchId = matchId, results = results });
    }
}
