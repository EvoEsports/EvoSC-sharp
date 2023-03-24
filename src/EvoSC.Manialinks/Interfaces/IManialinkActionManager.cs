using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Interfaces;

/// <summary>
/// Controls registration and searching of Manialink Actions.
/// </summary>
public interface IManialinkActionManager : IControllerActionRegistry
{
    /// <summary>
    /// Register a new Manialink Action route to a handler.
    /// </summary>
    /// <param name="route">The route to register.</param>
    /// <param name="action">The action handler for this route.</param>
    public void AddRoute(string route, IManialinkAction action);
    
    /// <summary>
    /// Unregister and remove a route.
    /// </summary>
    /// <param name="route"></param>
    public void RemoveRoute(string route);
    
    /// <summary>
    /// Find an Action Handler for the provided input route. The input route
    /// is a route with parameters set.
    /// </summary>
    /// <param name="action">The input route to look for.</param>
    /// <returns></returns>
    public (IManialinkAction, IMlRouteNode) FindAction(string action);
}
