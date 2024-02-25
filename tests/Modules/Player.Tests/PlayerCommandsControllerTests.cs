using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.Player.Controllers;
using EvoSC.Modules.Official.Player.Interfaces;
using EvoSC.Testing.Controllers;
using NSubstitute;

namespace EvoSC.Modules.Official.Player.Tests;

public class PlayerCommandsControllerTests : CommandInteractionControllerTestBase<PlayerCommandsController>
{
    private readonly IPlayerService _playerService = Substitute.For<IPlayerService>();
    private readonly IOnlinePlayer _player = Substitute.For<IOnlinePlayer>();
    
    public PlayerCommandsControllerTests()
    {
        InitMock(_player, _playerService);
    }

    [Fact]
    public async Task Player_Is_Kicked()
    {
        await Controller.KickPlayerAsync(_player);
        await _playerService.Received().KickAsync(_player, Context.Player);
    }
    
    [Fact]
    public async Task Player_Is_Muted()
    {
        await Controller.MutePlayerAsync(_player);
        
        await _playerService.Received().MuteAsync(_player, Context.Player);
    }
    
    [Fact]
    public async Task Player_Is_UnMuted()
    {
        await Controller.UnMutePlayerAsync(_player);
        
        await _playerService.Received().UnmuteAsync(_player, Context.Player);
    }
    
    [Fact]
    public async Task Player_Is_Banned()
    {
        await Controller.BanPlayerAsync(_player);
        
        await _playerService.Received().BanAsync(_player, Context.Player);
    }
    
    [Fact]
    public async Task Player_Is_Unbanned()
    {
        await Controller.UnbanPlayerAsync("ThePlayerLogin");
        
        await _playerService.Received().UnbanAsync("ThePlayerLogin", Context.Player);
    }
}
