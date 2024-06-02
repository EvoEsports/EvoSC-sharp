using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces.Models;

/// <summary>
/// Holds information about a map tag.
/// </summary>
public interface IMapTag
{
    /// <summary>
    /// ID of the tag.
    /// </summary>
    public int Id { get; }
    
    /// <summary>
    /// Name of the tag.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Maps assigned to this tag.
    /// </summary>
    public IEnumerable<IMap> Maps { get; }
}
