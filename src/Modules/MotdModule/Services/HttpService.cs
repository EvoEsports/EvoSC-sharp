using System.Net;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Modules.Official.MotdModule.Models;
using Newtonsoft.Json;

namespace EvoSC.Modules.Official.MotdModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;

    public HttpService()
    {
        HttpClientHandler httpClientHandler = new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.All
        };
        _httpClient = new HttpClient();
    }

    public async Task<string> GetAsync(string uri)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(uri);
        var result = await response.Content.ReadAsStringAsync();
        MotdResponse? responseObject = JsonConvert.DeserializeObject<MotdResponse>(result);
        if (responseObject is null)
            return "";
        return responseObject.data.FirstOrDefault()?.message ?? "";
    }
}
