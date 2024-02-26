using EvoSC.Common.Controllers;
using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Testing.Tests.TestClasses;

public class TestControllerWithServices : EvoScController<IPlayerInteractionContext>
{
    private readonly ITestService _service;

    public TestControllerWithServices(ITestService service)
    {
        _service = service;
    }
    
    public Task DoSomething()
    {
        _service.DoSomethingElse();
        return Task.CompletedTask;
    }
}
