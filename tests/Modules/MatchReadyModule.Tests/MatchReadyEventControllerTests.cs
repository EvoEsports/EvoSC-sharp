using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Controllers;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using Microsoft.Extensions.Logging;
using Moq;

namespace MatchReadyModule.Tests;

public class MatchReadyEventControllerTests : EventControllerTestBase<MatchReadyEventController>
{
    private readonly Mock<IReadyService> _readyService = new();
    private readonly Mock<IPlayerManagerService> _players = new();
    private readonly Mock<IReadyManialinkService> _readyManialinkService = new();
    private readonly Mock<IMatchControlService> _matchControl = new();
    private readonly Mock<ILogger<MatchReadyEventController>> _logger = new();
    private readonly (Mock<IServerClient> Server, Mock<GbxRemoteNet.Interfaces.IGbxRemoteClient> Remote, Mock<IChatService> Chat) _server = Mocking.NewServerClientMock();

    public MatchReadyEventControllerTests()
    {
        InitMock(_readyService, _players, _readyManialinkService, _matchControl, _logger, _server.Server);
    }

    [Fact]
    public async Task AllPlayersReady_Starts_The_Match_When_Enabled()
    {
        _readyService.SetupGet(s => s.Enabled).Returns(true);

        await Controller.OnAllPlayersReadyAsync(new object(), EventArgs.Empty);

        _matchControl.Verify(m => m.StartMatchAsync(), Times.Once);
    }

    [Fact]
    public async Task AllPlayersReady_Disables_The_Ready_Service_After_Starting_The_Match()
    {
        _readyService.SetupGet(s => s.Enabled).Returns(true);

        await Controller.OnAllPlayersReadyAsync(new object(), EventArgs.Empty);

        _readyService.Verify(s => s.DisableAsync(), Times.Once);
    }

    [Fact]
    public async Task AllPlayersReady_Does_Nothing_When_Not_Enabled()
    {
        _readyService.SetupGet(s => s.Enabled).Returns(false);

        await Controller.OnAllPlayersReadyAsync(new object(), EventArgs.Empty);

        _matchControl.Verify(m => m.StartMatchAsync(), Times.Never);
        _readyService.Verify(s => s.DisableAsync(), Times.Never);
    }

    [Fact]
    public async Task AllPlayersReady_Does_Not_Disable_When_Starting_The_Match_Fails()
    {
        _readyService.SetupGet(s => s.Enabled).Returns(true);
        _matchControl.Setup(m => m.StartMatchAsync()).ThrowsAsync(new InvalidOperationException());

        await Controller.OnAllPlayersReadyAsync(new object(), EventArgs.Empty);

        _readyService.Verify(s => s.DisableAsync(), Times.Never);
    }
}
