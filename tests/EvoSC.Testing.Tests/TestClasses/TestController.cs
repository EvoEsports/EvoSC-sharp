using EvoSC.Common.Controllers;
using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Testing.Tests.TestClasses;

public class TestController : EvoScController<IPlayerInteractionContext>
{
    public Task DoingSomething()
    {
        Context.AuditEvent.Success();
        return Task.CompletedTask;
    }
}
