using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models.Dto;

namespace EvoSC.Modules.Official.OpenPlanetModule.Models;

public class OpPlayerPair : IOpPlayerPair
{
    public required IPlayer Player { get; init; }
    public required IOpenPlanetInfo OpenPlanetInfo { get; init; }
}
