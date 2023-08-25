using Hawf.Client;

namespace EvoSC.Modules.Evo.GeardownModule.Repositories;

public class GeardownApiBase<T> : ApiBase<T> where T : ApiBase<T>
{
    private readonly IGeardownSettings _settings;
    
    public GeardownApiBase(IGeardownSettings settings)
    {
        _settings = settings;
        
        Configure(c =>
        {
            c.BaseUrl = settings.ApiBaseUrl;
            c.CacheResponse = false;
            c.UseRateLimit = false;
        });
    }

    protected T WithAccessToken() => 
        WithQueryParam("token", _settings.ApiAccessToken);
}
