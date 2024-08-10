using EvoSC.Common.Events.Arguments;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
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
        var player = new Mock<IOnlinePlayer>();
        var args = new PlayerJoinedEventArgs
        {
            Player = player.Object,
            IsPlayerListUpdate = false,
            IsNewPlayer = false
        };
        
        await Controller.OnPlayerJoined(null, args);

        _playerService.Verify(s => s.GreetPlayerAsync(player.Object));
    }
    
    [Fact]
    public async Task Player_Is_Setup_On_First_Join()
    {
        var player = new Mock<IOnlinePlayer>();
        var args = new PlayerJoinedEventArgs
        {
            Player = player.Object,
            IsPlayerListUpdate = false,
            IsNewPlayer = true
        };
        
        await Controller.OnPlayerJoined(null, args);

        _playerService.Verify(s => s.SetupPlayerAsync(player.Object, false));
    }
}
