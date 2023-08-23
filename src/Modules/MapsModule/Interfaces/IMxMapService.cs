using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MapsModule.Interfaces;

public interface IMxMapService
{
    /// <summary>
    /// Downloads a map from TrackmaniaExchange if it exists with the given ID.
    /// </summary>
    /// <param name="mxId">The maps MXID</param>
    /// <param name="shortName">If the map is hidden, the short name have to be given for it to find the map.</param>
    /// <param name="actor">The player who started the map download.</param>
    /// <returns></returns>
    Task<IMap?> FindAndDownloadMapAsync(int mxId, string? shortName, IPlayer actor);
}
