using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Config;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Models;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using GbxRemoteNet.Structs;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Tests.Services;

public class SpectatorTargetInfoServiceTests
{
    private readonly Mock<IManialinkManager> _manialinkManager = new();
    private readonly Mock<ISpectatorTargetInfoSettings> _settings = new();
    private readonly Mock<IPlayerManagerService> _playerManager = new();

    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote)
        _server = Mocking.NewServerClientMock();

    private ISpectatorTargetInfoService ServiceMock()
    {
        return new SpectatorTargetInfoService(
            _manialinkManager.Object,
            _server.Client.Object,
            _playerManager.Object,
            _settings.Object
        );
    }

    [Fact]
    public async Task Adds_Clears_Checkpoint_Data_And_Sorts_Times()
    {
        var spectatorTargetService = ServiceMock();
        _playerManager.Setup(s => s.GetOnlinePlayerAsync(It.IsAny<string>()))
            .ReturnsAsync(new OnlinePlayer { State = PlayerState.Spectating, });

        await spectatorTargetService.AddCheckpointAsync("*fakeplayer1*", 2, 1400);
        await spectatorTargetService.AddCheckpointAsync("*fakeplayer2*", 2, 1200);
        await spectatorTargetService.AddCheckpointAsync("*fakeplayer3*", 2, 1000);
        await spectatorTargetService.AddCheckpointAsync("*fakeplayer4*", 1, 1000);

        var cpTimes = await spectatorTargetService.GetCheckpointTimesAsync();
        Assert.Equal(2, cpTimes.Count);
        Assert.Single(cpTimes[1]);
        Assert.Equal(1000, cpTimes[1][0].time);
        Assert.Equal(3, cpTimes[2].Count);
        Assert.Equal(1000, cpTimes[2][0].time);
        Assert.Equal(1200, cpTimes[2][1].time);
        Assert.Equal(1400, cpTimes[2][2].time);

        await spectatorTargetService.ClearCheckpointsAsync();
        Assert.Empty(await spectatorTargetService.GetCheckpointTimesAsync());
    }

    [Fact]
    public async Task Gets_Login_By_Dedicated_Player_Id()
    {
        var spectatorTargetService = ServiceMock();
        _server.Remote.Setup(s => s.GetPlayerListAsync())
            .ReturnsAsync([
                new TmPlayerInfo { PlayerId = 22, Login = "UnitTest" }
            ]);

        var login = await spectatorTargetService.GetLoginOfDedicatedPlayerAsync(22);
        Assert.Equal("UnitTest", login);
    }

    [Fact]
    public async Task Updates_Spectator_Target_With_Dedicated_Player_Id()
    {
        var spectatorTargetService = ServiceMock();
        _server.Remote.Setup(s => s.GetPlayerListAsync())
            .ReturnsAsync([
                new TmPlayerInfo { PlayerId = 22, Login = "UnitTest" }
            ]);

        await spectatorTargetService.UpdateSpectatorTargetAsync("player1", 22);
        var spectatorOfPlayer = spectatorTargetService.GetLoginsSpectatingTarget("UnitTest").ToList();

        Assert.Single(spectatorOfPlayer);
        Assert.Contains("player1", spectatorOfPlayer);
    }

    [Fact]
    public async Task Removes_Spectator_If_Target_Login_Is_Null()
    {
        var spectatorTargetService = ServiceMock();

        await spectatorTargetService.UpdateSpectatorTargetAsync("player1", "UnitTest");
        await spectatorTargetService.UpdateSpectatorTargetAsync("player1", 1111);
        var spectatorOfPlayer = spectatorTargetService.GetLoginsSpectatingTarget("UnitTest").ToList();

        Assert.Empty(spectatorOfPlayer);
        Assert.DoesNotContain("player1", spectatorOfPlayer);
    }

    [Fact]
    public async Task Gets_Logins_Spectating_The_Given_Target()
    {
        var spectatorTargetService = ServiceMock();

        await spectatorTargetService.UpdateSpectatorTargetAsync("player1", "player10");
        await spectatorTargetService.UpdateSpectatorTargetAsync("player2", "player10");
        await spectatorTargetService.UpdateSpectatorTargetAsync("player3", "player20");

        var spectatorOfPlayer = spectatorTargetService.GetLoginsSpectatingTarget("player10").ToList();

        Assert.Equal(2, spectatorOfPlayer.Count);
        Assert.Contains("player1", spectatorOfPlayer);
        Assert.Contains("player2", spectatorOfPlayer);
        Assert.DoesNotContain("player3", spectatorOfPlayer);
    }

    [Theory]
    [InlineData(10_000, 1)]
    [InlineData(2_550_000, 255)]
    [InlineData(1_230_000, 123)]
    public Task Parses_Target_Player_Id_From_Spectator_Status(int spectatorStatus, int expectedPlayerId)
    {
        var spectatorTargetService = ServiceMock();
        var parsedStatus = spectatorTargetService.ParseSpectatorStatus(spectatorStatus);

        Assert.Equal(expectedPlayerId, parsedStatus.TargetPlayerId);

        return Task.CompletedTask;
    }

    [Fact]
    public Task Gets_Rank_From_Sorted_Checkpoints_List()
    {
        var spectatorTargetService = ServiceMock();
        var checkpointsList = new List<CheckpointData>
        {
            new(
                new OnlinePlayer
                {
                    AccountId = PlayerUtils.ConvertLoginToAccountId("*fakeplayer1*"), State = PlayerState.Spectating
                }, 1_000
            ),
            new(
                new OnlinePlayer
                {
                    AccountId = PlayerUtils.ConvertLoginToAccountId("*fakeplayer2*"), State = PlayerState.Spectating
                }, 1_000
            ),
            new(
                new OnlinePlayer
                {
                    AccountId = PlayerUtils.ConvertLoginToAccountId("*fakeplayer4*"), State = PlayerState.Spectating
                }, 1_001
            ),
            new(
                new OnlinePlayer
                {
                    AccountId = PlayerUtils.ConvertLoginToAccountId("*fakeplayer3*"), State = PlayerState.Spectating
                }, 1_111
            ),
        };

        Assert.Equal(1, spectatorTargetService.GetRankFromCheckpointList(checkpointsList, "*fakeplayer1*"));
        Assert.Equal(2, spectatorTargetService.GetRankFromCheckpointList(checkpointsList, "*fakeplayer2*"));
        Assert.Equal(3, spectatorTargetService.GetRankFromCheckpointList(checkpointsList, "*fakeplayer4*"));
        Assert.Equal(4, spectatorTargetService.GetRankFromCheckpointList(checkpointsList, "*fakeplayer3*"));

        return Task.CompletedTask;
    }

    [Theory]
    [InlineData(900, 1_000, 100)]
    [InlineData(100, 999, 899)]
    [InlineData(400, 200, -200)]
    public Task Calculates_Time_Difference(int leadingTime, int trailingTime, int expectedTime)
    {
        var spectatorTargetService = ServiceMock();

        Assert.Equal(expectedTime, spectatorTargetService.GetTimeDifference(leadingTime, trailingTime));

        return Task.CompletedTask;
    }
}
