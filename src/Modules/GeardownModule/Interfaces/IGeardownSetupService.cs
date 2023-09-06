using EvoSC.Modules.Evo.GeardownModule.Models.API;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces;

public interface IGeardownSetupService
{
    public Task<(GdMatch match, string token)> InitialSetupAsync(int matchId);

    public Task FinishSetupAsync();
}
