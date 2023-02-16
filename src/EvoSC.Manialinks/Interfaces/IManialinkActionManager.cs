using System.Reflection;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Interfaces;

public interface IManialinkActionManager
{
    public void AddActions(Type controllerType);
    public IManialinkAction FindAction(string action);
}
