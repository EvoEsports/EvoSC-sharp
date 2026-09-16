using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
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
    private readonly Mock<ILogger<MatchReadyEventController>> _logger = new();
    private readonly (Mock<IServerClient> Server, Mock<GbxRemoteNet.Interfaces.IGbxRemoteClient> Remote, Mock<IChatService> Chat) _server = Mocking.NewServerClientMock();

    public MatchReadyEventControllerTests()
    {
        InitMock(_readyService, _players, _readyManialinkService, _logger, _server.Server);
    }

    [Fact]
    public async Task AllPlayersReady_Disables_The_Ready_Service_After_Starting_The_Match()
    {
        _readyService.SetupGet(s => s.Enabled).Returns(true);

        await Controller.OnAllPlayersReadyAsync(new object(), EventArgs.Empty);

        _readyService.Verify(s => s.DisableAsync(), Times.Once);
    }

    [Fact]
    public async Task MatchStarted_Disables_The_Ready_Service_When_Still_Enabled()
    {
        _readyService.SetupGet(s => s.Enabled).Returns(true);

        await Controller.OnMatchStartedAsync(new object(), EventArgs.Empty);

        _readyService.Verify(s => s.DisableAsync(), Times.Once);
        _readyManialinkService.Verify(s => s.HideWidgetAsync(), Times.Never);
    }

    [Fact]
    public async Task MatchStarted_Just_Hides_The_Widget_When_Already_Disabled()
    {
        _readyService.SetupGet(s => s.Enabled).Returns(false);

        await Controller.OnMatchStartedAsync(new object(), EventArgs.Empty);

        _readyService.Verify(s => s.DisableAsync(), Times.Never);
        _readyManialinkService.Verify(s => s.HideWidgetAsync(), Times.Once);
    }
}
