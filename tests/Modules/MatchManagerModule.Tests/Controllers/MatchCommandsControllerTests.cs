using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MatchManagerModule.Controllers;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using Moq;

namespace MatchManagerModule.Tests.Controllers;

public class MatchCommandsControllerTests : CommandInteractionControllerTestBase<MatchCommandsController>
{
    private Mock<IOnlinePlayer> _player = new();
    private Mock<IMatchControlService> _matchControl = new();
    private Locale _locale;

    public MatchCommandsControllerTests()
    {
        _locale = Mocking.NewLocaleMock(this.ContextService.Object);
        InitMock(_player.Object, _matchControl, _locale);
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

    [Theory]
    [InlineData(0, PlayerTeam.Team1)]
    [InlineData(1, PlayerTeam.Team2)]
    public async Task Round_Points_Set_And_Audited(int team, PlayerTeam expectedTeam)
    {
        await Controller.SetRoundPointsAsync(team, 1337);
        
        _matchControl.Verify(m => m.SetTeamRoundPointsAsync(expectedTeam, 1337));
        AuditEventBuilder.Verify(m => m.Success());
        AuditEventBuilder.Verify(m => m.WithEventName(AuditEvents.TeamRoundPointsSet));
    }
    
    [Theory]
    [InlineData(0, PlayerTeam.Team1)]
    [InlineData(1, PlayerTeam.Team2)]
    public async Task Round_Map_Set_And_Audited(int team, PlayerTeam expectedTeam)
    {
        await Controller.SetMapPointsAsync(team, 1337);
        
        _matchControl.Verify(m => m.SetTeamMapPointsAsync(expectedTeam, 1337));
        AuditEventBuilder.Verify(m => m.Success());
        AuditEventBuilder.Verify(m => m.WithEventName(AuditEvents.TeamMapPointsSet));
    }
    
    [Theory]
    [InlineData(0, PlayerTeam.Team1)]
    [InlineData(1, PlayerTeam.Team2)]
    public async Task Round_Match_Set_And_Audited(int team, PlayerTeam expectedTeam)
    {
        await Controller.SetMatchPointsAsync(team, 1337);
        
        _matchControl.Verify(m => m.SetTeamMatchPointsAsync(expectedTeam, 1337));
        AuditEventBuilder.Verify(m => m.Success());
        AuditEventBuilder.Verify(m => m.WithEventName(AuditEvents.TeamMatchPointsSet));
    }

    [Fact]
    public async Task Pause_Match_Pauses_Match_And_Audits()
    {
        await Controller.PauseMatchAsync();
        
        _matchControl.Verify(m => m.PauseMatchAsync());
        AuditEventBuilder.Verify(m => m.Success());
        AuditEventBuilder.Verify(m => m.WithEventName(AuditEvents.MatchPaused));
    }
    
    [Fact]
    public async Task Unpause_Match_Unpauses_Match_And_Audits()
    {
        await Controller.UnpauseMatchAsync();
        
        _matchControl.Verify(m => m.UnpauseMatchAsync());
        AuditEventBuilder.Verify(m => m.Success());
        AuditEventBuilder.Verify(m => m.WithEventName(AuditEvents.MatchUnpaused));
    }
}
