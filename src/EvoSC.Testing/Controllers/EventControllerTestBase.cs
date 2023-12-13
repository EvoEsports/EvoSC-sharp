using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Testing.Controllers;

public class EventControllerTestBase<TController> : ControllerMock<TController, IEventControllerContext>
    where TController : class, IController;
