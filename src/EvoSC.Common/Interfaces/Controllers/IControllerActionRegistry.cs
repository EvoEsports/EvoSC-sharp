namespace EvoSC.Common.Interfaces.Controllers;

public interface IControllerActionRegistry
{
    /// <summary>
    /// Register actions of a controller for the current registry.
    /// </summary>
    /// <param name="controllerType">The type of the controller's class.</param>
    public void RegisterForController(Type controllerType);
}
