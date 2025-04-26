using System.Text.Json.Nodes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using EvoSC.Modules.Official.MatchManagerModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using GbxRemoteNet.XmlRpc.Types;
using Moq;
using Newtonsoft.Json.Linq;

namespace MatchManagerModule.Tests.Services;

public class MatchControlServiceTests
{
    private (
        IMatchControlService MatchControlService,
        (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote) Server,
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

        mock.Server.Remote.Verify(m =>
            m.TriggerModeScriptEventArrayAsync("Trackmania.SetTeamPoints", expectedTeam, "123", "0", "0"));
    }

    [Theory]
    [InlineData(PlayerTeam.Team1, "0")]
    [InlineData(PlayerTeam.Team2, "1")]
    public async Task SetTeamMapPoints_Triggers_Correct_ModeScript_Callback(PlayerTeam team, string expectedTeam)
    {
        var mock = NewMatchControlServiceMock();


        await mock.MatchControlService.SetTeamMapPointsAsync(team, 123);

        mock.Server.Remote.Verify(m =>
            m.TriggerModeScriptEventArrayAsync("Trackmania.SetTeamPoints", expectedTeam, "0", "123", "0"));
    }

    [Theory]
    [InlineData(PlayerTeam.Team1, "0")]
    [InlineData(PlayerTeam.Team2, "1")]
    public async Task SetTeamMatchPoints_Triggers_Correct_ModeScript_Callback(PlayerTeam team, string expectedTeam)
    {
        var mock = NewMatchControlServiceMock();

        await mock.MatchControlService.SetTeamMatchPointsAsync(team, 123);

        mock.Server.Remote.Verify(m =>
            m.TriggerModeScriptEventArrayAsync("Trackmania.SetTeamPoints", expectedTeam, "0", "0", "123"));
    }

    [Theory]
    [InlineData(PlayerTeam.Team1, "0", 7, 3, 4)]
    [InlineData(PlayerTeam.Team2, "1", 7, 3, 4)]
    public async Task UpdateTeamScore_Triggers_Correct_ModeScript_Callback(PlayerTeam team, string expectedTeam,
        int roundPoints, int mapPoints, int matchPoints)
    {
        var mock = NewMatchControlServiceMock();

        await mock.MatchControlService.UpdateTeamScoreAsync(team, roundPoints, mapPoints, matchPoints);

        mock.Server.Remote.Verify(m =>
            m.TriggerModeScriptEventArrayAsync("Trackmania.SetTeamPoints", expectedTeam, roundPoints.ToString(),
                mapPoints.ToString(), matchPoints.ToString()));
    }

    [Theory]
    [InlineData(PlayerTeam.Team1, 12, 23, 34)]
    [InlineData(PlayerTeam.Team2, 56, 67, 78)]
    public async Task Gets_Current_Team_Scores(PlayerTeam team, int expectedRoundPoints, int expectedMapPoints,
        int expectedMatchPoints)
    {
        var mock = NewMatchControlServiceMock();

        mock.Server.Remote.Setup(remote => remote.GetModeScriptResponseAsync("Trackmania.GetScores"))
            .ReturnsAsync((
                new JObject
                {
                    ["teams"] = new JArray(
                        new JObject { ["id"] = 0, ["roundPoints"] = 12, ["mapPoints"] = 23, ["matchPoints"] = 34 },
                        new JObject { ["id"] = 1, ["roundPoints"] = 56, ["mapPoints"] = 67, ["matchPoints"] = 78 }
                    ),
                }, new List<XmlRpcBaseType>().ToArray()));

        var currentTeamScore = await mock.MatchControlService.GetTeamScoreAsync(team);

        Assert.Equal(expectedRoundPoints, currentTeamScore.RoundPoints);
        Assert.Equal(expectedMapPoints, currentTeamScore.MapPoints);
        Assert.Equal(expectedMatchPoints, currentTeamScore.MatchPoints);
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
