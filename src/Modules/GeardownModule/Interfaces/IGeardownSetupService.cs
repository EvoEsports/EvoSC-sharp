using EvoSC.Modules.Evo.GeardownModule.Models.API;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces;

public interface IGeardownSetupService
{
    /// <summary>
    /// Initialize the server with a match.
    /// </summary>
    /// <param name="matchId">The match to set the server up for.</param>
    /// <returns></returns>
    public Task<(GdMatch match, string token)> InitialSetupAsync(int matchId);

    /// <summary>
    /// Finalize the server setup.
    /// </summary>
    /// <returns></returns>
    public Task FinalizeSetupAsync();
}
