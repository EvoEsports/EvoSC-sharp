using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using Moq;

namespace MatchManagerModule.Tests.Services;

public class MatchControlServiceTests
{
    private (
        IMatchControlService MatchControlService,
        (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote, Mock<IChatService> Chat) Server,
        Mock<IEventManager> EventManager
        ) NewMatchControlServiceMock()
    {
        var server = Mocking.NewServerClientMock();
        var events = new Mock<IEventManager>();

        var service = new MatchControlService(server.Client.Object, events.Object);

        return (
            service,
            server,
            events
        );
    }

    [Theory]
    [InlineData(PlayerTeam.Team1, "0")]
    [InlineData(PlayerTeam.Team2, "1")]
    public async Task SetTeamRoundPoints_Triggers_Correct_ModeScript_Callback(PlayerTeam team, string expectedTeam)
    {
        var mock = NewMatchControlServiceMock();

        await mock.MatchControlService.SetTeamRoundPointsAsync(team, 123);
        
        mock.Server.Remote.Verify(m => m.TriggerModeScriptEventArrayAsync("Trackmania.SetTeamPoints", expectedTeam, "123", "", ""));
    }
    
    [Theory]
    [InlineData(PlayerTeam.Team1, "0")]
    [InlineData(PlayerTeam.Team2, "1")]
    public async Task SetTeamMapPoints_Triggers_Correct_ModeScript_Callback(PlayerTeam team, string expectedTeam)
    {
        var mock = NewMatchControlServiceMock();

        await mock.MatchControlService.SetTeamMapPointsAsync(team, 123);
        
        mock.Server.Remote.Verify(m => m.TriggerModeScriptEventArrayAsync("Trackmania.SetTeamPoints", expectedTeam, "", "123", ""));
    }
    
    [Theory]
    [InlineData(PlayerTeam.Team1, "0")]
    [InlineData(PlayerTeam.Team2, "1")]
    public async Task SetTeamMatchPoints_Triggers_Correct_ModeScript_Callback(PlayerTeam team, string expectedTeam)
    {
        var mock = NewMatchControlServiceMock();

        await mock.MatchControlService.SetTeamMatchPointsAsync(team, 123);
        
        mock.Server.Remote.Verify(m => m.TriggerModeScriptEventArrayAsync("Trackmania.SetTeamPoints", expectedTeam, "", "", "123"));
    }

    [Fact]
    public async Task PauseMatch_Triggers_Pause_ModeScript_Method()
    {
        var mock = NewMatchControlServiceMock();

        await mock.MatchControlService.PauseMatchAsync();
        
        mock.Server.Remote.Verify(m => m.TriggerModeScriptEventArrayAsync("Maniaplanet.Pause.SetActive", "true"));
    }
    
    [Fact]
    public async Task UnpauseMatch_Triggers_Unpause_ModeScript_Method()
    {
        var mock = NewMatchControlServiceMock();

        await mock.MatchControlService.UnpauseMatchAsync();
        
        mock.Server.Remote.Verify(m => m.TriggerModeScriptEventArrayAsync("Maniaplanet.Pause.SetActive", "false"));
    }
}
