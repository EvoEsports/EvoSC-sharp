using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MotdModule.Controllers;
using EvoSC.Modules.Official.MotdModule.Database.Models;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using Moq;

namespace MotdModule.Tests;

public class MotdPlayerEventControllerTest : ControllerMock<MotdPlayerEventController, IEventControllerContext>
{
    private readonly Mock<IPlayerManagerService> _playerManager = new();
    private readonly Mock<IMotdService> _motdService = new();
    private readonly Mock<IMotdRepository> _motdRepository = new();
    
    public MotdPlayerEventControllerTest()
    {
        InitMock(_playerManager, _motdService, _motdRepository);
    }

    [Fact]
    private async Task On_Player_Connect_Test()
    {
        await Controller.OnPlayerConnectAsync(null, new PlayerConnectGbxEventArgs { Login = "F4aNYLSUS4iB3_Td_a4c8Q" });
        _motdService.Verify(r => r.ShowAsync(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
    }
}
