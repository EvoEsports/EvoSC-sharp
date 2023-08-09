using EvoSC.Common.Models;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces;

public interface IMatchTracker
{
    public IMatchTimeline LatestTimeline { get; }
    
    public Task TrackScoresAsync(ScoresEventArgs scoreArgs);
    public Task TrackChatMessageAsync();
    public Task BeginMatchAsync();
    public Task<IMatchTimeline> EndMatchAsync();
}
