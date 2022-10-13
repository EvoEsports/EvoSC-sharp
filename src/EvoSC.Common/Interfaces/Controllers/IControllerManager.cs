using EvoSC.Common.Controllers;

namespace EvoSC.Common.Interfaces.Controllers;

public interface IControllerManager
{
    public IEnumerable<ControllerInfo> Controllers { get; }

    public void AddController(Type controllerType, Guid moduleId);
    public void AddController<TController>(Guid moduleId) where TController : IController;
    public void AddControllerActionRegistry(IControllerActionRegistry registry);
    public ControllerInfo GetInfo(Type controllerType);
    public IController CreateInstance(Type controllerType);
}
