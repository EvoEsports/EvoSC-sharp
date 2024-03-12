using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces.Models;

public interface IMapTag
{
    public int Id { get; }
    
    public string Name { get; }
    
    public IEnumerable<IMap> Maps { get; }
}
