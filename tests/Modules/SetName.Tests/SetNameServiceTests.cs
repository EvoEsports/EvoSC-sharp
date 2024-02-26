using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.SetName.Events;
using EvoSC.Modules.Official.SetName.Services;
using EvoSC.Testing;
using NSubstitute;

namespace EvoSC.Modules.Official.SetName.Tests;

public class SetNameServiceTests
{
    [Fact]
    public async Task Name_Is_Set_And_Updated_In_Caches()
    {
        var player = Substitute.For<IOnlinePlayer>();
        player.NickName.Returns("OldName");
        var mlAction = Substitute.For<IManialinkActionContext>();
        var mlManager = Substitute.For<IManialinkManager>();
        var context = Mocking.NewManialinkInteractionContextMock(player, mlAction, mlManager);
        var contextService = Mocking.NewContextServiceMock(context.Context, null);
        var server = Mocking.NewServerClientMock();
        var playerRepository = Substitute.For<IPlayerRepository>();
        var playerCache = Substitute.For<IPlayerCacheService>();
        var eventManager = Substitute.For<IEventManager>();
        var locale = Mocking.NewLocaleMock(contextService);

        var service = new SetNameService(server.Client, playerRepository, playerCache,
            eventManager, locale);

        await service.SetNicknameAsync(player, "NewName");

        await playerRepository.Received(1).UpdateNicknameAsync(player, "NewName");
        await playerCache.Received(1).UpdatePlayerAsync(player);
        await eventManager.Received(1).RaiseAsync(SetNameEvents.NicknameUpdated, Arg.Any<NicknameUpdatedEventArgs>());
    }

    [Fact]
    public async Task Name_Equals_Old_Name_Wont_Update()
    {
        var player = Substitute.For<IOnlinePlayer>();
        player.NickName.Returns("OldName");
        var mlAction = Substitute.For<IManialinkActionContext>();
        var mlManager = Substitute.For<IManialinkManager>();
        var context = Mocking.NewManialinkInteractionContextMock(player, mlAction, mlManager);
        var contextService = Mocking.NewContextServiceMock(context.Context, null);
        var playerRepository = Substitute.For<IPlayerRepository>();
        var locale = Mocking.NewLocaleMock(contextService);
        var server = Mocking.NewServerClientMock();

        var service = new SetNameService(server.Client, playerRepository, null, null, locale);

        await service.SetNicknameAsync(player, "OldName");
        await playerRepository.DidNotReceive().UpdateNicknameAsync(player, "New Name");
    }
}
