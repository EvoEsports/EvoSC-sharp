using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MotdModule.Controllers;
using EvoSC.Modules.Official.MotdModule.Database.Models;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using Moq;
using Org.BouncyCastle.Utilities.Encoders;

namespace MotdModule.Tests;

public class MotdPlayerEventControllerTest : ControllerMock<MotdPlayerEventController, IEventControllerContext>
{
    private readonly Mock<IOnlinePlayer> _player = new();
    private readonly Mock<IPlayerManagerService> _playerManager = new();
    private readonly Mock<IMotdService> _motdService = new();
    private readonly Mock<IMotdRepository> _motdRepository = new();
    
    public MotdPlayerEventControllerTest()
    {
        InitMock(_playerManager, _motdService, _motdRepository);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(true, true)]
    async Task OnPlayerConnectTest(bool returnPlayer, bool hasEntry = false)
    {
        _playerManager.Setup(r => r.GetPlayerAsync(It.IsAny<string>()))
            .Returns((returnPlayer) ? Task.FromResult((IPlayer)_player.Object) : Task.FromResult<IPlayer>(null));
        if (hasEntry)
        {
            var entry = new MotdEntry() { PlayerId = _player.Object.Id, Hidden = true, DbPlayer = new DbPlayer(_player.Object)};
            _motdRepository.Setup(r => r.GetEntryAsync(It.IsAny<IPlayer>()))
                .Returns(Task.FromResult(entry));
            Assert.Equal(0, entry.PlayerId);
            Assert.Equal(_player.Object.Id, entry.Player.Id);
        }
        
        await Controller.OnPlayerConnect(null, new PlayerConnectGbxEventArgs() { Login = "F4aNYLSUS4iB3_Td_a4c8Q" });
        _playerManager.Verify(r => r.GetPlayerAsync(It.IsAny<string>()), Times.Once);
        _motdService.Verify(r => r.ShowAsync(It.IsAny<IPlayer>()), (returnPlayer && !hasEntry) ? Times.Once : Times.Never);
    }
}
