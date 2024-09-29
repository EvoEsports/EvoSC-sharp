using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
public interface INadeoMapService
{
    /// <summary>
    /// Downloads a map from Nadeo servers if it exists with the given ID.
    /// </summary>
    /// <param name="mapId">The maps ID</param>
    /// <returns></returns>
    Task<IMap?> FindAndDownloadMapAsync(string mapId);
}
