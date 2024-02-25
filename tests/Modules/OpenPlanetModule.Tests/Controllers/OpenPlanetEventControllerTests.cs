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
using NSubstitute;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests.Controllers;

public class OpenPlanetEventControllerTests : EventControllerTestBase<OpenPlanetEventController>
{
    private const string PlayerAccountId = "a467a996-eba5-44bf-9e2b-8543b50f39ae";
    private const string PlayerLogin = "pGepluulRL-eK4VDtQ85rg";
    
    private readonly IOpenPlanetTrackerService _trackerService = Substitute.For<IOpenPlanetTrackerService>();
    private readonly (IServerClient Server, IGbxRemoteClient Client) _server = Mocking.NewServerClientMock();
    private readonly IPlayerManagerService _playerService = Substitute.For<IPlayerManagerService>();
    private readonly IPlayer _player = Substitute.For<IPlayer>();

    public OpenPlanetEventControllerTests()
    {
        _player.AccountId.Returns(PlayerAccountId);
        
        InitMock(_server.Server, _trackerService, _playerService);
    }

    [Fact]
    public async Task Player_Due_For_Kick_Is_Kicked()
    {
        await Controller.OnPlayerDueForKickAsync(null, new PlayerDueForKickEventArgs {Player = _player});

        await _server.Client.Received(1).KickAsync(PlayerLogin);
    }

    [Fact]
    public async Task Player_Disconnected_Is_Removed_From_Tracker()
    {
        _playerService
            .GetPlayerAsync(PlayerAccountId)
            .Returns(Task.FromResult((IPlayer?)_player));

        await Controller.OnPlayerDisconnectAsync(null,
            new PlayerDisconnectGbxEventArgs {Login = PlayerLogin, Reason = null});
        
        _trackerService.Received(1).RemovePlayer(_player);
    }
}
