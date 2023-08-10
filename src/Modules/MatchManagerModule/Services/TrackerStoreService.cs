using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Stores;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class TrackerStoreService : ITrackerStoreService
{
    private readonly List<IMatchTrackerStore> _stores = new();

    public TrackerStoreService(IDatabaseMatchTrackerStore dbStore)
    {
        _stores.Add(dbStore);
    }
    
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
}
