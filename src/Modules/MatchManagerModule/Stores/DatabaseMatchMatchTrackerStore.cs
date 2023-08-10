using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Stores;

namespace EvoSC.Modules.Official.MatchManagerModule.Stores;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class DatabaseMatchMatchTrackerStore : IDatabaseMatchTrackerStore
{
    private readonly IMatchRecordRepository _repository;
    
    public DatabaseMatchMatchTrackerStore(IMatchRecordRepository repository)
    {
        _repository = repository;
    }
    
    public async Task SaveTimelineAsync(IMatchTimeline timeline)
    {
        foreach (var state in timeline.States)
        {
            await SaveStateAsync(state);
        }
    }

    public Task SaveStateAsync(IMatchState state)
    {
        return _repository.InsertStateAsync(state);
    }

    public Task<IEnumerable<IMatchTimeline>> GetTimeLinesAsync()
    {
        throw new NotImplementedException();
    }
}
