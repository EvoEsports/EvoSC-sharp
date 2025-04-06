using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Arguments;
using EvoSC.Common.Events.Arguments.MatchSettings;
using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util;

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
            .WithInstanceClass<MatchSettingsTrackerService>()
            .WithHandlerMethod<MapEventArgs>(OnMapAdded)
            .WithPriority(EventPriority.High)
        );
        
        events.Subscribe(s => s
            .WithEvent(MapEvent.MapUpdated)
            .WithInstance(this)
            .WithInstanceClass<MatchSettingsTrackerService>()
            .WithHandlerMethod<MapUpdatedEventArgs>(OnMapUpdated)
            .WithPriority(EventPriority.High)
        );
    }

    private async Task OnMapUpdated(object sender, MapUpdatedEventArgs e)
    {
        await _matchSettingsService.EditCurrentMatchSettingsAsync(ms =>
        {
            ms.Maps.Remove(e.OldMap);
            ms.AddMap(e.Map);
        });
        
        await AddMapToLiveMapListAsync(e.Map);
    }

    private async Task OnMapAdded(object sender, MapEventArgs e)
    {
        await _matchSettingsService.EditCurrentMatchSettingsAsync(ms => ms.AddMap(e.Map));
        await AddMapToLiveMapListAsync(e.Map);
    }

    private async Task AddMapToLiveMapListAsync(IMap map)
    {
        await _serverClient.Remote.AddMapAsync(map.FilePath);
    }
}
