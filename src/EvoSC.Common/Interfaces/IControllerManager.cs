using EvoSC.Common.Controllers;

namespace EvoSC.Common.Interfaces;

public interface IControllerManager
{
    public IEnumerable<ControllerInfo> Controllers { get; }

    public void Add(Type controllerType);
    public void Add<TController>() where TController : IController;
    public void Remove(Type controllerType);
    public void Remove<TController>() where TController : IController;
}
