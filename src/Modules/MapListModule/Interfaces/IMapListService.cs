using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces;

public interface IMapListService
{
    public Task<IEnumerable<IMapListMap>> GetCurrentMapsForPlayerAsync(IPlayer player);
}
