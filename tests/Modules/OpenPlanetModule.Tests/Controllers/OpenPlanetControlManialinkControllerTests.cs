using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Controllers;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Moq;

namespace OpenPlanetModule.Tests.Controllers;

public class OpenPlanetControlManialinkControllerTests : ManialinkControllerTestBase<OpenPlanetControlManialinkController>
{
    private const string PlayerAccountId = "a467a996-eba5-44bf-9e2b-8543b50f39ae";
    private const string PlayerLogin = "pGepluulRL-eK4VDtQ85rg";
    private readonly Mock<IOnlinePlayer> _player = new();
    private readonly Mock<IManialinkActionContext> _actionContext = new();
    private readonly Mock<IOpenPlanetControlService> _controlService = new();
    private readonly (Mock<IServerClient> Server, Mock<IGbxRemoteClient> Client) _server = Mocking.NewServerClientMock();
    private readonly Mock<IOpenPlanetTrackerService> _trackerService = new();

    public OpenPlanetControlManialinkControllerTests()
    {
        _player.Setup(p => p.AccountId).Returns(PlayerAccountId);
        
        InitMock(_player.Object, _actionContext.Object, _controlService, _server.Item1, _trackerService);
    }

    [Fact]
    public async Task Player_Is_Verified_With_Correct_OP_Configuration()
    {
        var infoMock = new Mock<IOpenPlanetInfo>();
        await Controller.CheckAsync(infoMock.Object);

        _controlService.Verify(s => s.VerifySignatureModeAsync(_player.Object, infoMock.Object), Times.Once);
        _trackerService.Verify(s => s.AddOrUpdatePlayer(_player.Object, infoMock.Object));
    }

    [Fact]
    public async Task Player_Is_Kicked_On_Disconnect_Action()
    {
        await Controller.DisconnectAsync();
        
        _server.Client.Verify(c => c.KickAsync(PlayerLogin));
    }
}
