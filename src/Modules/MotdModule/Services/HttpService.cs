using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Modules.Official.MotdModule.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EvoSC.Modules.Official.MotdModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class HttpService : IHttpService, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpService> _logger;
    
    public bool IsDisposed { get; private set; }

    public HttpService(ILogger<HttpService> logger)
    {
        _httpClient = new HttpClient();
        _logger = logger;
        IsDisposed = false;
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
            _logger.LogError(ex.Message);
        }

        if (responseObject is null)
        {
            return "";
        }

        return responseObject.data?.FirstOrDefault()?.message ?? "";
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
