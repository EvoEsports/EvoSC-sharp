using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Services;
using Moq;

namespace MatchReadyModule.Tests;

public class PlayerReadyTrackerServiceTests
{
    private Mock<IEventManager> _events = new();
    
    [Fact]
    public void Required_Players_Are_Set()
    {
        var service = NewService();
        var player1 = NewPlayer();
        var player2 = NewPlayer();
        var player3 = NewPlayer();

        service.AddRequiredPlayerAsync(player1.Object);
        service.AddRequiredPlayerAsync(player2.Object);
        service.AddRequiredPlayerAsync(player3.Object);

        Assert.Contains(player1.Object, service.RequiredPlayers);
        Assert.Contains(player2.Object, service.RequiredPlayers);
        Assert.Contains(player3.Object, service.RequiredPlayers);
    }

    [Fact]
    public void Player_Ready_Toggle_Simple()
    {
        var service = NewService();
        var player = NewPlayer();

        service.AddRequiredPlayerAsync(player.Object);
        service.SetIsReadyAsync(player.Object, true);

        Assert.Contains(player.Object, service.ReadyPlayers);

        service.SetIsReadyAsync(player.Object, false);
        
        Assert.DoesNotContain(player.Object, service.ReadyPlayers);
    }

    [Fact]
    public void Player_Ready_Toggle_Multiple_Required_Players()
    {
        var service = NewService();
        var player1 = NewPlayer();
        var player2 = NewPlayer();
        var player3 = NewPlayer();

        service.AddRequiredPlayerAsync(player1.Object);
        service.AddRequiredPlayerAsync(player2.Object);
        service.AddRequiredPlayerAsync(player3.Object);
        
        service.SetIsReadyAsync(player1.Object, true);

        Assert.Contains(player1.Object, service.ReadyPlayers);

        service.SetIsReadyAsync(player1.Object, false);
        
        Assert.DoesNotContain(player1.Object, service.ReadyPlayers);
    }

    [Fact]
    public void Multiple_Players_Ready_And_Unready()
    {
        var service = NewService();
        var player1 = NewPlayer();
        var player2 = NewPlayer();
        var player3 = NewPlayer();

        service.AddRequiredPlayerAsync(player1.Object);
        service.AddRequiredPlayerAsync(player2.Object);
        service.AddRequiredPlayerAsync(player3.Object);
        
        service.SetIsReadyAsync(player1.Object, true);
        service.SetIsReadyAsync(player2.Object, true);

        Assert.Contains(player1.Object, service.ReadyPlayers);
        Assert.Contains(player2.Object, service.ReadyPlayers);

        service.SetIsReadyAsync(player2.Object, false);
        
        Assert.Contains(player1.Object, service.ReadyPlayers);
        Assert.DoesNotContain(player2.Object, service.ReadyPlayers);
    }

    [Fact]
    public void Setting_Ready_Multiple_Times_Only_Keeps_One_Instance_Even()
    {
        var service = NewService();
        var player = NewPlayer();

        service.AddRequiredPlayerAsync(player.Object);

        service.SetIsReadyAsync(player.Object, true);
        service.SetIsReadyAsync(player.Object, true);

        Assert.Contains(player.Object, service.ReadyPlayers);
        Assert.Single(service.ReadyPlayers);
    }
    
    [Fact]
    public void Setting_Ready_Multiple_Times_Only_Keeps_One_Instance_Odd()
    {
        var service = NewService();
        var player = NewPlayer();

        service.AddRequiredPlayerAsync(player.Object);

        service.SetIsReadyAsync(player.Object, true);
        service.SetIsReadyAsync(player.Object, true);
        service.SetIsReadyAsync(player.Object, true);

        Assert.Contains(player.Object, service.ReadyPlayers);
        Assert.Single(service.ReadyPlayers);
    }

    [Fact]
    public async Task Multiple_Players_Toggling_ReadyState_Parallel()
    {
        var service = NewService();
        var player1 = NewPlayer();
        var player2 = NewPlayer();
        var player3 = NewPlayer();

        await service.AddRequiredPlayerAsync(player1.Object);
        await service.AddRequiredPlayerAsync(player2.Object);
        await service.AddRequiredPlayerAsync(player3.Object);

        var t1 = Task.Run(async () =>
        {
            await service.SetIsReadyAsync(player1.Object, true);
            await service.SetIsReadyAsync(player1.Object, false);
        });

        var t2 = Task.Run(async () =>
        {
            await service.SetIsReadyAsync(player2.Object, true);
            await service.SetIsReadyAsync(player2.Object, false);
            await service.SetIsReadyAsync(player2.Object, true);
        });
        
        
        var t3 = Task.Run(async () =>
        {
            await service.SetIsReadyAsync(player3.Object, true);
        });

        await t1;
        await t2;
        await t3;
        
        Assert.DoesNotContain(player1.Object, service.ReadyPlayers);
        Assert.Contains(player2.Object, service.ReadyPlayers);
        Assert.Contains(player3.Object, service.ReadyPlayers);
    }

    [Fact]
    public void Reset_Clears_Ready_Players()
    {
        var service = NewService();
        var player1 = NewPlayer();
        var player2 = NewPlayer();
        var player3 = NewPlayer();

        service.AddRequiredPlayerAsync(player1.Object);
        service.AddRequiredPlayerAsync(player2.Object);
        service.AddRequiredPlayerAsync(player3.Object);

        service.SetIsReadyAsync(player1.Object, true);
        service.SetIsReadyAsync(player2.Object, true);
        service.SetIsReadyAsync(player3.Object, true);

        service.Reset();
        
        Assert.DoesNotContain(player1.Object, service.ReadyPlayers);
        Assert.DoesNotContain(player2.Object, service.ReadyPlayers);
        Assert.DoesNotContain(player3.Object, service.ReadyPlayers);
    }

    private IPlayerReadyTrackerService NewService() => new PlayerReadyTrackerService(_events.Object);

    private Mock<IPlayer> NewPlayer()
    {
        var player = new Mock<IPlayer>();
        player.Setup(p => p.AccountId).Returns(Guid.NewGuid().ToString());
        player.Setup(p => p.Equals(It.IsAny<IPlayer>()))
            .Returns((IPlayer o) => player.Object.AccountId.Equals(o.AccountId));

        return player;
    }
}
