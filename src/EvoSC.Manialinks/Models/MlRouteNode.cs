using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Models;

public class MlRouteNode : IMlRouteNode
{
    public string Name { get; set; }
    public IManialinkAction? Action { get; set; }
    public Dictionary<string, IMlRouteNode>? Children { get; set; }
    public bool IsParameter { get; set; }

    /// <summary>
    /// Instantiate a new manialink route node.
    /// </summary>
    /// <param name="name">The name of the route component.</param>
    public MlRouteNode(string name)
    {
        Name = name;
    }
}
