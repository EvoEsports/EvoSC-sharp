using EvoSC.Modules.Evo.GeardownModule.Models.API;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces;

public interface IGeardownMatchState
{
    public GdMatch Match { get; }
    public string MatchToken { get; }
}
