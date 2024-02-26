using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util.Auditing;
using EvoSC.Modules.Official.Player.Interfaces;
using EvoSC.Modules.Official.Player.Services;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace EvoSC.Modules.Official.Player.Tests;

public class PlayerServiceTests
{
    private const string PlayerNickName = "snixtho";
    private const string PlayerAccountId = "a467a996-eba5-44bf-9e2b-8543b50f39ae";
    private const string PlayerLogin = "pGepluulRL-eK4VDtQ85rg";
    
    private readonly ILogger<PlayerService> _logger = Substitute.For<ILogger<PlayerService>>();
    private readonly IOnlinePlayer _actor = Substitute.For<IOnlinePlayer>();
    private readonly ControllerContextMock<IEventControllerContext> _eventContext;
    private readonly ControllerContextMock<ICommandInteractionContext> _commandContext;

    public PlayerServiceTests()
    {
        _eventContext = Mocking.NewControllerContextMock<IEventControllerContext>();
        _commandContext = Mocking.NewCommandInteractionContextMock(_actor);

        _actor.NickName.Returns(PlayerNickName);
        _actor.AccountId.Returns(PlayerAccountId);
    }
    
    /// <summary>
    /// Sets up mocks for the player service.
    /// </summary>
    /// <returns></returns>
    private (
        IPlayerService PlayerService,
        IContextService ContextService,
        Locale Locale,
        IPlayerManagerService PlayerManager,
        ILogger<PlayerService> Logger,
        (IServerClient Client, IGbxRemoteClient Remote) Server,
        IOnlinePlayer Player,
        IOnlinePlayer Actor,
        IAuditEventBuilder Audit
        )
        NewPlayerServiceMock()
    {
        var contextService = Mocking.NewContextServiceMock(_commandContext.Context, null);
        var locale = Mocking.NewLocaleMock(contextService);
        var playerManager = Substitute.For<IPlayerManagerService>();

        var server = Mocking.NewServerClientMock();

        var playerService = new PlayerService(playerManager, server.Client, _logger,
            contextService, locale);

        var player = Substitute.For<IOnlinePlayer>();
        player.AccountId.Returns(PlayerAccountId);
        player.NickName.Returns(PlayerNickName);

        return (
            PlayerService: playerService,
            ContextService: contextService,
            Locale: locale,
            PlayerManager: playerManager,
            Logger: _logger,
            Server: server,
            Player: player,
            Actor: _actor,
            Audit: _commandContext.AuditEventBuilder
        );
    }

    [Fact]
    public async Task Player_Created_And_Greeted_On_First_Join()
    {
        var contextService = Mocking.NewContextServiceMock(_eventContext.Context, null);
        var locale = Mocking.NewLocaleMock(contextService);
        
        var playerManager = Substitute.For<IPlayerManagerService>();
        playerManager.GetPlayerAsync(PlayerAccountId).Returns(Task.FromResult((IPlayer?)null));
        playerManager.CreatePlayerAsync(PlayerAccountId).Returns(Task.FromResult((IPlayer)_actor));

        var server = Mocking.NewServerClientMock();

        var playerService = new PlayerService(playerManager, server.Client, _logger, contextService, locale);
        
        await playerService.UpdateAndGreetPlayerAsync(PlayerLogin);

        await playerManager.Received(1).GetPlayerAsync(PlayerAccountId);
        await playerManager.Received(1).CreatePlayerAsync(PlayerAccountId);
        await playerManager.Received(1).UpdateLastVisitAsync(_actor);
        await server.Client.Received(1).InfoMessageAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task Player_Is_Greeted_When_Already_Exists()
    {
        var contextService = Mocking.NewContextServiceMock(_eventContext.Context, null);
        var locale = Mocking.NewLocaleMock(contextService);

        var playerManager = Substitute.For<IPlayerManagerService>();
        playerManager.GetPlayerAsync(PlayerAccountId)!.Returns(Task.FromResult((IPlayer)_actor));

        var server = Mocking.NewServerClientMock();
        
        var playerService = new PlayerService(playerManager, server.Client, _logger, contextService, locale);

        await playerService.UpdateAndGreetPlayerAsync(PlayerLogin);

        await playerManager.Received(1).GetPlayerAsync(PlayerAccountId);
        await playerManager.Received(1).UpdateLastVisitAsync(_actor);
        await server.Client.Received(1).InfoMessageAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task Player_Is_Kicked_And_Audited()
    {
        var mock = NewPlayerServiceMock();
        mock.Server.Remote.KickAsync(PlayerLogin, "").Returns(Task.FromResult(true));

        await mock.PlayerService.KickAsync(mock.Player, mock.Actor);
        
        
        mock.Audit.Received(1).Success();
        await mock.Server.Remote.Received(1).KickAsync(PlayerLogin, "");
        await mock.Server.Client.Received(1).SuccessMessageAsync(Arg.Any<string>(), mock.Actor);
    }

    [Fact]
    public async Task Player_Kicking_Failed_Sends_Error_Message()
    {
        var mock = NewPlayerServiceMock();

        await mock.PlayerService.KickAsync(mock.Player, mock.Actor);

        mock.ContextService.DidNotReceive().Audit();
        await mock.Server.Client.Received(1).ErrorMessageAsync(Arg.Any<string>(), mock.Actor);
    }

    [Fact]
    public async Task Player_Is_Muted_And_Audited()
    {
        var mock = NewPlayerServiceMock();
        mock.Server.Remote.IgnoreAsync(PlayerLogin).Returns(Task.FromResult(true));

        await mock.PlayerService.MuteAsync(mock.Player, mock.Actor);

        mock.Audit.Received(1).Success();
        await mock.Server.Remote.Received(1).IgnoreAsync(PlayerLogin);
        await mock.Server.Client.Received(1).WarningMessageAsync(Arg.Any<string>(), mock.Player);
        await mock.Server.Client.Received(1).SuccessMessageAsync(Arg.Any<string>(), mock.Actor);
    }
    
    [Fact]
    public async Task Player_Is_Muting_Failed_Sends_Error_Message()
    {
        var mock = NewPlayerServiceMock();

        await mock.PlayerService.MuteAsync(mock.Player, mock.Actor);
        
        mock.ContextService.DidNotReceive().Audit();
        await mock.Server.Client.Received(1).ErrorMessageAsync(Arg.Any<string>(), mock.Actor);
    }
    
    [Fact]
    public async Task Player_Is_UnMuted_And_Audited()
    {
        var mock = NewPlayerServiceMock();
        mock.Server.Remote.UnIgnoreAsync(PlayerLogin).Returns(Task.FromResult(true));

        await mock.PlayerService.UnmuteAsync(mock.Player, mock.Actor);
        
        mock.Audit.Received(1).Success();
        await mock.Server.Remote.Received(1).UnIgnoreAsync(PlayerLogin);
        await mock.Server.Client.Received(1).InfoMessageAsync(Arg.Any<string>(), mock.Player);
        await mock.Server.Client.Received(1).SuccessMessageAsync(Arg.Any<string>(), mock.Actor);
    }
    
    [Fact]
    public async Task Player_Is_UnMuting_Failed_Sends_Error_Message()
    {
        var mock = NewPlayerServiceMock();

        await mock.PlayerService.UnmuteAsync(mock.Player, mock.Actor);
        
        mock.ContextService.DidNotReceive().Audit();
        await mock.Server.Client.Received(1).ErrorMessageAsync(Arg.Any<string>(), mock.Actor);
    }

    [Fact]
    public async Task Player_Is_Banned_Blacklisted_And_Audited()
    {
        var mock = NewPlayerServiceMock();

        await mock.PlayerService.BanAsync(mock.Player, mock.Actor);
        
        mock.Audit.Received(1).Success();
        await mock.Server.Remote.Received(1).BanAsync(PlayerLogin);
        await mock.Server.Remote.Received(1).BlackListAsync(PlayerLogin);
        await mock.Server.Client.Received(1).SuccessMessageAsync(Arg.Any<string>(), mock.Actor);
    }
    
    [Fact]
    public async Task Player_Banning_Failed_Still_Blacklists_And_Logs_Trace()
    {
        var mock = NewPlayerServiceMock();
        var ex = new Exception();
        mock.Server.Remote.BanAsync(PlayerLogin).ThrowsAsync(ex);

        await mock.PlayerService.BanAsync(mock.Player, mock.Actor);


        await mock.Server.Remote.Received(1).BanAsync(PlayerLogin);
        mock.Logger.Verify(LogLevel.Trace, null,  null, Quantity.Exactly(1));
        await mock.Server.Remote.Received(1).BlackListAsync(PlayerLogin);
        await mock.Server.Client.Received(1).SuccessMessageAsync(Arg.Any<string>(), mock.Actor);
        mock.Audit.Received(1).Success();
    }
    
    [Fact]
    public async Task Player_Is_Unbanned_And_Audited()
    {
        var mock = NewPlayerServiceMock();
        mock.Server.Remote.UnBanAsync(PlayerLogin).Returns(Task.FromResult(true));
        mock.Server.Remote.UnBlackListAsync(PlayerLogin).Returns(Task.FromResult(true));

        await mock.PlayerService.UnbanAsync(PlayerLogin, mock.Actor);
        
        await mock.Server.Remote.Received(1).UnBanAsync(PlayerLogin);
        await mock.Server.Remote.Received(1).UnBlackListAsync(PlayerLogin);
        await mock.Server.Client.Received(2).SuccessMessageAsync(Arg.Any<string>(), _actor);
        mock.Audit.Received(2).Success();
    }
    
    [Fact]
    public async Task Player_Unban_Failed_Is_Logged_And_Still_Unblacklisted()
    {
        var mock = NewPlayerServiceMock();
        var ex = new Exception();
        mock.Server.Remote.UnBanAsync(PlayerLogin).ThrowsAsync(ex);
        mock.Server.Remote.UnBlackListAsync(PlayerLogin).Returns(Task.FromResult(true));

        await mock.PlayerService.UnbanAsync(PlayerLogin, mock.Actor);
        
        await mock.Server.Remote.Received(1).UnBanAsync(PlayerLogin);
        await mock.Server.Remote.Received(1).UnBlackListAsync(PlayerLogin);
        await mock.Server.Client.Received(1).ErrorMessageAsync(Arg.Any<string>(), _actor);
        mock.Audit.Received(1).Success();
        mock.Logger.Verify(LogLevel.Error, ex, null, Quantity.Exactly(1));
    }
    
    [Fact]
    public async Task Player_Unblacklist_Failed_Sends_ErrorMsg_And_ErrorLog()
    {
        var mock = NewPlayerServiceMock();
        var ex = new Exception();
        mock.Server.Remote.UnBanAsync(PlayerLogin).Returns(Task.FromResult(true));
        mock.Server.Remote.UnBlackListAsync(PlayerLogin).ThrowsAsync(ex);

        await mock.PlayerService.UnbanAsync(PlayerLogin, mock.Actor);
        
        await mock.Server.Remote.Received(1).UnBanAsync(PlayerLogin);
        await mock.Server.Remote.Received(1).UnBlackListAsync(PlayerLogin);
        await mock.Server.Client.Received(1).ErrorMessageAsync(Arg.Any<string>(), _actor);
        mock.Audit.Received(1).Success();
        mock.Logger.Verify(LogLevel.Error, ex, null, Quantity.Exactly(1));
    }
}
