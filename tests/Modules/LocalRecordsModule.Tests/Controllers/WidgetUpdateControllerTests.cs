using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Models.Players;
using EvoSC.Modules.Official.LocalRecordsModule.Controllers;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Events;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using Moq;

namespace LocalRecordsModule.Tests.Controllers;

public class WidgetUpdateControllerTests : EventControllerTestBase<WidgetUpdateController>
{
    private readonly Mock<ILocalRecordsService> _localRecordsService = new();
    private readonly Mock<IPlayerManagerService> _playerManagerService = new();
    
    public WidgetUpdateControllerTests()
    {
        InitMock(_localRecordsService, _playerManagerService);
    }

    [Fact]
    public async Task Widget_Shown_For_Player_On_Join()
    {
        var player = new OnlinePlayer
        {
            Id = 1,
            AccountId = "a467a996-eba5-44bf-9e2b-8543b50f39ae",
            NickName = "snixtho",
            UbisoftName = "snixtho",
            State = PlayerState.Playing
        };
        
        _playerManagerService.Setup(m => m.GetOnlinePlayerAsync("a467a996-eba5-44bf-9e2b-8543b50f39ae"))
            .ReturnsAsync(player);
        
        await Controller.OnPlayerConnectAsync(null, new PlayerConnectGbxEventArgs { Login = "pGepluulRL-eK4VDtQ85rg" });
        
        _localRecordsService.Verify(m => m.ShowWidgetAsync(player));
    }

    [Fact]
    public async Task Widget_Is_Updated_When_New_Map_Starts()
    {
        await Controller.OnBeginMapAsync(null, new MapGbxEventArgs());
        
        _localRecordsService.Verify(m => m.ShowWidgetToAllAsync());
    }

    [Fact]
    public async Task Local_Records_Are_Updated_When_Player_Gets_Pb()
    {
        var record = new DbPlayerRecord();
        var player = new Player();
        var map = new Map();

        var args = new PbRecordUpdateEventArgs
        {
            Player = player, Map = map, Record = record, Status = RecordUpdateStatus.New
        };

        await Controller.OnPbAsync(null, args);
        
        _localRecordsService.Verify(m => m.UpdatePbAsync(record));
    }
}
