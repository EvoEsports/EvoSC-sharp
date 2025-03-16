using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Players;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;
using EvoSC.Modules.Official.ScoreboardModule.Services;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.ScoreboardModule.Tests.Services;

public class ScoreboardNicknameServiceTests
{
    private readonly Mock<IManialinkManager> _manialinkManager = new();
    private readonly Mock<IPlayerManagerService> _playerManagerService = new();

    private IScoreboardNicknamesService NicknamesServiceMock()
    {
        return new ScoreboardNicknamesService(
            _playerManagerService.Object,
            _manialinkManager.Object
        );
    }

    [Fact]
    public async Task Initializes_With_Custom_Names_Only()
    {
        var nicknameService = NicknamesServiceMock();
        var playerLogin1 = "*fakeplayer1*";
        var playerLogin2 = "*fakeplayer2*";

        _playerManagerService.Setup(pms => pms.GetOnlinePlayersAsync())
            .ReturnsAsync(new List<IOnlinePlayer>
            {
                new OnlinePlayer //should not be added
                {
                    State = PlayerState.Playing,
                    AccountId = playerLogin1,
                    UbisoftName = "FakePlayer1",
                    NickName = "FakePlayer1"
                },
                new OnlinePlayer //should be added
                {
                    State = PlayerState.Playing,
                    AccountId = playerLogin2,
                    UbisoftName = "FakePlayer2",
                    NickName = "Custom Name"
                }
            });

        await nicknameService.InitializeNicknamesAsync();

        var nicknames = await nicknameService.GetNicknamesAsync();

        Assert.Single(nicknames);
        Assert.Equal("Custom Name", nicknames[playerLogin2]);
    }

    [Fact]
    public async Task Adds_Nicknames_By_Login()
    {
        var nicknameService = NicknamesServiceMock();
        var playerLogin = "*fakeplayer1*";

        _playerManagerService.Setup(pms => pms.GetOnlinePlayerAsync(playerLogin))
            .ReturnsAsync(new OnlinePlayer
            {
                State = PlayerState.Playing,
                AccountId = playerLogin,
                UbisoftName = "FakePlayer1",
                NickName = "Fake Player #1"
            });

        await nicknameService.FetchAndAddNicknameByLoginAsync(playerLogin);

        var nicknames = await nicknameService.GetNicknamesAsync();

        Assert.Single(nicknames);
        Assert.Equal("Fake Player #1", nicknames[playerLogin]);
    }

    [Fact]
    public async Task Does_Not_Add_Nicknames_That_Are_Ubinames()
    {
        var nicknameService = NicknamesServiceMock();
        var playerLogin = "*fakeplayer1*";

        _playerManagerService.Setup(pms => pms.GetOnlinePlayerAsync(playerLogin))
            .ReturnsAsync(new OnlinePlayer
            {
                State = PlayerState.Playing,
                AccountId = playerLogin,
                UbisoftName = "FakePlayer1",
                NickName = "FakePlayer1"
            });

        await nicknameService.FetchAndAddNicknameByLoginAsync(playerLogin);

        var nicknames = await nicknameService.GetNicknamesAsync();

        Assert.Empty(nicknames);
    }

    [Fact]
    public async Task Sends_Nicknames_Manialink()
    {
        var nicknameService = NicknamesServiceMock();

        await nicknameService.SendNicknamesManialinkAsync();

        _manialinkManager.Verify(mlm => mlm.SendPersistentManialinkAsync("ScoreboardModule.PlayerNicknames",
            It.IsAny<object>()));
    }
}
