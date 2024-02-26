using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Modules.Official.Player.Controllers;
using EvoSC.Modules.Official.Player.Interfaces;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using NSubstitute;

namespace EvoSC.Modules.Official.Player.Tests;

public class PlayerEventControllerTests : ControllerMock<PlayerEventController, IEventControllerContext>
{
    private readonly IPlayerService _playerService = Substitute.For<IPlayerService>();

    public PlayerEventControllerTests()
    {
        InitMock(_playerService);
    }

    [Fact]
    public async Task Player_Is_Greeted_On_Join()
    {
        var args = new PlayerConnectGbxEventArgs {Login = "MyPlayerLogin"};
        await Controller.OnPlayerConnect(null, args);

        await _playerService.Received().UpdateAndGreetPlayerAsync(args.Login);
    }
}
