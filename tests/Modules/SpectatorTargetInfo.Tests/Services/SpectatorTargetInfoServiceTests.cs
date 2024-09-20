using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Config;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Models;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using GbxRemoteNet.Structs;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Tests.Services;

public class SpectatorTargetInfoServiceTests
{
    private readonly Mock<IManialinkManager> _manialinkManager = new();
    private readonly Mock<IMatchSettingsService> _matchSettingsService = new();
    private readonly Mock<ISpectatorTargetInfoSettings> _settings = new();
    private readonly Mock<IPlayerManagerService> _playerManager = new();
    private readonly Mock<IGameModeUiModuleService> _gameModeUiModuleService = new();
    private readonly Mock<ILogger<SpectatorTargetInfoService>> _logger = new();
    private readonly Mock<IThemeManager> _theme = new();

    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote)
        _server = Mocking.NewServerClientMock();

    private ISpectatorTargetInfoService ServiceMock()
    {
        return new SpectatorTargetInfoService(
            _manialinkManager.Object,
            _server.Client.Object,
            _playerManager.Object,
            _matchSettingsService.Object,
            _settings.Object,
            _theme.Object,
            _gameModeUiModuleService.Object,
            _logger.Object
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

        var cpTimes = spectatorTargetService.GetCheckpointTimes();
        Assert.Equal(2, cpTimes.Count);
        Assert.Single(cpTimes[1]);
        Assert.Equal(1000, cpTimes[1][0].time);
        Assert.Equal(3, cpTimes[2].Count);
        Assert.Equal(1000, cpTimes[2][0].time);
        Assert.Equal(1200, cpTimes[2][1].time);
        Assert.Equal(1400, cpTimes[2][2].time);

        await spectatorTargetService.ClearCheckpointsAsync();
        Assert.Empty(spectatorTargetService.GetCheckpointTimes());
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
        
        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync("*fakeplayer_unittest*"))
            .ReturnsAsync(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer_unittest*" });

        await spectatorTargetService.SetSpectatorTargetAsync("*fakeplayer1*", "*fakeplayer_unittest*");

        var fakePlayer = new OnlinePlayer
        {
            State = PlayerState.Playing,
            AccountId = "*fakeplayer_unittest*"
        };
        var spectatorOfPlayer = spectatorTargetService.GetLoginsOfPlayersSpectatingTarget(fakePlayer).ToList();

        Assert.Single(spectatorOfPlayer);
        Assert.Contains("*fakeplayer1*", spectatorOfPlayer);
    }

    [Fact]
    public async Task Removes_Spectator_If_Target_Login_Is_Null()
    {
        var spectatorTargetService = ServiceMock();
        
        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync("*fakeplayer_UnitTest*"))
            .ReturnsAsync(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer_UnitTest*" });
        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync("*fakeplayer_SomeOtherPlayer*"))
            .ReturnsAsync(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer_SomeOtherPlayer*" });

        await spectatorTargetService.SetSpectatorTargetAsync("*fakeplayer1*", "*fakeplayer_UnitTest*");
        await spectatorTargetService.SetSpectatorTargetAsync("*fakeplayer1*", "*fakeplayer_SomeOtherPlayer*");

        var fakePlayer = new OnlinePlayer
        {
            State = PlayerState.Playing,
            AccountId = "*fakeplayer_UnitTest*"
        };

        var spectatorOfPlayer =
            spectatorTargetService.GetLoginsOfPlayersSpectatingTarget(fakePlayer).ToList();

        Assert.Empty(spectatorOfPlayer);
        Assert.DoesNotContain("*fakeplayer1*", spectatorOfPlayer);
    }

    [Fact]
    public async Task Gets_Logins_Spectating_The_Given_Target()
    {
        var spectatorTargetService = ServiceMock();

        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync("*fakeplayer10*"))
            .ReturnsAsync(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer10*" });
        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync("*fakeplayer99*"))
            .ReturnsAsync(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer99*" });

        await spectatorTargetService.SetSpectatorTargetAsync("*fakeplayer1*", "*fakeplayer10*");
        await spectatorTargetService.SetSpectatorTargetAsync("*fakeplayer2*", "*fakeplayer10*");
        await spectatorTargetService.SetSpectatorTargetAsync("*fakeplayer3*", "*fakeplayer99*");

        var fakePlayer10 = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer10*" };
        var spectatorOfPlayer = spectatorTargetService.GetLoginsOfPlayersSpectatingTarget(fakePlayer10).ToList();

        Assert.Equal(2, spectatorOfPlayer.Count);
        Assert.Contains("*fakeplayer1*", spectatorOfPlayer);
        Assert.Contains("*fakeplayer2*", spectatorOfPlayer);
        Assert.DoesNotContain("*fakeplayer3*", spectatorOfPlayer);
    }

    [Fact]
    public Task Gets_Rank_From_Sorted_Checkpoints_List()
    {
        var checkpointsList = new CheckpointsGroup
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

        Assert.Equal(1, checkpointsList.GetRank("*fakeplayer1*"));
        Assert.Equal(2, checkpointsList.GetRank("*fakeplayer2*"));
        Assert.Equal(3, checkpointsList.GetRank("*fakeplayer4*"));
        Assert.Equal(4, checkpointsList.GetRank("*fakeplayer3*"));

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
