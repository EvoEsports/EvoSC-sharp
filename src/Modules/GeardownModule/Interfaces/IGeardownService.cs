using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces;

public interface IGeardownService
{
    public Task SetupServerAsync(int matchId);
    public Task StartMatchAsync();
    public Task SendResultsAsync(string matchToken, IMatchTimeline argsTimeline);
}
