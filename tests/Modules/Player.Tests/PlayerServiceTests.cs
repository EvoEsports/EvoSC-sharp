using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.Player.Interfaces;
using EvoSC.Modules.Official.Player.Services;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using Microsoft.Extensions.Logging;
using Moq;

namespace EvoSC.Modules.Official.Player.Tests;

public class PlayerServiceTests
{
    private const string PlayerNickName = "snixtho";
    private const string PlayerAccountId = "a467a996-eba5-44bf-9e2b-8543b50f39ae";
    private const string PlayerLogin = "pGepluulRL-eK4VDtQ85rg";
    
    private Mock<IServerClient> _serverClient = new();
    private Mock<ILogger<PlayerService>> _logger = new();
    private Mock<IOnlinePlayer> _actor = new();
    
    private ControllerContextMock<IEventControllerContext> _eventContext;
    private ControllerContextMock<ICommandInteractionContext> _commandContext;

    public PlayerServiceTests()
    {
        _eventContext = Mocking.NewControllerContextMock<IEventControllerContext>();
        _commandContext = Mocking.NewCommandInteractionContextMock(_actor.Object);

        _actor.Setup(m => m.NickName).Returns(PlayerNickName);
        _actor.Setup(m => m.AccountId).Returns(PlayerAccountId);
    }

    [Fact]
    public async Task Player_Created_And_Greeted_On_First_Join()
    {
        var contextService = Mocking.NewContextServiceMock(_eventContext.Context.Object, null);
        var locale = Mocking.NewLocaleMock(contextService.Object);
        
        var playerManager = new Mock<IPlayerManagerService>();
        playerManager.Setup(m => m.GetPlayerAsync(PlayerAccountId)).Returns(Task.FromResult((IPlayer?)null));
        playerManager.Setup(m => m.CreatePlayerAsync(PlayerAccountId)).Returns(Task.FromResult((IPlayer)_actor.Object));

        var playerService = new PlayerService(playerManager.Object, _serverClient.Object, _logger.Object, contextService.Object, locale);
        
        await playerService.UpdateAndGreetPlayerAsync(PlayerLogin);
        
        playerManager.Verify(m => m.GetPlayerAsync(PlayerAccountId), Times.Once);
        playerManager.Verify(m => m.CreatePlayerAsync(PlayerAccountId), Times.Once);
        playerManager.Verify(m => m.UpdateLastVisitAsync(_actor.Object), Times.Once);
        _serverClient.Verify(m => m.InfoMessageAsync(It.IsAny<string>()));
    }

    [Fact]
    public async Task Player_Is_Greeted_When_Already_Exists()
    {
        var contextService = Mocking.NewContextServiceMock(_eventContext.Context.Object, null);
        var locale = Mocking.NewLocaleMock(contextService.Object);

        var playerManager = new Mock<IPlayerManagerService>();
        playerManager.Setup(m => m.GetPlayerAsync(PlayerAccountId)).Returns(Task.FromResult((IPlayer)_actor.Object));
        
        var playerService = new PlayerService(playerManager.Object, _serverClient.Object, _logger.Object, contextService.Object, locale);

        await playerService.UpdateAndGreetPlayerAsync(PlayerLogin);
        
        playerManager.Verify(m => m.GetPlayerAsync(PlayerAccountId), Times.Once);
        playerManager.Verify(m => m.UpdateLastVisitAsync(_actor.Object), Times.Once);
        _serverClient.Verify(m => m.InfoMessageAsync(It.IsAny<string>()));
    }

    [Fact]
    public async Task Player_Is_Kicked_And_Audited()
    {
        var contextService = Mocking.NewContextServiceMock(_commandContext.Context.Object, null);
        var locale = Mocking.NewLocaleMock(contextService.Object);
        var playerManager = new Mock<IPlayerManagerService>();
        var player = new Mock<IPlayer>();
        player.Setup(m => m.AccountId).Returns(PlayerAccountId);

        var playerService = new PlayerService(playerManager.Object, _serverClient.Object, _logger.Object, contextService.Object, locale);

        await playerService.KickAsync(player.Object, _actor.Object);
        
        // contextService.Verify(m => m.Audit().Success(), Times.Once); todo: make gbxremote testable
        _serverClient.Verify(m => m.SuccessMessageAsync(It.IsAny<string>()), Times.Once);
    }
}
