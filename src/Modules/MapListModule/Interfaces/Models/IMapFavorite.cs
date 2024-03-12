using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces.Models;

public interface IMapFavorite
{
    public IMap Map { get; }
    public IPlayer Player { get; }
}
