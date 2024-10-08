﻿using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util.Auditing;
using EvoSC.Modules.Official.Player.Config;
using EvoSC.Modules.Official.Player.Interfaces;
using EvoSC.Modules.Official.Player.Services;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace EvoSC.Modules.Official.Player.Tests;

public class PlayerServiceTests
{
    private const string PlayerNickName = "snixtho";
    private const string PlayerAccountId = "a467a996-eba5-44bf-9e2b-8543b50f39ae";
    private const string PlayerLogin = "pGepluulRL-eK4VDtQ85rg";
    
    private Mock<ILogger<PlayerService>> _logger = new();
    private Mock<IOnlinePlayer> _actor = new();
    private ControllerContextMock<IEventControllerContext> _eventContext;
    private ControllerContextMock<ICommandInteractionContext> _commandContext;
    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote, Mock<IChatService> Chat) _serverClient =
        Mocking.NewServerClientMock();

    public PlayerServiceTests()
    {
        _eventContext = Mocking.NewControllerContextMock<IEventControllerContext>();
        _commandContext = Mocking.NewCommandInteractionContextMock(_serverClient.Client, _actor.Object);

        _actor.Setup(m => m.NickName).Returns(PlayerNickName);
        _actor.Setup(m => m.AccountId).Returns(PlayerAccountId);
    }
    
    /// <summary>
    /// Sets up mocks for the player service.
    /// </summary>
    /// <returns></returns>
    private (
        IPlayerService PlayerService,
        Mock<IContextService> ContextService,
        Locale Locale,
        Mock<IPlayerManagerService> PlayerManager,
        Mock<ILogger<PlayerService>> Logger,
        (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote, Mock<IChatService> Chat) Server,
        Mock<IOnlinePlayer> Player,
        Mock<IOnlinePlayer> Actor,
        Mock<IAuditEventBuilder> Audit,
        Mock<IPlayerModuleSettings> PlayerModuleSettings,
        Mock<IPermissionManager> PermissionManager)
        NewPlayerServiceMock()
    {
        var contextService = Mocking.NewContextServiceMock(_commandContext.Context.Object, null);
        var locale = Mocking.NewLocaleMock(contextService.Object);
        var playerManager = new Mock<IPlayerManagerService>();
        var settings = new Mock<IPlayerModuleSettings>();
        var permissionManager = new Mock<IPermissionManager>();

        var server = Mocking.NewServerClientMock();

        var playerService = new PlayerService(playerManager.Object, server.Client.Object, _logger.Object,
            contextService.Object, locale, settings.Object, permissionManager.Object);

        var player = new Mock<IOnlinePlayer>();
        player.Setup(m => m.AccountId).Returns(PlayerAccountId);
        player.Setup(m => m.NickName).Returns(PlayerNickName);

        return (
            PlayerService: playerService,
            ContextService: contextService,
            Locale: locale,
            PlayerManager: playerManager,
            Logger: _logger,
            Server: server,
            Player: player,
            Actor: _actor,
            Audit: _commandContext.AuditEventBuilder,
            PlayerModuleSettings: settings,
            PermissionManager: permissionManager
        );
    }

    [Theory]
    [InlineData(false, 0)]
    [InlineData(true, 1)]
    public async Task SetupPlayer_Adds_To_Default_Group(bool addToGroup, int setGroupCalled)
    {
        var mock = NewPlayerServiceMock();
        
        mock.PlayerModuleSettings.Setup(m => m.DefaultGroupId).Returns(1337);
        mock.PlayerModuleSettings.Setup(m => m.AddToDefaultGroup).Returns(addToGroup);

        await mock.PlayerService.SetupPlayerAsync(mock.Player.Object);
        
        mock.PermissionManager.Verify(m => m.SetDisplayGroupAsync(mock.Player.Object, 1337), Times.Exactly(setGroupCalled));
    }

    [Fact]
    public async Task Player_Is_Greeted_When_Already_Exists()
    {
        var mock = NewPlayerServiceMock();

        mock.PlayerManager.Setup(m => m.GetPlayerAsync(PlayerAccountId))
            .ReturnsAsync(mock.Player.Object);
        
        await mock.PlayerService.GreetPlayerAsync(mock.Player.Object);
        
        mock.PlayerManager.Verify(m => m.UpdateLastVisitAsync(mock.Player.Object), Times.Once);
        mock.Server.Chat.Verify(m => m.InfoMessageAsync(It.IsAny<string>()));
    }

    [Fact]
    public async Task Player_Is_Kicked_And_Audited()
    {
        var mock = NewPlayerServiceMock();
        mock.Server.Remote.Setup(m => m.KickAsync(PlayerLogin, "")).Returns(Task.FromResult(true));

        await mock.PlayerService.KickAsync(mock.Player.Object, mock.Actor.Object);
        
        mock.Audit.Verify(m => m.Success(), Times.Once);
        mock.Server.Remote.Verify(m => m.KickAsync(PlayerLogin,""), Times.Once);
        mock.Server.Chat.Verify(m => m.SuccessMessageAsync(It.IsAny<string>(), mock.Actor.Object), Times.Once);
    }

    [Fact]
    public async Task Player_Kicking_Failed_Sends_Error_Message()
    {
        var mock = NewPlayerServiceMock();

        await mock.PlayerService.KickAsync(mock.Player.Object, mock.Actor.Object);

        mock.ContextService.Verify(m => m.Audit(), Times.Never);
        mock.Server.Chat.Verify(m => m.ErrorMessageAsync(It.IsAny<string>(), mock.Actor.Object), Times.Once);
    }

    [Fact]
    public async Task Player_Is_Muted_And_Audited()
    {
        var mock = NewPlayerServiceMock();
        mock.Server.Remote.Setup(m => m.IgnoreAsync(PlayerLogin)).Returns(Task.FromResult(true));

        await mock.PlayerService.MuteAsync(mock.Player.Object, mock.Actor.Object);
        
        mock.Audit.Verify(m => m.Success(), Times.Once);
        mock.Server.Remote.Verify(m => m.IgnoreAsync(PlayerLogin), Times.Once);
        mock.Server.Chat.Verify(m => m.WarningMessageAsync(It.IsAny<string>(), mock.Player.Object), Times.Once);
        mock.Server.Chat.Verify(m => m.SuccessMessageAsync(It.IsAny<string>(), mock.Actor.Object), Times.Once);
    }
    
    [Fact]
    public async Task Player_Is_Muting_Failed_Sends_Error_Message()
    {
        var mock = NewPlayerServiceMock();

        await mock.PlayerService.MuteAsync(mock.Player.Object, mock.Actor.Object);
        
        mock.ContextService.Verify(m => m.Audit(), Times.Never);
        mock.Server.Chat.Verify(m => m.ErrorMessageAsync(It.IsAny<string>(), mock.Actor.Object), Times.Once);
    }
    
    [Fact]
    public async Task Player_Is_UnMuted_And_Audited()
    {
        var mock = NewPlayerServiceMock();
        mock.Server.Remote.Setup(m => m.UnIgnoreAsync(PlayerLogin)).Returns(Task.FromResult(true));

        await mock.PlayerService.UnmuteAsync(mock.Player.Object, mock.Actor.Object);
        
        mock.Audit.Verify(m => m.Success(), Times.Once);
        mock.Server.Remote.Verify(m => m.UnIgnoreAsync(PlayerLogin), Times.Once);
        mock.Server.Chat.Verify(m => m.InfoMessageAsync(It.IsAny<string>(), mock.Player.Object), Times.Once);
        mock.Server.Chat.Verify(m => m.SuccessMessageAsync(It.IsAny<string>(), mock.Actor.Object), Times.Once);
    }
    
    [Fact]
    public async Task Player_Is_UnMuting_Failed_Sends_Error_Message()
    {
        var mock = NewPlayerServiceMock();

        await mock.PlayerService.MuteAsync(mock.Player.Object, mock.Actor.Object);
        
        mock.ContextService.Verify(m => m.Audit(), Times.Never);
        mock.Server.Chat.Verify(m => m.ErrorMessageAsync(It.IsAny<string>(), mock.Actor.Object), Times.Once);
    }

    [Fact]
    public async Task Player_Is_Banned_Blacklisted_And_Audited()
    {
        var mock = NewPlayerServiceMock();

        await mock.PlayerService.BanAsync(mock.Player.Object, mock.Actor.Object);
        
        mock.Server.Remote.Verify(m => m.BanAsync(PlayerLogin), Times.Once);
        mock.Server.Remote.Verify(m => m.BlackListAsync(PlayerLogin), Times.Once);
        mock.Audit.Verify(m => m.Success(), Times.Once);
        mock.Server.Chat.Verify(m => m.SuccessMessageAsync(It.IsAny<string>(), mock.Actor.Object), Times.Once);
    }
    
    [Fact]
    public async Task Player_Banning_Failed_Still_Blacklists_And_Logs_Trace()
    {
        var mock = NewPlayerServiceMock();
        var ex = new Exception();
        mock.Server.Remote.Setup(m => m.BanAsync(PlayerLogin)).Returns(() => throw ex);

        await mock.PlayerService.BanAsync(mock.Player.Object, mock.Actor.Object);
        
        mock.Server.Remote.Verify(m => m.BanAsync(PlayerLogin), Times.Once);
        mock.Logger.Verify(LogLevel.Trace, ex, null, Times.Once());
        mock.Server.Remote.Verify(m => m.BlackListAsync(PlayerLogin), Times.Once);
        mock.Audit.Verify(m => m.Success(), Times.Once);
        mock.Server.Chat.Verify(m => m.SuccessMessageAsync(It.IsAny<string>(), mock.Actor.Object), Times.Once);
    }
    
    [Fact]
    public async Task Player_Is_Unbanned_And_Audited()
    {
        var mock = NewPlayerServiceMock();
        mock.Server.Remote.Setup(m => m.UnBanAsync(PlayerLogin)).Returns(Task.FromResult(true));
        mock.Server.Remote.Setup(m => m.UnBlackListAsync(PlayerLogin)).Returns(Task.FromResult(true));

        await mock.PlayerService.UnbanAsync(PlayerLogin, mock.Actor.Object);
        
        mock.Server.Remote.Verify(m => m.UnBanAsync(PlayerLogin), Times.Once);
        mock.Server.Remote.Verify(m => m.UnBlackListAsync(PlayerLogin), Times.Once);
        mock.Server.Chat.Verify(m => m.SuccessMessageAsync(It.IsAny<string>(), mock.Actor.Object), Times.Exactly(2));
        mock.Audit.Verify(m => m.Success(), Times.Exactly(2));
    }
    
    [Fact]
    public async Task Player_Unban_Failed_Is_Logged_And_Still_Unblacklisted()
    {
        var mock = NewPlayerServiceMock();
        var ex = new Exception();
        mock.Server.Remote.Setup(m => m.UnBanAsync(PlayerLogin)).Returns(() => throw ex);
        mock.Server.Remote.Setup(m => m.UnBlackListAsync(PlayerLogin)).Returns(Task.FromResult(true));

        await mock.PlayerService.UnbanAsync(PlayerLogin, mock.Actor.Object);
        
        mock.Server.Remote.Verify(m => m.UnBanAsync(PlayerLogin), Times.Once);
        mock.Server.Remote.Verify(m => m.UnBlackListAsync(PlayerLogin), Times.Once);
        mock.Server.Chat.Verify(m => m.ErrorMessageAsync(It.IsAny<string>(), mock.Actor.Object), Times.Once);
        mock.Audit.Verify(m => m.Success(), Times.Once);
        mock.Logger.Verify(LogLevel.Error, ex, null, Times.Once());
    }
    
    [Fact]
    public async Task Player_Unblacklist_Failed_Sends_ErrorMsg_And_ErrorLog()
    {
        var mock = NewPlayerServiceMock();
        var ex = new Exception();
        mock.Server.Remote.Setup(m => m.UnBanAsync(PlayerLogin)).Returns(Task.FromResult(true));
        mock.Server.Remote.Setup(m => m.UnBlackListAsync(PlayerLogin)).Returns(() => throw ex);

        await mock.PlayerService.UnbanAsync(PlayerLogin, mock.Actor.Object);
        
        mock.Server.Remote.Verify(m => m.UnBanAsync(PlayerLogin), Times.Once);
        mock.Server.Remote.Verify(m => m.UnBlackListAsync(PlayerLogin), Times.Once);
        mock.Server.Chat.Verify(m => m.ErrorMessageAsync(It.IsAny<string>(), mock.Actor.Object), Times.Once);
        mock.Audit.Verify(m => m.Success(), Times.Once);
        mock.Logger.Verify(LogLevel.Error, ex, null, Times.Once());
    }

    [Fact]
    public async Task ForceSpectator_Calls_Correct_Remote_Method()
    {
        var mock = NewPlayerServiceMock();
        var player = new Mock<IPlayer>();
        player.Setup(m => m.AccountId).Returns("a467a996-eba5-44bf-9e2b-8543b50f39ae");

        await mock.PlayerService.ForceSpectatorAsync(player.Object);
        
        mock.Server.Remote.Verify(m => m.ForceSpectatorAsync("pGepluulRL-eK4VDtQ85rg", 3));
    }
}
