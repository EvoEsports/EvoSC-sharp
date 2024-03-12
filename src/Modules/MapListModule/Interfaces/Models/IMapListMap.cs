using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces.Models;

public interface IMapListMap
{
    public IMap Map { get; }
    public IEnumerable<IMapTag> Tags { get; }
    public IEnumerable<IPlayerRecord?> Records { get; }

    public bool HasPb => Records.Any();
    public IPlayerRecord? Pb => Records.FirstOrDefault();
}
