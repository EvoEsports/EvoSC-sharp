using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.Player.Controllers;
using EvoSC.Modules.Official.Player.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace EvoSC.Modules.Official.Player.Tests;

public class PlayerCommandsControllerTests : CommandInteractionControllerTestBase<PlayerCommandsController>
{
    private Mock<IPlayerService> _playerService = new();
    private Mock<IOnlinePlayer> _player = new();
    
    public PlayerCommandsControllerTests()
    {
        InitMock(_player.Object, _playerService);
    }

    [Fact]
    public async Task Player_Is_Kicked()
    {
        await Controller.KickPlayerAsync(_player.Object);
        _playerService.Verify(p => p.KickAsync(_player.Object, Context.Object.Player));
    }
    
    [Fact]
    public async Task Player_Is_Muted()
    {
        await Controller.MutePlayerAsync(_player.Object);
        
        _playerService.Verify(p => p.MuteAsync(_player.Object, Context.Object.Player));
    }
    
    [Fact]
    public async Task Player_Is_UnMuted()
    {
        await Controller.UnMutePlayerAsync(_player.Object);
        
        _playerService.Verify(p => p.UnmuteAsync(_player.Object, Context.Object.Player));
    }
    
    [Fact]
    public async Task Player_Is_Banned()
    {
        await Controller.BanPlayerAsync(_player.Object);
        
        _playerService.Verify(p => p.BanAsync(_player.Object, Context.Object.Player));
    }
    
    [Fact]
    public async Task Player_Is_Unbanned()
    {
        await Controller.UnbanPlayerAsync("ThePlayerLogin");
        
        _playerService.Verify(p => p.UnbanAsync("ThePlayerLogin", Context.Object.Player));
    }
}
