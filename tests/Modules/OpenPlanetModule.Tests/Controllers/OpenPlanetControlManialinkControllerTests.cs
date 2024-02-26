using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Controllers;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using NSubstitute;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests.Controllers;

public class OpenPlanetControlManialinkControllerTests : ManialinkControllerTestBase<OpenPlanetControlManialinkController>
{
    private const string PlayerAccountId = "a467a996-eba5-44bf-9e2b-8543b50f39ae";
    private const string PlayerLogin = "pGepluulRL-eK4VDtQ85rg";
    private readonly IOnlinePlayer _player = Substitute.For<IOnlinePlayer>();
    private readonly IManialinkActionContext _actionContext = Substitute.For<IManialinkActionContext>();
    private readonly IOpenPlanetControlService _controlService = Substitute.For<IOpenPlanetControlService>();
    private readonly (IServerClient Server, IGbxRemoteClient Client) _server = Mocking.NewServerClientMock();
    private readonly IOpenPlanetTrackerService _trackerService = Substitute.For<IOpenPlanetTrackerService>();

    public OpenPlanetControlManialinkControllerTests()
    {
        _player.AccountId.Returns(PlayerAccountId);
        
        InitMock(_player, _actionContext, _controlService, _server.Item1, _trackerService);
    }

    [Fact]
    public async Task Player_Is_Verified_With_Correct_OP_Configuration()
    {
        var infoMock = Substitute.For<IOpenPlanetInfo>();
        await Controller.CheckAsync(infoMock);

        await _controlService.Received(1).VerifySignatureModeAsync(_player, infoMock);
        _trackerService.Received().AddOrUpdatePlayer(_player, infoMock);
    }

    [Fact]
    public async Task Player_Is_Kicked_On_Disconnect_Action()
    {
        await Controller.DisconnectAsync();
        
        await _server.Client.Received().KickAsync(PlayerLogin);
    }
}
