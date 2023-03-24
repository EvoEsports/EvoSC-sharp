namespace EvoSC.Manialinks.Attributes;

/// <summary>
/// Allows to set a custom route or permission to the Manialink Action or all Manialink Actions within a controller.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ManialinkRouteAttribute : Attribute
{
    /// <summary>
    /// The route to this action or controller.
    /// </summary>
    public string? Route { get; init; }
    
    /// <summary>
    /// The permission required to execute this action. It can also be the default permission for all actions
    /// within the controller.
    /// </summary>
    public object? Permission { get; init; }
}
