using EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces.Services;

public interface IGeardownApiService
{
    public IGeardownEventApi Events { get; }
    public IGeardownGroupApi Groups { get; }
    public IGeardownMatchApi Matches { get; }
}