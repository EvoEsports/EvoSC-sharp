using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Config;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;
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
    public async Task Gets_Spectating_Logins_For_Given_Target()
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
}
