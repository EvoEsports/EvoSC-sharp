using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Evo.GeardownModule.Models.API;

namespace EvoSC.Modules.Evo.GeardownModule.Models;

public class GeardownMatchState : IGeardownMatchState
{
    public required GdMatch Match { get; init; }
    public required string MatchToken { get; init; }
}
