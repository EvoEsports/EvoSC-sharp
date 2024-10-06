using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Enums;
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

    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote, Mock<IChatService> Chat) _server =
        Mocking.NewServerClientMock();

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
    public async Task Adds_And_Clears_Checkpoint_Data()
    {
        var spectatorTargetService = ServiceMock();
        _playerManager.Setup(s => s.GetOnlinePlayerAsync(It.IsAny<string>()))
            .ReturnsAsync(new OnlinePlayer { State = PlayerState.Spectating, });

        await spectatorTargetService.AddCheckpointAsync("*fakeplayer4*", 1, 1000);
        await spectatorTargetService.AddCheckpointAsync("*fakeplayer3*", 2, 1000);
        await spectatorTargetService.AddCheckpointAsync("*fakeplayer2*", 2, 1200);
        await spectatorTargetService.AddCheckpointAsync("*fakeplayer1*", 2, 1400);

        var cpTimes = spectatorTargetService.GetCheckpointTimes();
        Assert.Equal(2, cpTimes.Count);
        Assert.Single(cpTimes[1]);
        Assert.Equal(1000, cpTimes[1][0].time);
        Assert.Equal(3, cpTimes[2].Count);
        Assert.Equal(1000, cpTimes[2][0].time);
        Assert.Equal(1200, cpTimes[2][1].time);
        Assert.Equal(1400, cpTimes[2][2].time);

        await spectatorTargetService.ClearCheckpointsAsync();
        var checkpointTime = spectatorTargetService.GetCheckpointTimes();

        Assert.Empty(checkpointTime);
    }

    [Fact]
    public async Task Gets_Online_Player_By_Login()
    {
        var login = "*fakeplayer1*";
        var spectatorTargetService = ServiceMock();
        var expectedPlayer = new OnlinePlayer { State = PlayerState.Playing, AccountId = login };

        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync(login))
            .ReturnsAsync(expectedPlayer);

        var player = await spectatorTargetService.GetOnlinePlayerByLoginAsync(login);

        Assert.Equal(login, player.GetLogin());
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

        var fakePlayer = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer_unittest*" };
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

        var fakePlayer = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer_UnitTest*" };

        var spectatorOfPlayer =
            spectatorTargetService.GetLoginsOfPlayersSpectatingTarget(fakePlayer).ToList();

        Assert.Empty(spectatorOfPlayer);
        Assert.DoesNotContain("*fakeplayer1*", spectatorOfPlayer);
    }

    [Fact]
    public async Task Removes_Spectator_From_Spectator_Target_Repository()
    {
        var spectatorTargetService = ServiceMock();
        var spectatorLogin = "*fakeplayer_spec*";

        await spectatorTargetService.AddCheckpointAsync("*fakeplayer1*", 1, 1);
        await spectatorTargetService.SetSpectatorTargetAsync(spectatorLogin, "*fakeplayer1*");
        await spectatorTargetService.RemovePlayerAsync(spectatorLogin);

        var spectatorInRepo = spectatorTargetService
            .GetSpectatorTargets()
            .ContainsKey(spectatorLogin);

        Assert.False(spectatorInRepo);
    }

    [Fact]
    public async Task Removes_Driver_From_Spectator_Target_Repository()
    {
        var spectatorTargetService = ServiceMock();
        var spectatorLogin1 = "*fakeplayer99*";
        var spectatorLogin2 = "*fakeplayer98*";
        var targetLogin = "*fakeplayer2*";
        
        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync("*fakeplayer1*"))
            .ReturnsAsync(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer1*" });
        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync("*fakeplayer2*"))
            .ReturnsAsync(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer2*" });
        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync("*fakeplayer3*"))
            .ReturnsAsync(new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer3*" });

        await spectatorTargetService.AddCheckpointAsync("*fakeplayer1*", 1, 1);
        await spectatorTargetService.AddCheckpointAsync(targetLogin, 2, 2);
        await spectatorTargetService.AddCheckpointAsync("*fakeplayer3*", 3, 3);
        await spectatorTargetService.SetSpectatorTargetAsync(spectatorLogin1, targetLogin);
        await spectatorTargetService.SetSpectatorTargetAsync(spectatorLogin2, targetLogin);

        await spectatorTargetService.RemovePlayerAsync(targetLogin);

        var targetPlayerInRepo = spectatorTargetService
            .GetSpectatorTargets()
            .Any(kv => kv.Value.GetLogin() == targetLogin);

        var spectatorsOfTargetInRepo = spectatorTargetService
            .GetSpectatorTargets()
            .Any(kv => kv.Key == spectatorLogin1 || kv.Key == spectatorLogin2);

        Assert.False(targetPlayerInRepo);
        Assert.False(spectatorsOfTargetInRepo);
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
            new CheckpointData(
                new OnlinePlayer
                {
                    AccountId = PlayerUtils.ConvertLoginToAccountId("*fakeplayer1*"), State = PlayerState.Spectating
                }, 1_000
            ),
            new CheckpointData(
                new OnlinePlayer
                {
                    AccountId = PlayerUtils.ConvertLoginToAccountId("*fakeplayer2*"), State = PlayerState.Spectating
                }, 1_000
            ),
            new CheckpointData(
                new OnlinePlayer
                {
                    AccountId = PlayerUtils.ConvertLoginToAccountId("*fakeplayer4*"), State = PlayerState.Spectating
                }, 1_001
            ),
            new CheckpointData(
                new OnlinePlayer
                {
                    AccountId = PlayerUtils.ConvertLoginToAccountId("*fakeplayer3*"), State = PlayerState.Spectating
                }, 1_111
            ),
        };

        var player1Rank = checkpointsList.GetRank("*fakeplayer1*");
        var player2Rank = checkpointsList.GetRank("*fakeplayer2*");
        var player3Rank = checkpointsList.GetRank("*fakeplayer3*");
        var player4Rank = checkpointsList.GetRank("*fakeplayer4*");

        Assert.Equal(1, player1Rank);
        Assert.Equal(2, player2Rank);
        Assert.Equal(4, player3Rank);
        Assert.Equal(3, player4Rank);

        return Task.CompletedTask;
    }

    [Theory]
    [InlineData(900, 1_000, 100)]
    [InlineData(100, 999, 899)]
    [InlineData(400, 200, 200)]
    public Task Calculates_Time_Difference(int leadingTime, int trailingTime, int expectedTime)
    {
        var spectatorTargetService = ServiceMock();
        var timeDifference = spectatorTargetService.GetTimeDifference(leadingTime, trailingTime);

        Assert.Equal(expectedTime, timeDifference);

        return Task.CompletedTask;
    }

    [Fact]
    public async Task Hides_Widget_For_Everyone()
    {
        var spectatorTargetService = ServiceMock();
        await spectatorTargetService.HideSpectatorInfoWidgetAsync();

        _manialinkManager.Verify(mm => mm.HideManialinkAsync("SpectatorTargetInfoModule.SpectatorTargetInfo"));
    }

    [Fact]
    public async Task Hides_Widget_For_The_Given_Player()
    {
        var login = "*fakeplayer1*";
        var spectatorTargetService = ServiceMock();
        await spectatorTargetService.HideSpectatorInfoWidgetAsync(login);

        _manialinkManager.Verify(mm => mm.HideManialinkAsync(login, "SpectatorTargetInfoModule.SpectatorTargetInfo"));
    }

    [Fact]
    public async Task Sends_Spec_Status_Report_Script()
    {
        var spectatorTargetService = ServiceMock();

        await spectatorTargetService.SendReportSpectatorTargetManialinkAsync();

        _manialinkManager.Verify(mm => mm.SendPersistentManialinkAsync("SpectatorTargetInfoModule.ReportSpecTarget"));
    }

    [Fact]
    public async Task Gets_The_Team_Color()
    {
        var spectatorTargetService = ServiceMock();

        _matchSettingsService.Setup(mss => mss.GetCurrentModeAsync())
            .ReturnsAsync(DefaultModeScriptName.Teams);
        _server.Remote.Setup(remote => remote.GetTeamInfoAsync(1))
            .ReturnsAsync(new TmTeamInfo { RGB = "FF0066" });
        _server.Remote.Setup(remote => remote.GetTeamInfoAsync(2))
            .ReturnsAsync(new TmTeamInfo { RGB = "111111" });

        await spectatorTargetService.DetectIsTeamsModeAsync();
        await spectatorTargetService.FetchAndCacheTeamInfoAsync();

        var team1Color = spectatorTargetService.GetTeamColor(PlayerTeam.Team1);
        var team2Color = spectatorTargetService.GetTeamColor(PlayerTeam.Team2);

        Assert.Equal("FF0066", team1Color);
        Assert.Equal("111111", team2Color);
    }

    [Fact]
    public async Task Hides_Default_Game_Mode_Ui()
    {
        await ServiceMock().HideGameModeUiAsync();

        _gameModeUiModuleService.Verify(gmums => gmums.ApplyComponentSettingsAsync(
            GameModeUiComponents.SpectatorBaseName,
            false,
            It.IsAny<double>(),
            It.IsAny<double>(),
            It.IsAny<double>()
        ));
    }

    [Fact]
    public async Task Sends_The_Widget_To_The_Given_Player_With_Arguments()
    {
        var spectatorLogin = "*fakeplayer1*";
        var targetPlayer = new OnlinePlayer
        {
            State = PlayerState.Playing, AccountId = "*fakeplayer2*", NickName = "UnitTest", Team = PlayerTeam.Team1
        };

        _matchSettingsService.Setup(mss => mss.GetCurrentModeAsync())
            .ReturnsAsync(DefaultModeScriptName.Teams);
        _server.Remote.Setup(remote => remote.GetTeamInfoAsync(1))
            .ReturnsAsync(new TmTeamInfo { RGB = "FF0066" });

        var spectatorTargetService = ServiceMock();
        await spectatorTargetService.DetectIsTeamsModeAsync();
        await spectatorTargetService.FetchAndCacheTeamInfoAsync();
        var widgetData = spectatorTargetService.GetWidgetData(targetPlayer, 2, 150);
        await spectatorTargetService.SendSpectatorInfoWidgetAsync(spectatorLogin, targetPlayer, widgetData);

        _manialinkManager.Verify(mm =>
            mm.SendManialinkAsync(spectatorLogin, "SpectatorTargetInfoModule.SpectatorTargetInfo", widgetData));
    }

    [Fact]
    public async Task Sends_The_Widget_To_The_Given_Players_With_Arguments()
    {
        var spectatorLogins = new List<string> { "*fakeplayer1*", "*fakeplayer2*", };
        var targetPlayer = new OnlinePlayer
        {
            State = PlayerState.Playing,
            AccountId = "*fakeplayer99*",
            NickName = "UnitTest",
            Team = PlayerTeam.Team1
        };

        _matchSettingsService.Setup(mss => mss.GetCurrentModeAsync())
            .ReturnsAsync(DefaultModeScriptName.Teams);
        _server.Remote.Setup(remote => remote.GetTeamInfoAsync(1))
            .ReturnsAsync(new TmTeamInfo { RGB = "FF0066" });
        _server.Remote.Setup(remote => remote.GetTeamInfoAsync(2))
            .ReturnsAsync(new TmTeamInfo { RGB = "111111" });

        var spectatorTargetService = ServiceMock();
        await spectatorTargetService.DetectIsTeamsModeAsync();
        await spectatorTargetService.FetchAndCacheTeamInfoAsync();
        await spectatorTargetService.SendSpectatorInfoWidgetAsync(spectatorLogins, targetPlayer, 2, 150);

        _manialinkManager.Verify(mm =>
            mm.SendManialinkAsync("*fakeplayer1*", "SpectatorTargetInfoModule.SpectatorTargetInfo",
                It.IsAny<object>()));
        _manialinkManager.Verify(mm =>
            mm.SendManialinkAsync("*fakeplayer2*", "SpectatorTargetInfoModule.SpectatorTargetInfo",
                It.IsAny<object>()));
    }

    [Fact]
    public async Task Sends_The_Widget_To_The_Given_Player_Without_Time_And_Checkpoint_Index()
    {
        var spectatorLogin = "*fakeplayer1*";
        var targetPlayer = new OnlinePlayer
        {
            State = PlayerState.Playing,
            AccountId = "*fakeplayer99*",
            NickName = "UnitTest",
            Team = PlayerTeam.Team1
        };
        var otherPlayer = new OnlinePlayer
        {
            State = PlayerState.Playing,
            AccountId = "*fakeplayer98*",
            NickName = "UnitTestOpponent",
            Team = PlayerTeam.Team2
        };

        _matchSettingsService.Setup(mss => mss.GetCurrentModeAsync())
            .ReturnsAsync(DefaultModeScriptName.Teams);
        _server.Remote.Setup(remote => remote.GetTeamInfoAsync(1))
            .ReturnsAsync(new TmTeamInfo { RGB = "FF0066" });
        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync(targetPlayer.AccountId))
            .ReturnsAsync(targetPlayer);
        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync(otherPlayer.AccountId))
            .ReturnsAsync(otherPlayer);

        var spectatorTargetService = ServiceMock();
        await spectatorTargetService.DetectIsTeamsModeAsync();
        await spectatorTargetService.FetchAndCacheTeamInfoAsync();

        await spectatorTargetService.AddCheckpointAsync(otherPlayer.GetLogin(), 2, 1000);
        await spectatorTargetService.AddCheckpointAsync(targetPlayer.GetLogin(), 2, 1234);
        await spectatorTargetService.SendSpectatorInfoWidgetAsync(spectatorLogin, targetPlayer);

        _manialinkManager.Verify(mm =>
                mm.SendManialinkAsync(spectatorLogin, "SpectatorTargetInfoModule.SpectatorTargetInfo",
                    It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task Resets_Widget_For_Spectators()
    {
        var spectatorLogin = "*fakeplayer1*";
        var targetPlayer = new OnlinePlayer
        {
            State = PlayerState.Playing,
            AccountId = "*fakeplayer99*",
            NickName = "UnitTest",
            Team = PlayerTeam.Team1
        };

        _matchSettingsService.Setup(mss => mss.GetCurrentModeAsync())
            .ReturnsAsync(DefaultModeScriptName.Teams);
        _server.Remote.Setup(remote => remote.GetTeamInfoAsync(1))
            .ReturnsAsync(new TmTeamInfo { RGB = "FF0066" });
        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync(targetPlayer.AccountId))
            .ReturnsAsync(targetPlayer);

        var spectatorTargetService = ServiceMock();
        await spectatorTargetService.DetectIsTeamsModeAsync();
        await spectatorTargetService.FetchAndCacheTeamInfoAsync();
        await spectatorTargetService.SetSpectatorTargetAsync(spectatorLogin, targetPlayer.GetLogin());
        await spectatorTargetService.ResetWidgetForSpectatorsAsync();

        _manialinkManager.Verify(mm => mm.SendManialinkAsync(spectatorLogin,
            "SpectatorTargetInfoModule.SpectatorTargetInfo",
            It.IsAny<object>()));
    }

    [Fact]
    public async Task Gets_Last_Checkpoint_Index_Of_Player()
    {
        var targetPlayer = new OnlinePlayer
        {
            State = PlayerState.Playing,
            AccountId = "*fakeplayer99*",
            NickName = "UnitTest",
            Team = PlayerTeam.Team1
        };

        _playerManager.Setup(pm => pm.GetOnlinePlayerAsync(targetPlayer.AccountId))
            .ReturnsAsync(targetPlayer);

        var spectatorTargetService = ServiceMock();
        await spectatorTargetService.AddCheckpointAsync(targetPlayer.GetLogin(), 1, 1000);
        await spectatorTargetService.AddCheckpointAsync(targetPlayer.GetLogin(), 2, 2000);
        await spectatorTargetService.AddCheckpointAsync(targetPlayer.GetLogin(), 3, 3000);
        var checkpointIndex = spectatorTargetService.GetLastCheckpointIndexOfPlayer(targetPlayer);

        Assert.Equal(3, checkpointIndex);
    }
}
