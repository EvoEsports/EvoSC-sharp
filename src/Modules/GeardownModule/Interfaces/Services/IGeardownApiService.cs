using EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces.Services;

public interface IGeardownApiService
{
    /// <summary>
    /// Manage Tourney matches.
    /// </summary>
    public IGeardownMatchApi Matches { get; }
}
