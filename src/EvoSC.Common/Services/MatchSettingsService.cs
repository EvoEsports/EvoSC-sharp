using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using GbxRemoteNet.XmlRpc.ExtraTypes;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class MatchSettingsService : IMatchSettingsService
{
    private readonly ILogger<MatchSettingsService> _logger;
    private readonly IServerClient _server;
    
    public MatchSettingsService(ILogger<MatchSettingsService> logger, IServerClient server)
    {
        _logger = logger;
        _server = server;
    }
    
    public async Task SetModeScriptSettingsAsync(Action<DynamicObject> settingsAction)
    {
        var settings = await _server.Remote.GetModeScriptSettingsAsync();

        if (settings == null)
        {
            throw new InvalidOperationException("Failed to get current ModeScript settings.");
        }
        
        settingsAction(settings);
        await _server.Remote.SetModeScriptSettingsAsync(settings);
    }
}
