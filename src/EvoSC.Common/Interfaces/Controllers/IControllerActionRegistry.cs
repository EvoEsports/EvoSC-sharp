namespace EvoSC.Common.Interfaces.Controllers;

public interface IControllerActionRegistry
{
    public void RegisterForController(Type controllerType);
}
