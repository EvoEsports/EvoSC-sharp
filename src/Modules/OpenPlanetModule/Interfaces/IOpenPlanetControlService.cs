using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Interfaces;

public interface IOpenPlanetControlService
{
    /// <summary>
    /// Verify that a player is allowed to play with their current OpenPlanet configuration.
    /// This method will jail or release the player depending on the outcome.
    /// </summary>
    /// <param name="player">Player to check.</param>
    /// <param name="playerOpInfo">The player's OpenPlanet tool information.</param>
    /// <returns></returns>
    public Task VerifySignatureModeAsync(IPlayer player, IOpenPlanetInfo playerOpInfo);
}
