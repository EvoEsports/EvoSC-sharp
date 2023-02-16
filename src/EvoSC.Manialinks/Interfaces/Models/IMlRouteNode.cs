namespace EvoSC.Manialinks.Interfaces.Models;

public interface IMlRouteNode
{
    public string Name { get; set; }
    public IManialinkAction? Action { get; set; }
    public Dictionary<string, IMlRouteNode>? Children { get; set; }

    public bool IsParameter { get; set; }
    public bool IsAction => Action != null;
}
