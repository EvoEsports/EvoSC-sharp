using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Modules.Official.MotdModule.Models;
using Newtonsoft.Json;

namespace EvoSC.Modules.Official.MotdModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class HttpService : IHttpService, IDisposable
{
    private readonly HttpClient _httpClient;

    public HttpService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetAsync(string uri)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(uri);
        var result = await response.Content.ReadAsStringAsync();
        MotdResponse? responseObject = null;
        try
        {
            responseObject = JsonConvert.DeserializeObject<MotdResponse>(result);
        }
        catch (Exception) { }
        if (responseObject is null)
            return "";
        return responseObject.data.FirstOrDefault()?.message ?? "";
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
