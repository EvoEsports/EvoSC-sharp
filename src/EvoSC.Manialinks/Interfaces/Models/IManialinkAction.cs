using System.Reflection;

namespace EvoSC.Manialinks.Interfaces.Models;

/// <summary>
/// Holds information about a registered Manialink Action.
/// </summary>
public interface IManialinkAction
{
    /// <summary>
    /// Permission required to execute this manialink action.
    /// </summary>
    public string? Permission { get; }
    
    /// <summary>
    /// The type of the controller which the callback handler resides in.
    /// </summary>
    public Type ControllerType { get; }
    
    /// <summary>
    /// The callback handler for this action.
    /// </summary>
    public MethodInfo HandlerMethod { get; }
    
    /// <summary>
    /// Pointer to the first parameter of the callback.
    /// </summary>
    public IMlActionParameter FirstParameter { get; }
}
