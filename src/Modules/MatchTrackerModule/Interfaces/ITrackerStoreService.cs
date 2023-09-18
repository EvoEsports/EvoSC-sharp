using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchTrackerModule.Interfaces;

public interface ITrackerStoreService
{
    public Task SaveTimelineAsync(IMatchTimeline timeline);
    public Task SaveState(IMatchState state);
}
