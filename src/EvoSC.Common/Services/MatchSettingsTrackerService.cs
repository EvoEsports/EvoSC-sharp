using EvoSC.Common.Config.Models;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Arguments.MatchSettings;
using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Common.Services;

public class MatchSettingsTrackerService : IMatchSettingsTrackerService
{
    private IMatchSettings _currentMatchSettings;
    private readonly object _currentMatchSettingsLock = new();

    private readonly IEvoSCApplication _app;
    
    public MatchSettingsTrackerService(EventManager events, IEvoSCApplication app)
    {
        _app = app;
        
        events.Subscribe(s => s
            .WithEvent(MatchSettingsEvent.MatchSettingsLoaded)
            .WithInstance(this)
            .WithInstanceClass<MatchSettingsTrackerService>()
            .WithHandlerMethod<MatchSettingsEventArgs>(OnMatchSettingsLoaded)
            .WithPriority(EventPriority.High)
        );
    }

    private Task OnMatchSettingsLoaded(object sender, MatchSettingsEventArgs e)
    {
        lock (_currentMatchSettingsLock)
        {
            _currentMatchSettings = e.MatchSettings;
        }

        return Task.CompletedTask;
    }

    public IMatchSettings CurrentMatchSettings
    {
        get
        {
            lock (_currentMatchSettingsLock)
            {
                return _currentMatchSettings;
            }
        }
    }

    public async Task SetDefaultMatchSettingsAsync()
    {
        var evoscConfig = _app.Services.GetInstance<IEvoScBaseConfig>();
        var msService = _app.Services.GetInstance<IMatchSettingsService>();

        var ms = await msService.GetMatchSettingsAsync(evoscConfig.Path.DefaultMatchSettings);

        lock (_currentMatchSettingsLock)
        {
            _currentMatchSettings = ms;
        }
    }
}
