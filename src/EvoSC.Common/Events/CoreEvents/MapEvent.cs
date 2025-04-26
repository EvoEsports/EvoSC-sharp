namespace EvoSC.Common.Events.CoreEvents;

/// <summary>
/// Events fired for changes to maps.
/// </summary>
public enum MapEvent
{
    /// <summary>
    /// When a map was added to the server.
    /// </summary>
    MapAdded,
    
    /// <summary>
    /// When a map was updated.
    /// </summary>
    MapUpdated,
    
    /// <summary>
    /// When a map as removed.
    /// </summary>
    MapRemoved,
}
