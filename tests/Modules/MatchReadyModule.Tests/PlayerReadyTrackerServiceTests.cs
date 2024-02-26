using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Services;
using NSubstitute;

namespace MatchReadyModule.Tests;

public class PlayerReadyTrackerServiceTests
{
    private readonly IEventManager _events = Substitute.For<IEventManager>();
    
    [Fact]
    public void Required_Players_Are_Set()
    {
        var service = NewService();
        var player1 = NewPlayer();
        var player2 = NewPlayer();
        var player3 = NewPlayer();

        service.AddRequiredPlayerAsync(player1);
        service.AddRequiredPlayerAsync(player2);
        service.AddRequiredPlayerAsync(player3);

        Assert.Contains(player1, service.RequiredPlayers);
        Assert.Contains(player2, service.RequiredPlayers);
        Assert.Contains(player3, service.RequiredPlayers);
    }

    [Fact]
    public void Player_Ready_Toggle_Simple()
    {
        var service = NewService();
        var player = NewPlayer();

        service.AddRequiredPlayerAsync(player);
        service.SetIsReadyAsync(player, true);

        Assert.Contains(player, service.ReadyPlayers);

        service.SetIsReadyAsync(player, false);
        
        Assert.DoesNotContain(player, service.ReadyPlayers);
    }

    [Fact]
    public void Player_Ready_Toggle_Multiple_Required_Players()
    {
        var service = NewService();
        var player1 = NewPlayer();
        var player2 = NewPlayer();
        var player3 = NewPlayer();

        service.AddRequiredPlayerAsync(player1);
        service.AddRequiredPlayerAsync(player2);
        service.AddRequiredPlayerAsync(player3);
        
        service.SetIsReadyAsync(player1, true);

        Assert.Contains(player1, service.ReadyPlayers);

        service.SetIsReadyAsync(player1, false);
        
        Assert.DoesNotContain(player1, service.ReadyPlayers);
    }

    [Fact]
    public void Multiple_Players_Ready_And_Unready()
    {
        var service = NewService();
        var player1 = NewPlayer();
        var player2 = NewPlayer();
        var player3 = NewPlayer();

        service.AddRequiredPlayerAsync(player1);
        service.AddRequiredPlayerAsync(player2);
        service.AddRequiredPlayerAsync(player3);
        
        service.SetIsReadyAsync(player1, true);
        service.SetIsReadyAsync(player2, true);

        Assert.Contains(player1, service.ReadyPlayers);
        Assert.Contains(player2, service.ReadyPlayers);

        service.SetIsReadyAsync(player2, false);
        
        Assert.Contains(player1, service.ReadyPlayers);
        Assert.DoesNotContain(player2, service.ReadyPlayers);
    }

    [Fact]
    public void Setting_Ready_Multiple_Times_Only_Keeps_One_Instance_Even()
    {
        var service = NewService();
        var player = NewPlayer();

        service.AddRequiredPlayerAsync(player);

        service.SetIsReadyAsync(player, true);
        service.SetIsReadyAsync(player, true);

        Assert.Contains(player, service.ReadyPlayers);
        Assert.Single(service.ReadyPlayers);
    }
    
    [Fact]
    public void Setting_Ready_Multiple_Times_Only_Keeps_One_Instance_Odd()
    {
        var service = NewService();
        var player = NewPlayer();

        service.AddRequiredPlayerAsync(player);

        service.SetIsReadyAsync(player, true);
        service.SetIsReadyAsync(player, true);
        service.SetIsReadyAsync(player, true);

        Assert.Contains(player, service.ReadyPlayers);
        Assert.Single(service.ReadyPlayers);
    }

    [Fact]
    public async Task Multiple_Players_Toggling_ReadyState_Parallel()
    {
        var service = NewService();
        var player1 = NewPlayer();
        var player2 = NewPlayer();
        var player3 = NewPlayer();

        await service.AddRequiredPlayerAsync(player1);
        await service.AddRequiredPlayerAsync(player2);
        await service.AddRequiredPlayerAsync(player3);

        var t1 = Task.Run(async () =>
        {
            await service.SetIsReadyAsync(player1, true);
            await service.SetIsReadyAsync(player1, false);
        });

        var t2 = Task.Run(async () =>
        {
            await service.SetIsReadyAsync(player2, true);
            await service.SetIsReadyAsync(player2, false);
            await service.SetIsReadyAsync(player2, true);
        });
        
        
        var t3 = Task.Run(async () =>
        {
            await service.SetIsReadyAsync(player3, true);
        });

        await t1;
        await t2;
        await t3;
        
        Assert.DoesNotContain(player1, service.ReadyPlayers);
        Assert.Contains(player2, service.ReadyPlayers);
        Assert.Contains(player3, service.ReadyPlayers);
    }

    [Fact]
    public void Reset_Clears_Ready_Players()
    {
        var service = NewService();
        var player1 = NewPlayer();
        var player2 = NewPlayer();
        var player3 = NewPlayer();

        service.AddRequiredPlayerAsync(player1);
        service.AddRequiredPlayerAsync(player2);
        service.AddRequiredPlayerAsync(player3);

        service.SetIsReadyAsync(player1, true);
        service.SetIsReadyAsync(player2, true);
        service.SetIsReadyAsync(player3, true);

        service.Reset();
        
        Assert.DoesNotContain(player1, service.ReadyPlayers);
        Assert.DoesNotContain(player2, service.ReadyPlayers);
        Assert.DoesNotContain(player3, service.ReadyPlayers);
    }

    private IPlayerReadyTrackerService NewService() => new PlayerReadyTrackerService(_events);

    private IPlayer NewPlayer()
    {
        var player = Substitute.For<IPlayer>();
        player.AccountId.Returns(Guid.NewGuid().ToString());
        player.Equals(Arg.Any<IPlayer>())
            .ReturnsForAnyArgs(p => player.AccountId.Equals(p.Arg<IPlayer>().AccountId));

        return player;
    }
}
