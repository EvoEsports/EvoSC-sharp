using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MatchManagerModule.Controllers;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Moq;

namespace MatchManagerModule.Tests.Controllers;

public class MatchCommandsControllerTests : CommandInteractionControllerTestBase<MatchCommandsController>
{
    private Mock<IOnlinePlayer> _player = new();
    private Mock<IMatchControlService> _matchControl = new();
    private (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote) _server = Mocking.NewServerClientMock();
    private Locale _locale;

    public MatchCommandsControllerTests()
    {
        _locale = Mocking.NewLocaleMock(this.ContextService.Object);
        InitMock(_player.Object, _matchControl, _server.Client, _locale);
    }

    [Fact]
    public async Task Match_Is_Started_And_Audited()
    {
        var id = Guid.NewGuid();
        _matchControl.Setup(m => m.StartMatchAsync()).Returns(Task.FromResult(id));
        
        await Controller.StartMatchAsync();
        
        _matchControl.Verify(m => m.StartMatchAsync(), Times.Once);
        AuditEventBuilder.Verify(m => m.Success(), Times.Once);
    }

    [Fact]
    public async Task Match_Is_Ended_And_Audited()
    {
        var id = Guid.NewGuid();
        _matchControl.Setup(m => m.EndMatchAsync()).Returns(Task.FromResult(id));
        
        await Controller.EndMatchAsync();
        
        _matchControl.Verify(m => m.EndMatchAsync(), Times.Once);
        AuditEventBuilder.Verify(m => m.Success(), Times.Once);
    }

    [Fact]
    public async Task Match_Is_Restarted_And_Audited()
    {
        await Controller.RestartMatchAsync();
        
        _matchControl.Verify(m => m.RestartMatchAsync(), Times.Once);
        AuditEventBuilder.Verify(m => m.Success(), Times.Once);
    }

    [Fact]
    public async Task Round_Is_Ended_And_Audited()
    {
        await Controller.EndRoundAsync();
        
        _matchControl.Verify(m => m.EndRoundAsync(), Times.Once);
        AuditEventBuilder.Verify(m => m.Success(), Times.Once);
    }
    
    [Fact]
    public async Task Map_Is_Skipped_And_Audited()
    {
        await Controller.SkipMapAsync();
        
        _matchControl.Verify(m => m.SkipMapAsync(), Times.Once);
        AuditEventBuilder.Verify(m => m.Success(), Times.Once);
    }
}
