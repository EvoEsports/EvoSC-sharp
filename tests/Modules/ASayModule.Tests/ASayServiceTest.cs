using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util.Auditing;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ASayModule.Interfaces;
using EvoSC.Modules.Official.ASayModule.Services;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace EvoSC.Modules.Official.ASayModule.Tests;

public class ASayServiceTest
{
    private const string PlayerNickName = "Evo.Chris92";
    private const string PlayerAccountId = "17868d60-b494-4b88-81df-f4ddfdae1cf1";
    private const string PlayerLogin = "F4aNYLSUS4iB3_Td_a4c8Q";

    private readonly IOnlinePlayer _actor = Substitute.For<IOnlinePlayer>();
    private readonly IManialinkManager _manialinkManager = Substitute.For<IManialinkManager>();
    private readonly ControllerContextMock<ICommandInteractionContext> _commandContext;

    public ASayServiceTest()
    {
        _commandContext = Mocking.NewCommandInteractionContextMock(_actor);

        _actor.NickName.Returns(PlayerNickName);
        _actor.AccountId.Returns(PlayerAccountId);
    }

    private (
        IASayService ASayService,
        IContextService ContextService,
        IPlayerManagerService PlayerManager,
        (IServerClient Client, IGbxRemoteClient Remote) Server,
        IOnlinePlayer Player,
        IOnlinePlayer Actor,
        IAuditEventBuilder Audit
        )
        NewASayServiceMock()
    {
        var contextService = Mocking.NewContextServiceMock(_commandContext.Context, null);
        var playerManager = Substitute.For<IPlayerManagerService>();

        var server = Mocking.NewServerClientMock();

        var aSayService = new ASayService(_manialinkManager, contextService);

        var player = Substitute.For<IOnlinePlayer>();
        player.AccountId.Returns(PlayerAccountId);
        player.NickName.Returns(PlayerNickName);

        return (
            ASayService: aSayService,
            ContextService: contextService,
            PlayerManager: playerManager,
            Server: server,
            Player: player,
            Actor: _actor,
            Audit: _commandContext.AuditEventBuilder
        );
    }
    
    [Fact]
    private async void Should_Show_Announcement_Message()
    {
        var mock = NewASayServiceMock();
        
        var text = "example message";
        await mock.ASayService.ShowAnnouncementAsync(text);
        mock.Audit.Received(1).Success();
        await _manialinkManager.Received().SendPersistentManialinkAsync("ASayModule.Announcement", Arg.Is<object>(o => text.Equals(o.GetType().GetProperty("text")!.GetValue(o))));
    }

    [Fact]
    private async void Should_Hide_Announcement_Message()
    {
        var mock = NewASayServiceMock();
        await mock.ASayService.HideAnnouncementAsync();
        mock.Audit.Received(1).Success();
        await _manialinkManager.Received().HideManialinkAsync("ASayModule.Announcement");
    }
}
