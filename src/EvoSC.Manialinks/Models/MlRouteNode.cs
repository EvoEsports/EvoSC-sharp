using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Models;

/// <summary>
/// Instantiate a new manialink route node.
/// </summary>
/// <param name="name">The name of the route component.</param>
public class MlRouteNode(string name) : IMlRouteNode
{
    public string Name { get; set; } = name;
    public IManialinkAction? Action { get; set; }
    public Dictionary<string, IMlRouteNode>? Children { get; set; }
    public bool IsParameter { get; set; }
}
