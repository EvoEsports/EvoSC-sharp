using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces;

public interface ITrackerStoreService
{
    public Task SaveTimelineAsync(IMatchTimeline timeline);
    public Task SaveState(IMatchState state);
}
