using EvoSC.Common.Models.Players;
using EvoSC.Modules.Official.MatchReadyModule.Exceptions;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Services;
using EvoSC.Testing;

namespace MatchReadyModule.Tests;

public class ReadyTrackerServiceTests
{
    private IReadyTrackerService NewServiceMock()
    {
        var trackerService = new ReadyTrackerService();
        return trackerService;
    }

    [Fact]
    public async Task Player_Is_Added()
    {
        var service = NewServiceMock();
        var player = new Player { AccountId = "MyAccountId" };
        
        await service.AddPlayerAsync(player);
        
        Assert.Contains(player, service.Players);
    }

    [Fact]
    public async Task Player_Is_Removed()
    {
        var service = NewServiceMock();
        var player = new Player { AccountId = "MyAccountId" };
        
        await service.AddPlayerAsync(player);
        await service.RemovePlayerAsync(player);
        
        Assert.DoesNotContain(player, service.Players);
    }

    [Fact]
    public async Task Multiple_Players_Are_Added()
    {
        var service = NewServiceMock();
        
        var player1 = new Player { AccountId = "MyAccountId1" };
        var player2 = new Player { AccountId = "MyAccountId2" };
        var player3 = new Player { AccountId = "MyAccountId3" };
        
        await service.AddPlayerAsync(player1);
        await service.AddPlayerAsync(player2);
        await service.AddPlayerAsync(player3);
        
        Assert.Contains(player1, service.Players);
        Assert.Contains(player2, service.Players);
        Assert.Contains(player3, service.Players);
    }

    [Fact]
    public async Task Multiple_Players_Are_Removed()
    {
        var service = NewServiceMock();
        var player1 = new Player { AccountId = "MyAccountId1" };
        var player2 = new Player { AccountId = "MyAccountId2" };
        var player3 = new Player { AccountId = "MyAccountId3" };
        
        await service.AddPlayerAsync(player1);
        await service.AddPlayerAsync(player2);
        await service.AddPlayerAsync(player3);
        await service.RemovePlayerAsync(player1);
        await service.RemovePlayerAsync(player2);
        await service.RemovePlayerAsync(player3);
        
        Assert.DoesNotContain(player1, service.Players);
        Assert.DoesNotContain(player2, service.Players);
        Assert.DoesNotContain(player3, service.Players);
    }

    [Fact]
    public async Task Same_Player_Is_Added_Once()
    {
        var service = NewServiceMock();
        
        var player1 = new Player { AccountId = "MyAccountId1" };
        var player2 = new Player { AccountId = "MyAccountId1" };
        
        await service.AddPlayerAsync(player1);
        await service.AddPlayerAsync(player2);
        await service.AddPlayerAsync(player2);
        
        Assert.Equal(2, service.Players.Count());
        Assert.Contains(player1, service.Players);
        Assert.Contains(player2, service.Players);
    }

    [Fact]
    public async Task Correct_Player_Is_Removed()
    {
        var service = NewServiceMock();
        var player1 = new Player { AccountId = "MyAccountId1" };
        var player2 = new Player { AccountId = "MyAccountId2" };
        
        await service.AddPlayerAsync(player1);
        await service.AddPlayerAsync(player2);
        await service.RemovePlayerAsync(player1);
        
        Assert.DoesNotContain(player1, service.Players);
        Assert.Contains(player2, service.Players);
    }

    [Fact]
    public async Task All_Players_Removed_After_Clear()
    {
        var service = NewServiceMock();
        
        var player1 = new Player { AccountId = "MyAccountId1" };
        var player2 = new Player { AccountId = "MyAccountId2" };
        
        await service.AddPlayerAsync(player1);
        await service.AddPlayerAsync(player2);
        
        await service.ClearAsync();
        
        Assert.DoesNotContain(player1, service.Players);
        Assert.DoesNotContain(player2, service.Players);
        Assert.Empty(service.Players);
    }

    [Fact]
    public async Task Enable_Sets_Correctly()
    {
        var service = NewServiceMock();
        
        await service.EnableAsync();
        
        Assert.True(service.Enabled);
    }
    
    [Fact]
    public async Task Disable_Sets_Correctly()
    {
        var service = NewServiceMock();
        
        await service.EnableAsync();
        await service.DisableAsync();
        
        Assert.False(service.Enabled);
    }

    [Fact]
    public async Task Player_Set_Ready_If_Exists_In_Players()
    {
        var service = NewServiceMock();
        var player1 = new Player { AccountId = "MyAccountId1" };
        var player2 = new Player { AccountId = "MyAccountId2" };
        
        await service.AddPlayerAsync(player1);
        await service.AddReadyAsync(player1);
        
        Assert.Single(service.ReadyPlayers);
        Assert.Contains(player1, service.ReadyPlayers);
        Assert.DoesNotContain(player2, service.ReadyPlayers);
    }

    [Fact]
    public async Task Adding_Ready_Player_Throws_Exception_If_Not_In_Players()
    {
        var service = NewServiceMock();
        var player1 = new Player { AccountId = "MyAccountId1" };

        await Assert.ThrowsAsync<PlayerNotAddedMatchReadyException>(() => service.AddReadyAsync(player1));
    }

    [Fact]
    public async Task Removing_Ready_Player_Unsets_From_Ready_Players()
    {
        var service = NewServiceMock();
        
        var player1 = new Player { AccountId = "MyAccountId1" };
        var player2 = new Player { AccountId = "MyAccountId2" };
        
        await service.AddPlayerAsync(player1);
        await service.AddPlayerAsync(player2);
        await service.AddReadyAsync(player1);
        await service.AddReadyAsync(player2);
        await service.RemoveReadyAsync(player1);
        
        Assert.DoesNotContain(player1, service.ReadyPlayers);
        Assert.Contains(player2, service.ReadyPlayers);
    }

    [Fact]
    public async Task AllReady_Is_True_If_All_Players_Are_Ready()
    {
        var service = NewServiceMock();
        var player1 = new Player { AccountId = "MyAccountId1" };
        var player2 = new Player { AccountId = "MyAccountId2" };
        
        await service.AddPlayerAsync(player1);
        await service.AddPlayerAsync(player2);
        
        await service.AddReadyAsync(player1);
        await service.AddReadyAsync(player2);
        
        Assert.True(service.AllReady);
    }

    [Fact]
    public async Task AllReady_Is_False_If_One_Player_Is_Not_Ready()
    {
        var service = NewServiceMock();
        var player1 = new Player { AccountId = "MyAccountId1" };
        var player2 = new Player { AccountId = "MyAccountId2" };
        
        await service.AddPlayerAsync(player1);
        await service.AddPlayerAsync(player2);
        
        await service.AddReadyAsync(player1);
        await service.AddReadyAsync(player2);
        await service.RemoveReadyAsync(player1);
        
        Assert.False(service.AllReady);
    }

    [Fact]
    public async Task Adding_New_Player_After_All_Ready_Sets_AllReady_To_False()
    {
        var service = NewServiceMock();
        var player1 = new Player { AccountId = "MyAccountId1" };
        var player2 = new Player { AccountId = "MyAccountId2" };
        var player3 = new Player { AccountId = "MyAccountId3" };
        
        await service.AddPlayerAsync(player1);
        await service.AddPlayerAsync(player2);
        
        await service.AddReadyAsync(player1);
        await service.AddReadyAsync(player2);
        
        Assert.True(service.AllReady);
        
        await service.AddPlayerAsync(player3);
        
        Assert.False(service.AllReady);
    }

    [Fact]
    public async Task Removing_Player_Removes_From_ReadyPlayer_And_Players()
    {
        var service = NewServiceMock();
        var player1 = new Player { AccountId = "MyAccountId1" };
        var player2 = new Player { AccountId = "MyAccountId2" };
        
        await service.AddPlayerAsync(player1);
        await service.AddPlayerAsync(player2);
        
        await service.AddReadyAsync(player1);
        await service.AddReadyAsync(player2);
        
        await service.RemovePlayerAsync(player1);
        
        Assert.True(service.AllReady);
        Assert.DoesNotContain(player1, service.Players);
        Assert.DoesNotContain(player1, service.ReadyPlayers);
    }

    [Fact]
    public async Task Clear_Resets_Everything()
    {
        var service = NewServiceMock();
        var player1 = new Player { AccountId = "MyAccountId1" };
        var player2 = new Player { AccountId = "MyAccountId2" };
        
        await service.AddPlayerAsync(player1);
        await service.AddPlayerAsync(player2);
        await service.AddReadyAsync(player1);
        await service.AddReadyAsync(player2);
        
        await service.ClearAsync();
        
        Assert.Empty(service.Players);
        Assert.Empty(service.ReadyPlayers);
        Assert.False(service.AllReady);
        
    }
}
