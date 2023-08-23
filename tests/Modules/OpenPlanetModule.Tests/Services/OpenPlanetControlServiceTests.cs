using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util.Auditing;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Config;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Services;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests.Services;

public class OpenPlanetControlServiceTests
{
    private const string PlayerAccountId = "a467a996-eba5-44bf-9e2b-8543b50f39ae";
    private const string PlayerLogin = "pGepluulRL-eK4VDtQ85rg";

    private (
        IOpenPlanetControlService Service,
        Mock<IOnlinePlayer> Player,
        Mock<ILogger<OpenPlanetControlService>> Logger,
        Mock<IPermissionManager> Permissions,
        Mock<IOpenPlanetControlSettings> Settings,
        Mock<IManialinkManager> Manialinks,
        (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote) Server,
        Mock<IOpenPlanetScheduler> Scheduler,
        ControllerContextMock<IManialinkInteractionContext> Context
        ) NewServiceMock()
    {
        var player = new Mock<IOnlinePlayer>();
        var logger = new Mock<ILogger<OpenPlanetControlService>>();
        var permissions = new Mock<IPermissionManager>();
        var settings = new Mock<IOpenPlanetControlSettings>();
        var manialinks = new Mock<IManialinkManager>();
        var server = Mocking.NewServerClientMock();
        var scheduler = new Mock<IOpenPlanetScheduler>();

        var actionContext = new Mock<IManialinkActionContext>();
        var context =
            Mocking.NewManialinkInteractionContextMock(player.Object, actionContext.Object, manialinks.Object);
        var contextService = Mocking.NewContextServiceMock(context.Context.Object, player.Object);
        var locale = Mocking.NewLocaleMock(contextService.Object);

        var controlService = new OpenPlanetControlService(logger.Object, permissions.Object, settings.Object,
            manialinks.Object, server.Client.Object, scheduler.Object, locale);

        player.Setup(m => m.AccountId).Returns(PlayerAccountId);
        
        return (
            controlService,
            player,
            logger,
            permissions,
            settings,
            manialinks,
            server,
            scheduler,
            context
        );
    }

    [Theory]
    [InlineData(true, new[] {0, 0, 1}, OpenPlanetSignatureMode.Regular, OpenPlanetSignatureMode.Official,
        new[] {0, 0, 1}, true, true, false)]
    
    [InlineData(true, new[] {0, 0, 1}, OpenPlanetSignatureMode.Regular, OpenPlanetSignatureMode.Regular,
        new[] {0, 0, 1}, true, false, true)]
    
    [InlineData(true, new[] {0, 0, 1}, OpenPlanetSignatureMode.Regular, OpenPlanetSignatureMode.Regular,
        new[] {0, 0, 2}, true, true, false)]
    
    [InlineData(true, new[] {0, 0, 1}, OpenPlanetSignatureMode.Regular, OpenPlanetSignatureMode.Regular,
        new[] {0, 0, 1}, false, true, false)]
    
    [InlineData(false, new[] {0, 0, 2}, OpenPlanetSignatureMode.Regular, OpenPlanetSignatureMode.Official,
        new[] {0, 0, 1}, false, false, true)]
    public async Task Player_Is_Jailed(bool isOpenPlanet, int[] version, OpenPlanetSignatureMode signature,
        OpenPlanetSignatureMode expectedSignature, int[] expectedVersion, bool allowOpenplanet, bool isJailed,
        bool alreadyScheduled)
    {
        var mock = NewServiceMock();
        var opInfo = new Mock<IOpenPlanetInfo>();

        opInfo.Setup(m => m.IsOpenPlanet).Returns(isOpenPlanet);
        opInfo.Setup(m => m.Version).Returns(new Version(version[0], version[1], version[2]));
        opInfo.Setup(m => m.SignatureMode).Returns(signature);

        mock.Settings.Setup(m => m.AllowedSignatureModes).Returns(expectedSignature);
        mock.Settings.Setup(m => m.MinimumRequiredVersion)
            .Returns(new Version(expectedVersion[0], expectedVersion[1], expectedVersion[2]));
        mock.Settings.Setup(m => m.AllowOpenplanet).Returns(allowOpenplanet);

        mock.Scheduler.Setup(m => m.PlayerIsScheduledForKick(It.IsAny<IOnlinePlayer>())).Returns(alreadyScheduled);

        await mock.Service.VerifySignatureModeAsync(mock.Player.Object, opInfo.Object);

        switch (isJailed)
        {
            case true when !alreadyScheduled:
                mock.Server.Remote.Verify(m => m.ForceSpectatorAsync(PlayerLogin, 1), Times.Once);
                break;
            case false when alreadyScheduled:
                mock.Server.Remote.Verify(m => m.ForceSpectatorAsync(PlayerLogin, 0), Times.Once);
                break;
            default:
                mock.Server.Remote.Verify(m => m.ForceSpectatorAsync(PlayerLogin, It.IsAny<int>()), Times.Never);
                break;
        }
    }

    [Fact]
    public async Task Player_Is_Not_Jailed_If_Already_Jailed()
    {
        var mock = NewServiceMock();
        var opInfo = new Mock<IOpenPlanetInfo>();

        opInfo.Setup(m => m.IsOpenPlanet).Returns(true);
        mock.Settings.Setup(m => m.AllowOpenplanet).Returns(false);

        mock.Scheduler.Setup(m => m.PlayerIsScheduledForKick(It.IsAny<IOnlinePlayer>())).Returns(true);

        await mock.Service.VerifySignatureModeAsync(mock.Player.Object, opInfo.Object);
        
        mock.Server.Remote.Verify(m => m.ForceSpectatorAsync(PlayerLogin, It.IsAny<int>()), Times.Never);
    }
    
    [Fact]
    public async Task Player_Is_Not_Released_If_Already_Released()
    {
        var mock = NewServiceMock();
        var opInfo = new Mock<IOpenPlanetInfo>();

        opInfo.Setup(m => m.IsOpenPlanet).Returns(false);
        mock.Settings.Setup(m => m.AllowOpenplanet).Returns(false);

        mock.Scheduler.Setup(m => m.PlayerIsScheduledForKick(It.IsAny<IOnlinePlayer>())).Returns(false);

        await mock.Service.VerifySignatureModeAsync(mock.Player.Object, opInfo.Object);
        
        mock.Server.Remote.Verify(m => m.ForceSpectatorAsync(PlayerLogin, It.IsAny<int>()), Times.Never);
    }
}
