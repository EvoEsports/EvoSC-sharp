using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.Maps.Interfaces;

public interface INextMapService
{
    public Task<IMap> GetNextMapAsync();
}
