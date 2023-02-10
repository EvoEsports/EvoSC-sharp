using SimpleInjector;

namespace EvoSC.Common.Interfaces.Controllers;

public interface IContextService
{
    internal IControllerContext CreateContext(Scope scope, IController controller);
    public IControllerContext GetContext();
}
