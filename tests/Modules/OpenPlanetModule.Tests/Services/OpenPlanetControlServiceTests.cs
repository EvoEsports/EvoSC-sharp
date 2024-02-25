using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
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
using NSubstitute;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests.Services;

public class OpenPlanetControlServiceTests
{
    private const string PlayerAccountId = "a467a996-eba5-44bf-9e2b-8543b50f39ae";
    private const string PlayerLogin = "pGepluulRL-eK4VDtQ85rg";

    private (
        IOpenPlanetControlService Service,
        IOnlinePlayer Player,
        ILogger<OpenPlanetControlService> Logger,
        IPermissionManager Permissions,
        IOpenPlanetControlSettings Settings,
        IManialinkManager Manialinks,
        (IServerClient Client, IGbxRemoteClient Remote) Server,
        IOpenPlanetScheduler Scheduler,
        ControllerContextMock<IManialinkInteractionContext> Context
        ) NewServiceMock()
    {
        var player = Substitute.For<IOnlinePlayer>();
        var logger = Substitute.For<ILogger<OpenPlanetControlService>>();
        var permissions = Substitute.For<IPermissionManager>();
        var settings = Substitute.For<IOpenPlanetControlSettings>();
        var manialinks = Substitute.For<IManialinkManager>();
        var server = Mocking.NewServerClientMock();
        var scheduler = Substitute.For<IOpenPlanetScheduler>();

        var actionContext = Substitute.For<IManialinkActionContext>();
        var context =
            Mocking.NewManialinkInteractionContextMock(player, actionContext, manialinks);
        var contextService = Mocking.NewContextServiceMock(context.Context, player);
        var locale = Mocking.NewLocaleMock(contextService);

        var controlService = new OpenPlanetControlService(logger, permissions, settings,
            manialinks, server.Client, scheduler, locale);

        player.AccountId.Returns(PlayerAccountId);
        
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
        var opInfo = Substitute.For<IOpenPlanetInfo>();

        opInfo.IsOpenPlanet.Returns(isOpenPlanet);
        opInfo.Version.Returns(new Version(version[0], version[1], version[2]));
        opInfo.SignatureMode.Returns(signature);

        mock.Settings.AllowedSignatureModes.Returns(expectedSignature);
        mock.Settings.MinimumRequiredVersion
            .Returns(new Version(expectedVersion[0], expectedVersion[1], expectedVersion[2]));
        mock.Settings.AllowOpenplanet.Returns(allowOpenplanet);

        mock.Scheduler.PlayerIsScheduledForKick(Arg.Any<IOnlinePlayer>()).Returns(alreadyScheduled);

        await mock.Service.VerifySignatureModeAsync(mock.Player, opInfo);

        switch (isJailed)
        {
            case true when !alreadyScheduled:
                await mock.Server.Remote.Received(1).ForceSpectatorAsync(PlayerLogin, 1);
                break;
            case false when alreadyScheduled:
                await mock.Server.Remote.Received(1).ForceSpectatorAsync(PlayerLogin, 0);
                break;
            default:
                await mock.Server.Remote.DidNotReceive().ForceSpectatorAsync(PlayerLogin, Arg.Any<int>());
                break;
        }
    }

    [Fact]
    public async Task Player_Is_Not_Jailed_If_Already_Jailed()
    {
        var mock = NewServiceMock();
        var opInfo = Substitute.For<IOpenPlanetInfo>();

        opInfo.IsOpenPlanet.Returns(true);
        mock.Settings.AllowOpenplanet.Returns(false);

        mock.Scheduler.PlayerIsScheduledForKick(Arg.Any<IOnlinePlayer>()).Returns(true);

        await mock.Service.VerifySignatureModeAsync(mock.Player, opInfo);
        
        await mock.Server.Remote.DidNotReceive().ForceSpectatorAsync(PlayerLogin, Arg.Any<int>());
    }
    
    [Fact]
    public async Task Player_Is_Not_Released_If_Already_Released()
    {
        var mock = NewServiceMock();
        var opInfo = Substitute.For<IOpenPlanetInfo>();

        opInfo.IsOpenPlanet.Returns(false);
        mock.Settings.AllowOpenplanet.Returns(false);

        mock.Scheduler.PlayerIsScheduledForKick(Arg.Any<IOnlinePlayer>()).Returns(false);

        await mock.Service.VerifySignatureModeAsync(mock.Player, opInfo);
        
        await mock.Server.Remote.DidNotReceive().ForceSpectatorAsync(PlayerLogin, Arg.Any<int>());
    }
}
