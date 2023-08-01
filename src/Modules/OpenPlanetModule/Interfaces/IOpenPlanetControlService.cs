using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Interfaces;

public interface IOpenPlanetControlService
{
    public Task VerifySignatureModeAsync(IPlayer player, IOpenPlanetInfo playerOpInfo);
}
