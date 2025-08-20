using EvoSC.Common.Interfaces;
using EvoSC.Common.Models.Players;
using EvoSC.Modules.Official.MatchReadyModule.Events;
using EvoSC.Modules.Official.MatchReadyModule.Events.Args;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Services;
using Moq;

namespace MatchReadyModule.Tests;

public class ReadyServiceTests
{
    private (
        IReadyService ReadyService,
        Mock<IEventManager> EventManagerMock,
        Mock<IReadyTrackerService> TrackerServiceMock
        ) NewServiceMock()
    {
        var eventManagerMock = new Mock<IEventManager>();
        var readyTrackerMock = new Mock<IReadyTrackerService>();
        
        var service = new ReadyService(readyTrackerMock.Object, eventManagerMock.Object);
        
        return (service, eventManagerMock, readyTrackerMock);
    }

    [Fact]
    public void ReadyPlayers_Returns_Ready_Players()
    {
        var mock = NewServiceMock();
        var player = new Player { AccountId = "1" };

        mock.TrackerServiceMock.SetupGet(p => p.ReadyPlayers).Returns([player]);
        
        Assert.Contains(player, mock.ReadyService.ReadyPlayers);
    }
    
    [Fact]
    public void Players_Returns_Players()
    {
        var mock = NewServiceMock();
        var player = new Player { AccountId = "1" };

        mock.TrackerServiceMock.SetupGet(p => p.Players).Returns([player]);
        
        Assert.Contains(player, mock.ReadyService.Players);
    }
    
    [Fact]
    public void Enabled_Returns_Ready_Enabled()
    {
        var mock = NewServiceMock();
        var player = new Player { AccountId = "1" };

        mock.TrackerServiceMock.SetupGet(p => p.Enabled).Returns(true);
        
        Assert.True(mock.ReadyService.Enabled);
    }

    [Fact]
    public async Task Adding_Players_Adds_Players_To_Tracker_Service()
    {
        var mock = NewServiceMock();
        
        var player1 = new Player { AccountId = "1" };
        var player2 = new Player { AccountId = "2" };
        var player3 = new Player { AccountId = "3" };
        
        await mock.ReadyService.AddPlayersAsync(player1, player2, player3);
        
        mock.TrackerServiceMock.Verify(t => t.AddPlayerAsync(player1));
        mock.TrackerServiceMock.Verify(t => t.AddPlayerAsync(player2));
        mock.TrackerServiceMock.Verify(t => t.AddPlayerAsync(player3));
    }

    [Fact]
    public async Task Reset_Clears_Tracker_Service()
    {
        var mock = NewServiceMock();
        
        await mock.ReadyService.ResetAsync();
        
        mock.TrackerServiceMock.Verify(t => t.ClearAsync());
    }

    [Fact]
    public async Task Enable_Sets_Enabled_To_True_In_Tracker_And_Raises_Enabled_Event()
    {
        var mock = NewServiceMock();
        
        await mock.ReadyService.EnableAsync();
        
        mock.TrackerServiceMock.Verify(m => m.EnableAsync());
        mock.EventManagerMock.Verify(m => m.RaiseAsync(MatchReadyEvents.Enabled, EventArgs.Empty));
    }
    
    [Fact]
    public async Task Disable_Sets_Enabled_To_False_In_Tracker_And_Raises_Disabled_Event()
    {
        var mock = NewServiceMock();
        
        await mock.ReadyService.DisableAsync();
        
        mock.TrackerServiceMock.Verify(m => m.DisableAsync());
        mock.EventManagerMock.Verify(m => m.RaiseAsync(MatchReadyEvents.Disabled, EventArgs.Empty));
    }

    [Fact]
    public async Task Set_Player_Status_To_Ready_Adds_Player_As_Ready_And_Raises_Event()
    {
        var mock = NewServiceMock();
        var player = new Player { AccountId = "1" };
        mock.TrackerServiceMock.SetupGet(p => p.AllReady).Returns(false);

        await mock.ReadyService.SetPlayerReadyStatusAsync(player, true);
        
        mock.TrackerServiceMock.Verify(m => m.AddReadyAsync(player));
        mock.TrackerServiceMock.Verify(m => m.RemoveReadyAsync(player), Times.Never);
        mock.EventManagerMock.Verify(m => m.RaiseAsync(
            MatchReadyEvents.PlayerReadyChanged, 
            It.Is<PlayerReadyEventArgs>(args => 
                args.Player == player && 
                args.IsReady == true
            )
        ));
        mock.EventManagerMock.Verify(m => m.RaiseAsync(MatchReadyEvents.AllPlayersReady, EventArgs.Empty), Times.Never);
    }
    
    [Fact]
    public async Task Set_Player_Status_To_Not_Ready_Removes_Player_As_Ready_And_Raises_Event()
    {
        var mock = NewServiceMock();
        var player = new Player { AccountId = "1" };
        mock.TrackerServiceMock.SetupGet(p => p.AllReady).Returns(false);

        await mock.ReadyService.SetPlayerReadyStatusAsync(player, false);
        
        mock.TrackerServiceMock.Verify(m => m.RemoveReadyAsync(player));
        mock.TrackerServiceMock.Verify(m => m.AddReadyAsync(player), Times.Never);
        mock.EventManagerMock.Verify(m => m.RaiseAsync(
            MatchReadyEvents.PlayerReadyChanged, 
            It.Is<PlayerReadyEventArgs>(args => 
                args.Player == player && 
                args.IsReady == false
            )
        ));
        mock.EventManagerMock.Verify(m => m.RaiseAsync(MatchReadyEvents.AllPlayersReady, EventArgs.Empty), Times.Never);
    }
    
    [Fact]
    public async Task Adding_Last_Ready_Player_Raises_Both_PlayerReadyChanged_And_AllPlayersReady_Events()
    {
        var mock = NewServiceMock();
        var player = new Player { AccountId = "1" };
        mock.TrackerServiceMock.SetupGet(p => p.AllReady).Returns(true);

        await mock.ReadyService.SetPlayerReadyStatusAsync(player, true);
        
        mock.TrackerServiceMock.Verify(m => m.AddReadyAsync(player));
        mock.TrackerServiceMock.Verify(m => m.RemoveReadyAsync(player), Times.Never);
        mock.EventManagerMock.Verify(m => m.RaiseAsync(
            MatchReadyEvents.PlayerReadyChanged, 
            It.Is<PlayerReadyEventArgs>(args => 
                args.Player == player && 
                args.IsReady == true
            )
        ));
        mock.EventManagerMock.Verify(m => m.RaiseAsync(MatchReadyEvents.AllPlayersReady, EventArgs.Empty));
    }
}
