using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Stores;

namespace EvoSC.Modules.Official.MatchTrackerModule.Stores;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class DatabaseMatchMatchTrackerStore(IMatchRecordRepository repository) : IDatabaseMatchTrackerStore
{
    public async Task SaveTimelineAsync(IMatchTimeline timeline)
    {
        foreach (var state in timeline.States)
        {
            await SaveStateAsync(state);
        }
    }

    public Task SaveStateAsync(IMatchState state)
    {
        return repository.InsertStateAsync(state);
    }

    public Task<IEnumerable<IMatchTimeline>> GetTimeLinesAsync()
    {
        throw new NotImplementedException();
    }
}
