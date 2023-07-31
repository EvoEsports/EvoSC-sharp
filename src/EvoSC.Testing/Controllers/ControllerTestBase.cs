using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Testing.Controllers;

public class ControllerTestBase<TController, TContext> : ControllerContextMock<TContext>
    where TController : class, IController
    where TContext : class, IControllerContext
{
    public ControllerTestBase()
    {
        
    }
}
