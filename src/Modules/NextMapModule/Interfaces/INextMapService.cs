using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.NextMapModule.Interfaces;

public interface INextMapService
{
    /// <summary>
    /// Get the next map from the server.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the next map could not be found.</exception>
    /// <returns></returns>
    public Task<IMap> GetNextMapAsync();
}
