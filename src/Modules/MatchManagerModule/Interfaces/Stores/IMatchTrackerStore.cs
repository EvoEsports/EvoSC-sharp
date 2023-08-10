using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces.Stores;

public interface IMatchTrackerStore
{
    public Task SaveTimelineAsync(IMatchTimeline timeline);
    public Task SaveStateAsync(IMatchState state);
    public Task<IEnumerable<IMatchTimeline>> GetTimeLinesAsync();
}
