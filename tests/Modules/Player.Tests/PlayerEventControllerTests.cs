using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Modules.Official.Player.Controllers;
using EvoSC.Modules.Official.Player.Interfaces;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using Moq;

namespace EvoSC.Modules.Official.Player.Tests;

public class PlayerEventControllerTests : ControllerMock<PlayerEventController, IEventControllerContext>
{
    private Mock<IPlayerService> _playerService = new();

    public PlayerEventControllerTests()
    {
        InitMock(_playerService);
    }

    [Fact]
    public async Task Player_Is_Greeted_On_Join()
    {
        var args = new PlayerConnectGbxEventArgs {Login = "MyPlayerLogin"};
        await Controller.OnPlayerConnect(null, args);

        _playerService.Verify(s => s.UpdateAndGreetPlayerAsync(args.Login));
    }
}
