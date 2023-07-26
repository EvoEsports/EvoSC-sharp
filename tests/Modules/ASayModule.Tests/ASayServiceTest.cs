using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util.Auditing;
using EvoSC.Common.Models.Players;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ASayModule.Interfaces;
using EvoSC.Modules.Official.ASayModule.Services;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EvoSC.Modules.Official.ASayModule.Tests;

public class ASayServiceTest
{
    private const string PlayerNickName = "Evo.Chris92";
    private const string PlayerAccountId = "17868d60-b494-4b88-81df-f4ddfdae1cf1";
    private const string PlayerLogin = "F4aNYLSUS4iB3_Td_a4c8Q";
    
    private ILogger<ASayService> _logger = new NullLogger<ASayService>();
    private Mock<IOnlinePlayer> _actor = new();
    private readonly Mock<IManialinkManager> _manialinkManager = new();
    private ControllerContextMock<IEventControllerContext> _eventContext;
    private ControllerContextMock<ICommandInteractionContext> _commandContext;

    public ASayServiceTest()
    {
        _eventContext = Mocking.NewControllerContextMock<IEventControllerContext>();
        _commandContext = Mocking.NewCommandInteractionContextMock(_actor.Object);

        _actor.Setup(m => m.NickName).Returns(PlayerNickName);
        _actor.Setup(m => m.AccountId).Returns(PlayerAccountId);
    }

    private (
        IASayService ASayService,
        Mock<IContextService> ContextService,
        Locale Locale,
        Mock<IPlayerManagerService> PlayerManager,
        Mock<ILogger<ASayService>> Logger,
        (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote) Server,
        Mock<IOnlinePlayer> Player,
        Mock<IOnlinePlayer> Actor,
        Mock<IAuditEventBuilder> Audit
        )
        NewASayServiceMock()
    {
        var logger = new Mock<ILogger<ASayService>>(); 
        var contextService = Mocking.NewContextServiceMock(_commandContext.Context.Object, null);
        var locale = Mocking.NewLocaleMock(contextService.Object);
        var playerManager = new Mock<IPlayerManagerService>();

        var server = Mocking.NewServerClientMock();

        var aSayService = new ASayService(_logger, _manialinkManager.Object, contextService.Object, locale);

        var player = new Mock<IOnlinePlayer>();
        player.Setup(m => m.AccountId).Returns(PlayerAccountId);
        player.Setup(m => m.NickName).Returns(PlayerNickName);

        return (
            ASayService: aSayService,
            ContextService: contextService,
            Locale: locale,
            PlayerManager: playerManager,
            Logger: logger,
            Server: server,
            Player: player,
            Actor: _actor,
            Audit: _commandContext.AuditEventBuilder
        );
    }

    [Fact]
    private async void Should_On_Disable()
    {
        var mock = NewASayServiceMock();
        
        await mock.ASayService.OnDisableAsync();
        mock.Audit.Verify(m=>m.Success(), Times.Once);
        _manialinkManager.Verify(manager => manager.HideManialinkAsync("ASayModule.Announcement"));
    }
    
    [Fact]
    private async void Should_Show_Announcement_Message()
    {
        var mock = NewASayServiceMock();
        
        var text = "example message";
        await mock.ASayService.ShowAnnouncementAsync(text);
        mock.Audit.Verify(m=>m.Success(), Times.Once());
        _manialinkManager.Verify(manager => manager.SendPersistentManialinkAsync("ASayModule.Announcement", It.Is<object>(o => text.Equals(o.GetType().GetProperty("text")!.GetValue(o)))));
    }

    [Fact]
    private async void Should_Hide_Announcement_Message()
    {
        var mock = NewASayServiceMock();
        await mock.ASayService.HideAnnouncementAsync();
        mock.Audit.Verify(m => m.Success(), Times.Once());
        _manialinkManager.Verify(manager => manager.HideManialinkAsync("ASayModule.Announcement"));
    }
}
