using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Stores;

namespace EvoSC.Modules.Official.MatchTrackerModule.Interfaces;

public interface ITrackerStoreService
{
    public Task SaveTimelineAsync(IMatchTimeline timeline);
    public Task SaveState(IMatchState state);
    public void AddStore(IMatchTrackerStore store);
}
