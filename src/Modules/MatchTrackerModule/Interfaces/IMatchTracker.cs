using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Models;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.MatchTrackerModule.Interfaces;

public interface IMatchTracker
{
    public bool IsTracking { get; }
    public IMatchTimeline LatestTimeline { get; }
    public MatchStatus Status { get; }
    
    public Task TrackScoresAsync(ScoresEventArgs scoreArgs);
    public Task TrackMapChangeAsync(MapGbxEventArgs mapArgs);

    public Task TrackCurrentMapAsync();
    public Task<Guid> BeginMatchAsync();
    public Task<IMatchTimeline> EndMatchAsync();
}
