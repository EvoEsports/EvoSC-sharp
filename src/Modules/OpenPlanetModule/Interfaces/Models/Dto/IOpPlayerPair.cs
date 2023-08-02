using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models.Dto;

public interface IOpPlayerPair
{
    /// <summary>
    /// The player associated with this openplanet info.
    /// </summary>
    public IPlayer Player { get; }
    
    /// <summary>
    /// Information about the state of the player's OpenPlanet client.
    /// </summary>
    public IOpenPlanetInfo OpenPlanetInfo { get; }
}
