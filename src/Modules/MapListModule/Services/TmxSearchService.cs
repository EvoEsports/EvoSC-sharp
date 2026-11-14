using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Modules.Official.MapListModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MapListModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class TmxSearchService(ILogger<TmxSearchService> logger) : ITmxSearchService
{
    private const string Fields = "MapId,MapUid,Name,Authors[],Tags[],Difficulty,Length,AwardCount,UploadedAt";

    private static readonly string[] Difficulties =
        ["Beginner", "Intermediate", "Advanced", "Expert", "Lunatic", "Impossible"];

    private static readonly HttpClient Http = CreateClient();

    private static HttpClient CreateClient()
    {
        var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        client.DefaultRequestHeaders.UserAgent.ParseAdd("EvoSC#");
        return client;
    }

    public async Task<TmxSearchResult?> SearchAsync(string query, int page)
    {
        // The search has no page offset, so fetch everything up to the requested page (plus one
        // extra to know if there is a next page) and keep only the last page.
        var count = (page + 1) * ITmxSearchService.PageSize;
        query = query.Trim();
        var baseUrl = $"https://trackmania.exchange/api/maps?fields={Uri.EscapeDataString(Fields)}" +
                      $"&count={count + 1}&order1=6";

        try
        {
            // A tag name (e.g. "fullspeed") searches by tag, a number searches by TMX ID, anything else by name.
            var tagId = await FindTagIdAsync(query);
            List<TmxMapInfo> results;

            if (tagId != null)
            {
                results = await FetchAsync($"{baseUrl}&tag={tagId}");
            }
            else if (int.TryParse(query, out var mapId))
            {
                results = await FetchAsync($"{baseUrl}&id={mapId}");
                if (results.Count == 0)
                {
                    results = await FetchAsync($"{baseUrl}&name={Uri.EscapeDataString(query)}");
                }
            }
            else
            {
                results = await FetchAsync($"{baseUrl}&name={Uri.EscapeDataString(query)}");
            }

            var pageMaps = results.Skip(page * ITmxSearchService.PageSize).Take(ITmxSearchService.PageSize).ToList();

            return new TmxSearchResult
            {
                Maps = pageMaps,
                HasMore = results.Count > (page + 1) * ITmxSearchService.PageSize
            };
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            logger.LogError(ex, "Failed to search Trackmania Exchange for '{Query}'", query);
            return null;
        }
    }

    private static async Task<List<TmxMapInfo>> FetchAsync(string url)
    {
        using var response = await Http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<TmxMapsResponse>()
                   ?? throw new JsonException("Empty response from Trackmania Exchange.");

        return body.Results.Select(ToMapInfo).ToList();
    }

    private static async Task<int?> FindTagIdAsync(string query)
    {
        var tags = _tags;
        if (tags == null)
        {
            using var response = await Http.GetAsync("https://trackmania.exchange/api/meta/tags");
            response.EnsureSuccessStatusCode();

            var apiTags = await response.Content.ReadFromJsonAsync<List<TmxTagDto>>() ?? [];

            tags = apiTags
                .GroupBy(t => Normalize(t.Name))
                .ToDictionary(g => g.Key, g => g.First().Id);
            _tags = tags;
        }

        return tags.TryGetValue(Normalize(query), out var id) ? id : null;
    }

    private static string Normalize(string value) => value.Replace(" ", "").ToLowerInvariant();

    private static Dictionary<string, int>? _tags;

    private static TmxMapInfo ToMapInfo(TmxMapDto dto) => new()
    {
        Id = dto.MapId,
        Uid = dto.MapUid ?? "",
        Name = dto.Name ?? "",
        Authors = string.Join(", ", dto.Authors.Select(a => a.User.Name)),
        Tags = dto.Tags.Select(t => t.Name).ToList(),
        Difficulty = dto.Difficulty >= 0 && dto.Difficulty < Difficulties.Length
            ? Difficulties[dto.Difficulty]
            : "Unknown",
        Length = TimeSpan.FromMilliseconds(dto.Length),
        Awards = dto.AwardCount,
        UploadedAt = DateTime.TryParse(dto.UploadedAt, CultureInfo.InvariantCulture, DateTimeStyles.None,
            out var uploaded)
            ? uploaded
            : null
    };

    private record TmxMapsResponse(List<TmxMapDto> Results);

    private record TmxMapDto(
        int MapId,
        string? MapUid,
        string? Name,
        List<TmxAuthorDto> Authors,
        List<TmxTagDto> Tags,
        int Difficulty,
        int Length,
        int AwardCount,
        string? UploadedAt);

    private record TmxAuthorDto(TmxUserDto User);

    private record TmxUserDto(string Name);

    private record TmxTagDto([property: JsonPropertyName("ID")] int Id, string Name);
}
