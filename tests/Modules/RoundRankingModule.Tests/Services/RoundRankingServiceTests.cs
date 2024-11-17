using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Config;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using Moq;

namespace EvoSC.Modules.Official.RoundRankingModule.Tests.Services;

public class RoundRankingServiceTests
{
    private readonly Mock<IRoundRankingSettings> _settings = new();
    private readonly Mock<IManialinkManager> _manialinkManager = new();
    private readonly Mock<IPlayerManagerService> _playerManagerService = new();
    private readonly Mock<IMatchSettingsService> _matchSettingsService = new();
    private readonly Mock<IThemeManager> _theme = new();

    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote, Mock<IChatService> Chat) _server =
        Mocking.NewServerClientMock();

    private IRoundRankingService RoundRankingServiceMock()
    {
        return new RoundRankingService(
            _settings.Object,
            _manialinkManager.Object,
            _playerManagerService.Object,
            _matchSettingsService.Object,
            _theme.Object,
            _server.Client.Object
        );
    }

    [Fact]
    public async Task Adds_Checkpoint_Data_To_Repository()
    {
        var roundRankingService = RoundRankingServiceMock();

        await roundRankingService.ConsumeCheckpointAsync("*fakeplayer1*", 1, 1000, true, false);
        await roundRankingService.ConsumeCheckpointAsync("*fakeplayer2*", 2, 1200, true, false);
        await roundRankingService.ConsumeCheckpointAsync("*fakeplayer3*", 3, 1300, true, false);

        var sortedCheckpoints = roundRankingService.GetSortedCheckpoints();
        var firstCheckpointData = sortedCheckpoints.First();

        Assert.Equal(3, sortedCheckpoints.Count);
        Assert.Equal(3, firstCheckpointData.CheckpointId);
        Assert.Equal(1300, firstCheckpointData.Time.TotalMilliseconds);

        _manialinkManager.Verify(mlm =>
            mlm.SendPersistentManialinkAsync("RoundRankingModule.RoundRanking", It.IsAny<object>()), Times.Exactly(3));
    }

    [Fact]
    public async Task Adds_Dnf_To_Repository()
    {
        var roundRankingService = RoundRankingServiceMock();
        await roundRankingService.SetIsTimeAttackModeAsync(false);

        await roundRankingService.ConsumeCheckpointAsync("*fakeplayer1*", 1, 1000, true, false);
        await roundRankingService.ConsumeDnfAsync("*fakeplayer1*");

        var sortedCheckpoints = roundRankingService.GetSortedCheckpoints();
        var firstCheckpointData = sortedCheckpoints.First();

        Assert.Equal("DNF", firstCheckpointData.FormattedTime());
    }

    [Fact]
    public async Task Removes_Entry_On_Dnf_In_TimeAttack_Mode()
    {
        var roundRankingService = RoundRankingServiceMock();
        await roundRankingService.SetIsTimeAttackModeAsync(true);

        await roundRankingService.ConsumeCheckpointAsync("*fakeplayer1*", 1, 1000, true, false);
        await roundRankingService.ConsumeCheckpointAsync("*fakeplayer2*", 1, 2000, true, false);
        await roundRankingService.ConsumeDnfAsync("*fakeplayer1*");

        var sortedCheckpoints = roundRankingService.GetSortedCheckpoints();
        var firstCheckpointData = sortedCheckpoints.First();

        Assert.Single(sortedCheckpoints);
        Assert.Equal("2.000", firstCheckpointData.FormattedTime());
    }

    [Fact]
    public async Task Removes_Player_Entry_From_Repository_In_TimeAttack_Mode()
    {
        var roundRankingService = RoundRankingServiceMock();
        await roundRankingService.SetIsTimeAttackModeAsync(true);

        await roundRankingService.ConsumeCheckpointAsync("*fakeplayer1*", 1, 1000, true, false);
        await roundRankingService.RemovePlayerCheckpointDataAsync("*fakeplayer1*");
        await roundRankingService.RemovePlayerCheckpointDataAsync("*fakeplayer2*");

        var sortedCheckpoints = roundRankingService.GetSortedCheckpoints();

        Assert.Empty(sortedCheckpoints);
    }

    [Fact]
    public async Task Ignores_Remove_Player_During_Rounds_Mode()
    {
        var roundRankingService = RoundRankingServiceMock();
        await roundRankingService.SetIsTimeAttackModeAsync(false);

        await roundRankingService.ConsumeCheckpointAsync("*fakeplayer1*", 1, 1234, true, false);
        await roundRankingService.RemovePlayerCheckpointDataAsync("*fakeplayer1*");

        var sortedCheckpoints = roundRankingService.GetSortedCheckpoints();
        var firstCheckpointData = sortedCheckpoints.First();

        Assert.Single(sortedCheckpoints);
        Assert.Equal(1234, firstCheckpointData.Time.TotalMilliseconds);
    }

    [Fact]
    public async Task Gets_Sorted_Checkpoints()
    {
        var roundRankingService = RoundRankingServiceMock();

        await roundRankingService.ConsumeDnfAsync("*fakeplayer1*");
        await roundRankingService.ConsumeCheckpointAsync("*fakeplayer2*", 2, 2345, true, false);
        await roundRankingService.ConsumeCheckpointAsync("*fakeplayer3*", 1, 1234, true, false);

        var sortedCheckpoints = roundRankingService.GetSortedCheckpoints();
        Assert.Equal(2345, sortedCheckpoints[0].Time.TotalMilliseconds);
        Assert.Equal(1234, sortedCheckpoints[1].Time.TotalMilliseconds);
        Assert.Equal("DNF", sortedCheckpoints[2].FormattedTime());
    }

    [Fact]
    public async Task Clears_Checkpoint_Data()
    {
        var roundRankingService = RoundRankingServiceMock();
        await roundRankingService.SetIsTimeAttackModeAsync(false);

        await roundRankingService.ConsumeCheckpointAsync("*fakeplayer1*", 1, 1234, true, false);
        await roundRankingService.ConsumeCheckpointAsync("*fakeplayer2*", 1, 2345, true, false);
        await roundRankingService.ClearCheckpointDataAsync();

        var sortedCheckpoints = roundRankingService.GetSortedCheckpoints();

        Assert.Empty(sortedCheckpoints);
    }

    [Fact]
    public async Task Sends_Round_Ranking_Widget()
    {
        var roundRankingService = RoundRankingServiceMock();

        _matchSettingsService.Setup(mss => mss.GetCurrentModeAsync())
            .ReturnsAsync(DefaultModeScriptName.Cup);

        await roundRankingService.DetectIsTeamsModeAsync();
        await roundRankingService.SendRoundRankingWidgetAsync();

        _manialinkManager.Verify(
            mlm => mlm.SendPersistentManialinkAsync("RoundRankingModule.RoundRanking", It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task Hides_Round_Ranking_Widget()
    {
        var roundRankingService = RoundRankingServiceMock();
        await roundRankingService.HideRoundRankingWidgetAsync();

        _manialinkManager.Verify(mlm => mlm.HideManialinkAsync("RoundRankingModule.RoundRanking"));
    }
}
