using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Models;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces;

public interface IGeardownService
{
    public Task SetupServerAsync(int matchId);
    public Task FinishServerSetupAsync();
    public Task StartMatchAsync();
    public Task EndMatchAsync(IMatchTimeline timeline);
    public MatchStatus GetMatchStatus();
}
