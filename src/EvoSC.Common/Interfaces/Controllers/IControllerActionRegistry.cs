namespace EvoSC.Common.Interfaces.Controllers;

public interface IControllerActionRegistry
{
    /// <summary>
    /// Register actions of a controller for the current registry.
    /// </summary>
    /// <param name="controllerType"></param>
    public void RegisterForController(Type controllerType);
}
