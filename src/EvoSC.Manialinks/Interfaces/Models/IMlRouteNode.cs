namespace EvoSC.Manialinks.Interfaces.Models;

/// <summary>
/// Holds general information about a component within a route.
/// </summary>
public interface IMlRouteNode
{
    /// <summary>
    /// The name of this route node.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The action that is called if the route ends on this node.
    /// </summary>
    public IManialinkAction? Action { get; set; }
    
    /// <summary>
    /// Sub routes of the current route.
    /// </summary>
    public Dictionary<string, IMlRouteNode>? Children { get; set; }

    /// <summary>
    /// Whether this node is a route parameter.
    /// </summary>
    public bool IsParameter { get; set; }
    
    /// <summary>
    /// Whether this node has an action assigned to it.
    /// </summary>
    public bool IsAction => Action != null;
}
