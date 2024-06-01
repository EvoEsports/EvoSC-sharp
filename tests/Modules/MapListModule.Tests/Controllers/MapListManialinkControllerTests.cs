using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Controllers;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Modules.Official.MapQueueModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace EvoSC.Modules.Official.MapListModule.Tests.Controllers;

public class MapListManialinkControllerTests : ManialinkControllerTestBase<MapListManialinkController>
{
    private Mock<IOnlinePlayer> _player = new();
    private Mock<IManialinkActionContext> _manialinkActionContext = new();
    private Mock<IMapService> _mapService = new();
    private Mock<IMapQueueService> _mapQueueService = new();
    private Mock<IMapListService> _mapListService = new();
    
    public MapListManialinkControllerTests()
    {
        InitMock(_player.Object, _manialinkActionContext.Object, _mapService, _mapQueueService, _mapListService);
    }

    [Fact]
    public async Task Map_Queued_By_Uid()
    {
        var map = new Map { Uid = "map" };
        _mapService.Setup(m => m.GetMapByUidAsync(map.Uid)).ReturnsAsync(map);

        await Controller.QueueMapAsync(map.Uid);
        
        _mapQueueService.Verify(m => m.EnqueueAsync(map));
    }

    [Fact]
    public async Task Map_Dropped_By_Uid()
    {
        var map = new Map { Uid = "map" };
        _mapService.Setup(m => m.GetMapByUidAsync(map.Uid)).ReturnsAsync(map);

        await Controller.DropMapAsync(map.Uid);
        
        _mapQueueService.Verify(m => m.DropAsync(map));
    }

    [Fact]
    public async Task Map_Deletion_Will_Be_Confirmed_And_Confirmation_Audited_Async()
    {
        var map = new Map { Uid = "map" };
        _mapService.Setup(m => m.GetMapByUidAsync(map.Uid)).ReturnsAsync(map);

        await Controller.DeleteMapAsync(map.Uid);
        
        _mapListService.Verify(m => m.ConfirmMapDeletionsAsync(_player.Object, map));
        AuditEventBuilder.Verify(m => m.Success());
    }

    [Fact]
    public async Task Map_Deleted_On_Confirmation_Audited()
    {
        var map = new Map { Uid = "map" };

        await Controller.ConfirmDeleteAsync(map.Uid, true);
        
        _mapListService.Verify(m => m.DeleteMapAsync(_player.Object, map.Uid));
        AuditEventBuilder.Verify(m => m.Success());
        _mapListService.Verify(m => m.ShowMapListAsync(_player.Object));
    }
    
    [Fact]
    public async Task Map_Not_Deleted_If_Not_Confirmed()
    {
        var map = new Map { Uid = "map" };

        await Controller.ConfirmDeleteAsync(map.Uid, false);
        
        _mapListService.Verify(m => m.DeleteMapAsync(_player.Object, map.Uid), Times.Never);
        AuditEventBuilder.Verify(m => m.Cancel());
        _mapListService.Verify(m => m.ShowMapListAsync(_player.Object), Times.Never);
    }
}
