using EvoSC.Common.Interfaces.Controllers;
using Moq;

namespace EvoSC.Testing.Controllers;

public class ControllerMock<TController, TContext> : ControllerContextMock<TContext>
    where TController : class, IController
    where TContext : class, IControllerContext
{
    private TController _controller;

    public TController Controller => _controller;

    public virtual void InitMock(params Mock[] services)
    {
        _controller = Mocking.NewControllerMock<TController, TContext>(this, services);
    }
}
