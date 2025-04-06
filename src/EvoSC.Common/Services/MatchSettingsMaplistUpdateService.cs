using EvoSC.Common.Events;
using EvoSC.Common.Events.Arguments;
using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;

namespace EvoSC.Common.Services;

public class MatchSettingsMaplistUpdateService : IMatchSettingsMaplistUpdateService
{
    private readonly IMatchSettingsService _matchSettingsService;
    private readonly IServerClient _serverClient;
    
    public MatchSettingsMaplistUpdateService(IEventManager events, IMatchSettingsService matchSettingsService, IServerClient serverClient)
    {
        _matchSettingsService = matchSettingsService;
        _serverClient = serverClient;
        
        events.Subscribe(s => s
            .WithEvent(MapEvent.MapAdded)
            .WithInstance(this)
            .WithInstanceClass<MatchSettingsMaplistUpdateService>()
            .WithHandlerMethod<MapEventArgs>(OnMapAddedAsync)
            .WithPriority(EventPriority.High)
        );
        
        events.Subscribe(s => s
            .WithEvent(MapEvent.MapUpdated)
            .WithInstance(this)
            .WithInstanceClass<MatchSettingsMaplistUpdateService>()
            .WithHandlerMethod<MapUpdatedEventArgs>(OnMapUpdatedAsync)
            .WithPriority(EventPriority.High)
        );
    }

    private async Task OnMapUpdatedAsync(object sender, MapUpdatedEventArgs e)
    {
        await _matchSettingsService.EditCurrentMatchSettingsAsync(ms =>
        {
            ms.Maps.Remove(e.OldMap);
            ms.AddMap(e.Map);
        });
        
        await AddMapToLiveMapListAsync(e.Map);
    }

    private async Task OnMapAddedAsync(object sender, MapEventArgs e)
    {
        await _matchSettingsService.EditCurrentMatchSettingsAsync(ms => ms.AddMap(e.Map));
        await AddMapToLiveMapListAsync(e.Map);
    }

    private async Task AddMapToLiveMapListAsync(IMap map)
    {
        await _serverClient.Remote.AddMapAsync(map.FilePath);
    }
}
