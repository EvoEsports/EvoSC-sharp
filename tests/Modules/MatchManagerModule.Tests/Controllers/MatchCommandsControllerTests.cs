using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MatchManagerModule.Controllers;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using NSubstitute;

namespace MatchManagerModule.Tests.Controllers;

public class MatchCommandsControllerTests : CommandInteractionControllerTestBase<MatchCommandsController>
{
    private readonly IOnlinePlayer _player = Substitute.For<IOnlinePlayer>();
    private readonly IMatchControlService _matchControl = Substitute.For<IMatchControlService>();
    private readonly (IServerClient Client, IGbxRemoteClient Remote) _server = Mocking.NewServerClientMock();

    public MatchCommandsControllerTests()
    {
        var locale = Mocking.NewLocaleMock(this.ContextService);
        InitMock(_player, _matchControl, _server.Client, locale);
    }

    [Fact]
    public async Task Match_Is_Started_And_Audited()
    {
        var id = Guid.NewGuid();
        _matchControl.StartMatchAsync().Returns(Task.FromResult(id));
        
        await Controller.StartMatchAsync();
        
        await _matchControl.Received(1).StartMatchAsync();
        AuditEventBuilder.Received(1).Success();
    }

    [Fact]
    public async Task Match_Is_Ended_And_Audited()
    {
        var id = Guid.NewGuid();
        _matchControl.EndMatchAsync().Returns(Task.FromResult(id));
        
        await Controller.EndMatchAsync();
        
        await _matchControl.Received(1).EndMatchAsync();
        AuditEventBuilder.Received(1).Success();
    }

    [Fact]
    public async Task Match_Is_Restarted_And_Audited()
    {
        await Controller.RestartMatchAsync();
        
        await _matchControl.Received(1).RestartMatchAsync();
        AuditEventBuilder.Received(1).Success();
    }

    [Fact]
    public async Task Round_Is_Ended_And_Audited()
    {
        await Controller.EndRoundAsync();
        
        await _matchControl.Received(1).EndRoundAsync();
        AuditEventBuilder.Received(1).Success();
    }
    
    [Fact]
    public async Task Map_Is_Skipped_And_Audited()
    {
        await Controller.SkipMapAsync();
        
        await _matchControl.Received(1).SkipMapAsync();
        AuditEventBuilder.Received(1).Success();
    }
}
