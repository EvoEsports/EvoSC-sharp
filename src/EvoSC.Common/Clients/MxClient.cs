using System.Text.Json;
using EvoSC.Common.Clients.Dtos;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Clients;

public class MxClient
{
    private const string MxUrl = "https://trackmania.exchange";

    private readonly ILogger<MxClient> _logger;
    private readonly HttpClient Client;

    public MxClient(ILogger<MxClient> logger)
    {
        _logger = logger;

        // initialize http client
        Client = new HttpClient();
        Client.DefaultRequestHeaders.Add("User-Agent", "EvoSC# 0.0.1");
    }

    /// <summary>
    /// Gets a map from Trackmania Exchange with a given MX ID. ShortName can be added if the map is unlisted.
    /// </summary>
    /// <param name="mxId">ManiaExchange ID.</param>
    /// <param name="shortName">Needed to download unlisted maps.</param>
    /// <returns>Mapfile, .Gbx format.</returns>
    public async Task<Stream> GetMapAsync(int mxId, string? shortName)
    {
        var downloadMapPath = MxUrl + $"/maps/download/{mxId}";
        if (shortName != null)
        {
            downloadMapPath += $"/{shortName}";
        }

        return await Client.GetStreamAsync(downloadMapPath);
    }

    /// <summary>
    /// Gets a mappack from Trackmania Exchange with a given MX ID. Secret can be added if the mappack is unlisted.
    /// </summary>
    /// <param name="mxId">ManiaExchange ID.</param>
    /// <param name="secret">Mappack Secret.</param>
    /// <returns>A zip-file containing the maps within the mappack.</returns>
    public async Task<Stream> GetMappackAsync(int mxId, string? secret)
    {
        var downloadMappackPath = MxUrl + $"/mappack/download/{mxId}";
        if (secret != null)
        {
            downloadMappackPath += $"/{secret}";
        }

        return await Client.GetStreamAsync(downloadMappackPath);
    }

    /// <summary>
    /// Gets map information from Trackmania Exchange with a given MX ID. ShortName can be added if the map is unlisted.
    /// </summary>
    /// <param name="mxId">ManiaExchange ID.</param>
    /// <param name="shortName">Needed to download unlisted maps.</param>
    /// <returns><see cref="MxMapInfoDto"/></returns>
    public async Task<MxMapInfoDto> GetMapInfoAsync(int mxId, string? shortName)
    {
        var mapInfoPath = MxUrl + $"/api/maps/get_map_info/id/{mxId}";
        
        if (shortName != null)
        {
            mapInfoPath += $"/{shortName}";
        }

        var response = await Client.GetStreamAsync(mapInfoPath);
        var mapInfo = await JsonSerializer.DeserializeAsync<MxMapInfoDto>(response);

        if (mapInfo == null)
        {
            throw new NullReferenceException("Retrieved map info from TMX was empty or null.");
        }

        return mapInfo;
    }
}
