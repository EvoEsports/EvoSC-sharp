using System.Dynamic;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Players;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Services;
using Moq;

namespace MatchReadyModule.Tests;

public class ReadyManialinkServiceTests
{
    interface IManialinkData
    {
        public bool isReady { get; set; }
        public IEnumerable<IPlayer> requiredPlayers { get; set; }
        public IEnumerable<IPlayer> players { get; set; }
        public bool showButton { get; set; }
    }
    
    private (
        IReadyManialinkService ReadyManialinkService,
        Mock<IReadyService> ReadyServiceMock,
        Mock<IManialinkManager> ManialinkManagerMock,
        Mock<IPlayerManagerService> PlayerManagerServiceMock
        ) NewServiceMock()
    {
        var readyServiceMock = new Mock<IReadyService>();
        var manialinkManagerMock = new Mock<IManialinkManager>();
        var playerManagerServiceMock = new Mock<IPlayerManagerService>();
        
        var service = new ReadyManialinkService(readyServiceMock.Object, manialinkManagerMock.Object, playerManagerServiceMock.Object);
        
        return (service, readyServiceMock, manialinkManagerMock, playerManagerServiceMock);
    }

    [Fact]
    public async Task? SendWidget_Sends_Enables_Button_On_Required_Players()
    {
        var mock = NewServiceMock();
        var manialinkTransactionMock = new Mock<IManialinkTransaction>();
        var player1 = new OnlinePlayer { AccountId = "p1", State = PlayerState.Spectating };
        var player2 = new OnlinePlayer { AccountId = "p2", State = PlayerState.Spectating };
        var player3 = new OnlinePlayer { AccountId = "p3", State = PlayerState.Spectating };
        
        mock.ReadyServiceMock.SetupGet(m => m.Players).Returns([player1, player2]);
        mock.ReadyServiceMock.SetupGet(m => m.ReadyPlayers).Returns([player1]);
        mock.ManialinkManagerMock.Setup(m => m.CreateTransaction()).Returns(manialinkTransactionMock.Object);
        mock.PlayerManagerServiceMock.Setup(m => m.GetOnlinePlayersAsync()).ReturnsAsync([player1, player2, player3]);

        await mock.ReadyManialinkService.SendWidgetAsync();

        manialinkTransactionMock.Verify(m => m.SendManialinkAsync(player1, "MatchReadyModule.ReadyWidget",
            It.Is<object>(args =>
                args.GetType().GetProperty("isReady")!.GetValue(args) as bool? == true &&
                args.GetType().GetProperty("requiredPlayers")!.GetValue(args) as int? == 2 &&
                args.GetType().GetProperty("playersReady")!.GetValue(args) as int? == 1 &&
                args.GetType().GetProperty("showButton")!.GetValue(args) as bool? == true
            )));
        
        manialinkTransactionMock.Verify(m => m.SendManialinkAsync(player2, "MatchReadyModule.ReadyWidget",
            It.Is<object>(args =>
                args.GetType().GetProperty("isReady")!.GetValue(args) as bool? == false &&
                args.GetType().GetProperty("requiredPlayers")!.GetValue(args) as int? == 2 &&
                args.GetType().GetProperty("playersReady")!.GetValue(args) as int? == 1 &&
                args.GetType().GetProperty("showButton")!.GetValue(args) as bool? == true
            )));
        
        manialinkTransactionMock.Verify(m => m.SendManialinkAsync(player3, "MatchReadyModule.ReadyWidget",
            It.Is<object>(args =>
                args.GetType().GetProperty("isReady")!.GetValue(args) as bool? == false &&
                args.GetType().GetProperty("requiredPlayers")!.GetValue(args) as int? == 2 &&
                args.GetType().GetProperty("playersReady")!.GetValue(args) as int? == 1 &&
                args.GetType().GetProperty("showButton")!.GetValue(args) as bool? == false
            )));
    }
    
    
}
