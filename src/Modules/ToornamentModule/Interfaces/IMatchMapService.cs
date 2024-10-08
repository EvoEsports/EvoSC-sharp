using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;

public interface IMatchMapService
{
    Task<List<IMap?>> AddMapsAsync(IPlayer player);
    Task<List<IMap?>> AddMapsFromNadeo(IPlayer player, IEnumerable<string> mapIds);
    Task<List<IMap?>> AddMapsFromTmx(IPlayer player, IEnumerable<int> mapIds);

}
