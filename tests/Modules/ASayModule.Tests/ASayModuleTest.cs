using EvoSC.Modules.Official.ASayModule.Interfaces;
using Moq;

namespace EvoSC.Modules.Official.ASayModule.Tests;

public class ASayModuleTest
{
    private readonly ASayModule _module;
    private readonly Mock<IASayService> _service = new();

    public ASayModuleTest()
    {
        _module = new ASayModule(_service.Object);
    }

    [Fact]
    async Task Should_Disable_Async()
    {
        await _module.DisableAsync();
        _service.Verify(service => service.OnDisable());
    }
}
