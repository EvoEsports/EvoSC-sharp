using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Modules.Official.MotdModule.Controllers;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using Moq;

namespace MotdModule.Tests;

public class MotdPlayerEventControllerTest : ControllerMock<MotdPlayerEventController, IEventControllerContext>
{
    private readonly Mock<IMotdService> _motdService = new();
    
    public MotdPlayerEventControllerTest()
    {
        InitMock(_motdService);
    }

    [Fact]
    public async Task OnPlayerConnect_Shows_Motd()
    {
        await Controller.OnPlayerConnectAsync(null, new PlayerConnectGbxEventArgs { Login = "F4aNYLSUS4iB3_Td_a4c8Q" });
        _motdService.Verify(r => r.ShowAsync(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
    }
}
