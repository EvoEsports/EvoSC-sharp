using EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces.Services;

public interface IGeardownApiService
{
    public IGeardownMatchApi Matches { get; }
}
