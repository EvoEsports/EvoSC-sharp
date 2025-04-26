using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Stores;

namespace EvoSC.Modules.Official.MatchTrackerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class TrackerStoreService : ITrackerStoreService
{
    private readonly List<IMatchTrackerStore> _stores = new();
    
    public async Task SaveTimelineAsync(IMatchTimeline timeline)
    {
        foreach (var store in _stores)
        {
            await store.SaveTimelineAsync(timeline);
        }
    }

    public async Task SaveState(IMatchState state)
    {
        foreach (var store in _stores)
        {
            await store.SaveStateAsync(state);
        }
    }

    public void AddStore(IMatchTrackerStore store) => _stores.Add(store);
}
