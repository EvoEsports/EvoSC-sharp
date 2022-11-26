using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models;

namespace EvoSC.Modules.Official.Maps.Interfaces;

public interface IMxMapService
{
    Task<Map?> FindAndDownloadMap(int mxId, string? shortName, IPlayer actor);
}
