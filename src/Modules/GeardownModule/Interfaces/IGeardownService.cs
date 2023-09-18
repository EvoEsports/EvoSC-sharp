using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Models;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces;

public interface IGeardownService
{
    /// <summary>
    /// Begin automatic setup of the server. This is the initial setup phase.
    /// </summary>
    /// <param name="matchId">ID of the match to set the server up for.</param>
    /// <returns></returns>
    public Task SetupServerAsync(int matchId);
    
    /// <summary>
    /// Finish setting up the server. SetupServerAsync must be called first.
    /// </summary>
    /// <returns></returns>
    public Task FinishServerSetupAsync();
    
    /// <summary>
    /// Start the match now.
    /// </summary>
    /// <returns></returns>
    public Task StartMatchAsync();
    
    /// <summary>
    /// End the match and send the provided timeline as match results.
    /// </summary>
    /// <param name="timeline">Timeline representing the match's results.</param>
    /// <returns></returns>
    public Task EndMatchAsync(IMatchTimeline timeline);
    
    /// <summary>
    /// Get the current status of the match.
    /// </summary>
    /// <returns></returns>
    public MatchStatus GetMatchStatus();
}
