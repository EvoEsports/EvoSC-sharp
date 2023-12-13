using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Modules.Official.MotdModule.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EvoSC.Modules.Official.MotdModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class HttpService(ILogger<HttpService> logger) : IHttpService, IDisposable
{
    private HttpClient _httpClient = new();

    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Only for unit testing!
    /// </summary>
    /// <param name="httpClient"></param>
    public void SetHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetAsync(string uri)
    {
        MotdResponse? responseObject = null;
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(new Uri(uri));
            var result = await response.Content.ReadAsStringAsync();
            responseObject = JsonConvert.DeserializeObject<MotdResponse>(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }

        if (responseObject is null)
        {
            return "";
        }

        return responseObject.Data?.FirstOrDefault()?.Message ?? "";
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        _httpClient.Dispose();
        IsDisposed = true;
    }
}
