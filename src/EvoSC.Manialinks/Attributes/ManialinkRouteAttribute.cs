namespace EvoSC.Manialinks.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ManialinkRouteAttribute : Attribute
{
    public required string Route { get; set; }
    public object? Permission { get; init; }
}
