using EvoSC.Common.Models;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces;

public interface IMatchTracker
{
    public bool IsTracking { get; }
    public IMatchTimeline LatestTimeline { get; }
    
    public Task TrackScoresAsync(ScoresEventArgs scoreArgs);
    public Task<Guid> BeginMatchAsync();
    public Task<IMatchTimeline> EndMatchAsync();
}
