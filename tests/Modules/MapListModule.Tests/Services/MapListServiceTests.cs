using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Tests;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Services;
using EvoSC.Modules.Official.MapsModule;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace EvoSC.Modules.Official.MapListModule.Tests.Services;

public class MapListServiceTests
{
    private (
        IMapListService MapListService,
        Mock<IMatchSettingsService> MatchSettings,
        Mock<IPlayerRecordsService> PlayerRecords,
        Mock<IMapService> MapService,
        Mock<IContextService> ContextService,
        ControllerContextMock<IControllerContext> ControllerContext,
        Mock<IOnlinePlayer> Player,
        ILogger<MapListService> Logger,
        (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote) Server,
        Mock<IManialinkManager> ManialinkManager,
        Mock<IPermissionManager> PermissionManager
        ) NewMapListServiceMock()
    {
        var matchSettingsService = new Mock<IMatchSettingsService>();
        var playerRecordsService = new Mock<IPlayerRecordsService>();
        var context = Mocking.NewGenericServiceMock();
        var mapService = new Mock<IMapService>();
        var logger = LoggerSetup.CreateLogger<MapListService>();
        var server = Mocking.NewServerClientMock();
        var manialinkManagerService = new Mock<IManialinkManager>();
        var permissionManagerService = new Mock<IPermissionManager>();

        var maplistService = new MapListService(
            matchSettingsService.Object,
            playerRecordsService.Object,
            context.ContextService.Object,
            mapService.Object,
            logger,
            server.Client.Object,
            manialinkManagerService.Object,
            permissionManagerService.Object
        );

        return (
            maplistService,
            matchSettingsService,
            playerRecordsService,
            mapService,
            context.ContextService,
            context.ControllerContext,
            context.Player,
            logger,
            server,
            manialinkManagerService,
            permissionManagerService
        );
    }

    [Fact]
    public async Task Current_Maps_For_Player_Is_Returned()
    {
        var mock = NewMapListServiceMock();
        IMap[] maps = [new Map{Uid = "map1"}, new Map{Uid = "map2"}, new Map{Uid = "map3"}];
        IPlayerRecord[] records = [new DbPlayerRecord(), new DbPlayerRecord(), new DbPlayerRecord()];

        mock.MatchSettings
            .Setup(m => m.GetCurrentMapListAsync())
            .ReturnsAsync(maps);

        mock.PlayerRecords
            .Setup(m => m.GetPlayerRecordAsync(mock.Player.Object, maps[0]))
            .ReturnsAsync(records[0]);

        mock.PlayerRecords
            .Setup(m => m.GetPlayerRecordAsync(mock.Player.Object, maps[1]))
            .ReturnsAsync((IPlayerRecord?)null);
        
        mock.PlayerRecords
            .Setup(m => m.GetPlayerRecordAsync(mock.Player.Object, maps[2]))
            .ReturnsAsync(records[2]);

        var playerMaps = (await mock.MapListService.GetCurrentMapsForPlayerAsync(mock.Player.Object)).ToArray();
        
        Assert.Equal(3, playerMaps.Length);
        var record1 = playerMaps[0].Records.FirstOrDefault();
        var record2 = playerMaps[1].Records.FirstOrDefault();
        var record3 = playerMaps[2].Records.FirstOrDefault();
        
        Assert.Equal(records[0], record1);
        Assert.Null(record2);
        Assert.Equal(records[2], record3);
    }

    [Fact]
    public async Task MapDeletion_Success_Deletes_Map_And_Succeds_Audit()
    {
        var mock = NewMapListServiceMock();
        var map = new Map { Uid = "map", Id = 1234};
        
        mock.MapService.Setup(m => m.GetMapByUidAsync("map")).ReturnsAsync(map);

        await mock.MapListService.DeleteMapAsync(mock.Player.Object, "map");

        mock.MapService.Verify(m => m.RemoveMapAsync(1234));
        mock.ControllerContext.AuditEventBuilder.Verify(m => m.Success());
    }
    
    [Fact]
    public async Task Map_Not_Found_Does_Not_Delete_Map_And_Errors_Audit()
    {
        var mock = NewMapListServiceMock();
        var map = new Map { Uid = "map", Id = 1234};
        
        mock.MapService.Setup(m => m.GetMapByUidAsync("map")).ReturnsAsync((IMap)null);

        await mock.MapListService.DeleteMapAsync(mock.Player.Object, "map");

        mock.MapService.Verify(m => m.RemoveMapAsync(1234), Times.Never);
        mock.ControllerContext.AuditEventBuilder.Verify(m => m.Error());
    }

    [Fact]
    public async Task Map_Deletion_Failing_Causes_Audit_Error()
    {
        var mock = NewMapListServiceMock();
        var map = new Map { Uid = "map", Id = 1234};
        
        mock.MapService.Setup(m => m.GetMapByUidAsync("map")).ReturnsAsync(map);
        mock.MapService.Setup(m => m.RemoveMapAsync(map.Id)).Throws<Exception>();

        await mock.MapListService.DeleteMapAsync(mock.Player.Object, "map");
        
        mock.ControllerContext.AuditEventBuilder.Verify(m => m.Error());
    }

    [Fact]
    public async Task ShowMapList_Is_Shown_To_Player()
    {
        var mock = NewMapListServiceMock();
        var map = new Map { Uid = "map" };

        mock.MatchSettings
            .Setup(m => m.GetCurrentMapListAsync())
            .ReturnsAsync([map]);

        mock.PlayerRecords
            .Setup(m => m.GetPlayerRecordAsync(mock.Player.Object, map))
            .ReturnsAsync((IPlayerRecord)null);

        mock.PermissionManager
            .Setup(m => m.HasPermissionAsync(mock.Player.Object, MapsPermissions.RemoveMap))
            .ReturnsAsync(true);

        await mock.MapListService.ShowMapListAsync(mock.Player.Object);

        mock.ManialinkManager.Verify(m => m.SendManialinkAsync(
            mock.Player.Object,
            "MapListModule.MapList",
            It.Is<object>(d =>
                ((IEnumerable<IMapListMap>)d.GetType().GetProperty("maps").GetValue(d, null)).First().Map == map
                && (bool)(d.GetType().GetProperty("canRemoveMaps").GetValue(d, null))
            )));
    }

    [Fact]
    public async Task Map_Delete_Confirmation_Dialog_Is_Shown_To_Player()
    {
        var mock = NewMapListServiceMock();
        var map = new Map { Uid = "map", Name = "mapname"};

        await mock.MapListService.ConfirmMapDeletionAsync(mock.Player.Object, map);

        mock.ManialinkManager.Verify(m => m.SendManialinkAsync(
            mock.Player.Object,
            "MapListModule.Dialogs.ConfirmDeleteDialog",
            It.Is<object>(d =>
                (string)d.GetType().GetProperty("mapName").GetValue(d, null) == map.Name
                && (string)d.GetType().GetProperty("mapUid").GetValue(d, null) == map.Uid
            )));
    }
}
