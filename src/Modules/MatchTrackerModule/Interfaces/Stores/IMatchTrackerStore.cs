using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Stores;

public interface IMatchTrackerStore
{
    public Task SaveTimelineAsync(IMatchTimeline timeline);
    public Task SaveStateAsync(IMatchState state);
    public Task<IEnumerable<IMatchTimeline>> GetTimeLinesAsync();
}
