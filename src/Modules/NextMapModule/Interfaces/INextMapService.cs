using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.NextMapModule.Interfaces;

public interface INextMapService
{
    public Task<IMap> GetNextMapAsync();
}
