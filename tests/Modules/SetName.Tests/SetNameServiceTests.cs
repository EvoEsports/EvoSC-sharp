using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.SetName.Events;
using EvoSC.Modules.Official.SetName.Services;
using EvoSC.Testing;
using Moq;

namespace EvoSC.Modules.Official.SetName.Tests;

public class SetNameServiceTests
{
    [Fact]
    public async Task Name_Is_Set_And_Updated_In_Caches()
    {
        var player = new Mock<IOnlinePlayer>();
        player.Setup(m => m.NickName).Returns("OldName");
        var mlAction = new Mock<IManialinkActionContext>();
        var mlManager = new Mock<IManialinkManager>();
        var server = Mocking.NewServerClientMock();
        var context = Mocking.NewManialinkInteractionContextMock(server.Client, player.Object, mlAction.Object, mlManager.Object);
        var contextService = Mocking.NewContextServiceMock(context.Context.Object, null);
        var playerRepository = new Mock<IPlayerRepository>();
        var playerCache = new Mock<IPlayerCacheService>();
        var eventManager = new Mock<IEventManager>();
        var locale = Mocking.NewLocaleMock(contextService.Object);

        var service = new SetNameService(server.Chat.Object, playerRepository.Object, playerCache.Object,
            eventManager.Object, locale);

        await service.SetNicknameAsync(player.Object, "NewName");

        playerRepository.Verify(m => m.UpdateNicknameAsync(player.Object, "NewName"), Times.Once);
        playerCache.Verify(m => m.UpdatePlayerAsync(player.Object));
        eventManager.Verify(m => m.RaiseAsync(SetNameEvents.NicknameUpdated, It.IsAny<NicknameUpdatedEventArgs>()));
    }

    [Fact]
    public async Task Name_Equals_Old_Name_Wont_Update()
    {
        var player = new Mock<IOnlinePlayer>();
        player.Setup(m => m.NickName).Returns("OldName");
        var mlAction = new Mock<IManialinkActionContext>();
        var mlManager = new Mock<IManialinkManager>();
        var server = Mocking.NewServerClientMock();
        var context = Mocking.NewManialinkInteractionContextMock(server.Client, player.Object, mlAction.Object, mlManager.Object);
        var contextService = Mocking.NewContextServiceMock(context.Context.Object, null);
        var playerRepository = new Mock<IPlayerRepository>();
        var locale = Mocking.NewLocaleMock(contextService.Object);
        
        var service = new SetNameService(server.Chat.Object, playerRepository.Object, null, null, locale);

        await service.SetNicknameAsync(player.Object, "OldName");
        
        playerRepository.Verify(m => m.UpdateNicknameAsync(player.Object, "NewName"), Times.Never);
    }
}
