using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ForceTeamModule.Interfaces;
using EvoSC.Modules.Official.ForceTeamModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using Moq;

namespace ForceTeamModule.Tests.Services;

public class ForceTeamServiceTests
{
    private (
        IForceTeamService ForceTeamService,
        Mock<IManialinkManager> ManialinkManager,
        (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote) Server,
        Locale locale
        ) NewForceTeamServiceMock()
    {
        var manialinkManager = new Mock<IManialinkManager>();
        var server = Mocking.NewServerClientMock();
        var playerManager = new Mock<IPlayerManagerService>();

        var serverClient = Mocking.NewServerClientMock();
        var player = new Mock<IOnlinePlayer>();
        var context = Mocking.NewPlayerInteractionContextMock(serverClient.Client, player.Object);
        var contextServiceMock = Mocking.NewContextServiceMock(context.Context.Object, player.Object);
        var locale = Mocking.NewLocaleMock(contextServiceMock.Object);

        var forceTeamService =
            new ForceTeamService(manialinkManager.Object, playerManager.Object, server.Client.Object, locale);

        return (
            forceTeamService,
            manialinkManager,
            server,
            locale
        );
    }

    [Fact]
    public async Task Window_Is_Shown()
    {
        var mock = NewForceTeamServiceMock();
        var player = new Mock<IPlayer>();

        await mock.ForceTeamService.ShowWindowAsync(player.Object);

        mock.ManialinkManager.Verify(m =>
            m.SendManialinkAsync(player.Object, "ForceTeamModule.ForceTeamWindow", It.IsAny<object>()));
    }

    [Fact]
    public async Task BalanceTeams_Will_Trigger_Server_Method_For_Auto_Team_Balance()
    {
        var mock = NewForceTeamServiceMock();

        await mock.ForceTeamService.BalanceTeamsAsync();
        
        mock.Server.Remote.Verify(m => m.AutoTeamBalanceAsync());
    }

    [Fact]
    public async Task Switch_Player_To_Team_2_If_In_Team_1()
    {
        var mock = NewForceTeamServiceMock();
        var player = new Mock<IOnlinePlayer>();
        player.Setup(m => m.AccountId).Returns("a467a996-eba5-44bf-9e2b-8543b50f39ae");
        player.Setup(m => m.Team).Returns(PlayerTeam.Team1);

        var newTeam = await mock.ForceTeamService.SwitchPlayerAsync(player.Object);
        
        mock.Server.Remote.Verify(m => m.ForcePlayerTeamAsync("pGepluulRL-eK4VDtQ85rg", 1));
        Assert.Equal(PlayerTeam.Team2, newTeam);
    }
    
    [Fact]
    public async Task Switch_Player_To_Team_1_If_In_Team_2()
    {
        var mock = NewForceTeamServiceMock();
        var player = new Mock<IOnlinePlayer>();
        player.Setup(m => m.AccountId).Returns("a467a996-eba5-44bf-9e2b-8543b50f39ae");
        player.Setup(m => m.Team).Returns(PlayerTeam.Team2);

        var newTeam = await mock.ForceTeamService.SwitchPlayerAsync(player.Object);
        
        mock.Server.Remote.Verify(m => m.ForcePlayerTeamAsync("pGepluulRL-eK4VDtQ85rg", 0));
        Assert.Equal(PlayerTeam.Team1, newTeam);
    }

    [Fact]
    public async Task Switching_Player_From_Unknown_Team_Does_Not_Switch()
    {
        var mock = NewForceTeamServiceMock();
        var player = new Mock<IOnlinePlayer>();
        player.Setup(m => m.AccountId).Returns("a467a996-eba5-44bf-9e2b-8543b50f39ae");
        player.Setup(m => m.Team).Returns(PlayerTeam.Unknown);

        var newTeam = await mock.ForceTeamService.SwitchPlayerAsync(player.Object);
        
        mock.Server.Remote.Verify(m => m.ForcePlayerTeamAsync("pGepluulRL-eK4VDtQ85rg", It.IsAny<int>()), Times.Never);
        Assert.Equal(PlayerTeam.Unknown, newTeam);
    }
}
