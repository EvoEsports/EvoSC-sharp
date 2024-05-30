using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces;

public interface IMapListService
{
    public Task<IEnumerable<IMapListMap>> GetCurrentMapsForPlayerAsync(IPlayer player);
    public Task DeleteMapAsync(IPlayer actor, string mapUid);

    public Task ShowMapListAsync(IPlayer player);
    public Task ConfirmMapDeletionsAsync(IPlayer player, IMap map);
}
