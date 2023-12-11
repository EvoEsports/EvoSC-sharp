using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Testing.Controllers;

public class ControllerMock<TController, TContext> : ControllerContextMock<TContext>
    where TController : class, IController
    where TContext : class, IControllerContext
{
    private TController _controller;

    /// <summary>
    /// The instance of the mocked controller.
    /// </summary>
    public TController Controller => _controller;

    /// <summary>
    /// Initialize this controller mock.
    /// </summary>
    /// <param name="services">Services for the controller. Can be Mock objects or plain objects.</param>
    public virtual void InitMock(params object[] services)
    {
        _controller = Mocking.NewControllerMock<TController, TContext>(this, services);
    }
}
