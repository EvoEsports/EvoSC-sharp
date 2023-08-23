using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models.Dto;

namespace EvoSC.Modules.Official.OpenPlanetModule.Interfaces;

public interface IOpenPlanetTrackerService
{
    public IEnumerable<IOpPlayerPair> Players { get; }

    public void AddOrUpdatePlayer(IPlayer player, IOpenPlanetInfo playerOpInfo);
    public void RemovePlayer(IPlayer player);
}
