namespace EvoSC.Manialinks.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ManialinkRouteAttribute : Attribute
{
    public string? Route { get; init; }
    public object? Permission { get; init; }
}
