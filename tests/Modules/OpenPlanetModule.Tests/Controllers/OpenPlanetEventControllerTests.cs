using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.OpenPlanetModule.Controllers;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Models;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using GbxRemoteNet.Interfaces;
using Moq;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests.Controllers;

public class OpenPlanetEventControllerTests : EventControllerTestBase<OpenPlanetEventController>
{
    private const string PlayerAccountId = "a467a996-eba5-44bf-9e2b-8543b50f39ae";
    private const string PlayerLogin = "pGepluulRL-eK4VDtQ85rg";
    
    private readonly Mock<IOpenPlanetTrackerService> _trackerService = new();
    private readonly (Mock<IServerClient> Server, Mock<IGbxRemoteClient> Client) _server = Mocking.NewServerClientMock();
    private readonly Mock<IPlayerManagerService> _playerService = new();
    private Mock<IPlayer> _player = new();

    public OpenPlanetEventControllerTests()
    {
        _player.Setup(p => p.AccountId).Returns(PlayerAccountId);
        
        InitMock(_server.Server, _trackerService, _playerService);
    }

    [Fact]
    public async Task Player_Due_For_Kick_Is_Kicked()
    {
        await Controller.OnPlayerDueForKickAsync(null, new PlayerDueForKickEventArgs {Player = _player.Object});

        _server.Client.Verify(c => c.KickAsync(PlayerLogin), Times.Once);
    }

    [Fact]
    public async Task Player_Disconnected_Is_Removed_From_Tracker()
    {
        _playerService
            .Setup(s => s.GetPlayerAsync(PlayerAccountId))
            .Returns(Task.FromResult((IPlayer?)_player.Object));

        await Controller.OnPlayerDisconnectAsync(null,
            new PlayerDisconnectGbxEventArgs {Login = PlayerLogin, Reason = null});
        
        _trackerService.Verify(s => s.RemovePlayer(_player.Object), Times.Once);
    }
}
