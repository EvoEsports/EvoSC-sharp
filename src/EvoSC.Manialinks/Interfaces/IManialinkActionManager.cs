using System.Reflection;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Interfaces;

public interface IManialinkActionManager : IControllerActionRegistry
{
    public void AddRoute(string route, IManialinkAction action);
    public void RemoveRoute(string route);
    public (IManialinkAction, IMlRouteNode) FindAction(string action);
}
