using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Modules.Official.MotdModule.Controllers;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using NSubstitute;

namespace MotdModule.Tests;

public class MotdPlayerEventControllerTest : ControllerMock<MotdPlayerEventController, IEventControllerContext>
{
    private readonly IMotdService _motdService = Substitute.For<IMotdService>();
    
    public MotdPlayerEventControllerTest()
    {
        InitMock(_motdService);
    }

    [Fact]
    public async Task OnPlayerConnect_Shows_Motd()
    {
        await Controller.OnPlayerConnectAsync(null, new PlayerConnectGbxEventArgs { Login = "F4aNYLSUS4iB3_Td_a4c8Q" });
        await _motdService.Received(1).ShowAsync(Arg.Any<string>(), Arg.Any<bool>());
    }
}
