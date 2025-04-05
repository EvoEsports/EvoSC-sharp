using EvoSC.Common.Events;
using EvoSC.Common.Events.Arguments;
using EvoSC.Common.Events.Arguments.MatchSettings;
using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Common.Services;

public class MatchSettingsMaplistUpdateService : IMatchSettingsMaplistUpdateService
{
    private readonly IMatchSettingsService _matchSettingsService;
    
    public MatchSettingsMaplistUpdateService(EventManager events, IMatchSettingsService matchSettingsService)
    {
        _matchSettingsService = matchSettingsService;
        
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

    private Task OnMapUpdated(object sender, MapUpdatedEventArgs e) =>
        _matchSettingsService.EditCurrentMatchSettingsAsync(ms =>
        {
            ms.Maps.Remove(e.OldMap);
            ms.AddMap(e.Map);
        });

    private Task OnMapAdded(object sender, MapEventArgs e) =>
        _matchSettingsService.EditCurrentMatchSettingsAsync(ms => ms.AddMap(e.Map));

}
