using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchTrackerModule.Interfaces;

public interface IMatchTracker
{
    public bool IsTracking { get; }
    public IMatchTimeline LatestTimeline { get; }
    
    public Task TrackScoresAsync(ScoresEventArgs scoreArgs);
    public Task<Guid> BeginMatchAsync();
    public Task<IMatchTimeline> EndMatchAsync();
}
