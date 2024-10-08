using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Tests;
using EvoSC.Common.Themes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Config;
using EvoSC.Modules.Official.LocalRecordsModule.Database.Models;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Database;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;
using EvoSC.Modules.Official.LocalRecordsModule.Services;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Moq;

namespace LocalRecordsModule.Tests.Services;

using MockData = (
    ILocalRecordsService Service,
    Mock<IMapService> MapService,
    Mock<ILocalRecordRepository> LocalRecordRepository,
    Mock<IPlayerManagerService> PlayerManagerService,
    Mock<IManialinkManager> ManialinkManager,
    ILogger<LocalRecordsService> Logger,
    Mock<ILocalRecordsSettings> Settings,
    (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote, Mock<IChatService> Chat) Server,
    Mock<IThemeManager> ThemeManager,
    Mock<IPlayerRecordsRepository> PlayerRecordsRepository);

public class LocalRecordsServiceTests
{
    private MockData NewLocalRecordsServiceMock()
    {
        var mapService = new Mock<IMapService>();
        var localRecordRepository = new Mock<ILocalRecordRepository>();
        var playerManager = new Mock<IPlayerManagerService>();
        var manialinkManager = new Mock<IManialinkManager>();
        var logger = TestLoggerSetup.CreateLogger<LocalRecordsService>();
        var settings = new Mock<ILocalRecordsSettings>();
        var server = Mocking.NewServerClientMock();
        var themeManager = new Mock<IThemeManager>();
        var playerRecordsRepository = new Mock<IPlayerRecordsRepository>();

        themeManager.SetupGet(m => m.Theme)
            .Returns(new DynamicThemeOptions(new Dictionary<string, object> { { "Info", "FFF" } }));
        
        var localRecordsService = new LocalRecordsService(
            mapService.Object,
            localRecordRepository.Object,
            playerManager.Object,
            manialinkManager.Object,
            logger,
            settings.Object,
            server.Chat.Object,
            themeManager.Object,
            playerRecordsRepository.Object
        );

        return (
            localRecordsService,
            mapService,
            localRecordRepository,
            playerManager,
            manialinkManager,
            logger,
            settings,
            server,
            themeManager,
            playerRecordsRepository
        );
    }

    private (IMap Map, IPlayer Player) SetupMockRecords(MockData mock)
    {
        var currentMap = new Map { Id = 1234, Uid = "my map" };
        var player = new Player { Id = 3, AccountId = "myplayer", NickName = "my player" };

        DbPlayerRecord[] records =
        [
            new DbPlayerRecord
            {
                Id = 1,
                MapId = currentMap.Id,
                Score = 1337,
                RecordType = PlayerRecordType.Time,
                DbMap = new DbMap(currentMap),
                DbPlayer = new DbPlayer{ Id = 2, AccountId = "myplayer2", NickName = "my player2"}
            },
            new DbPlayerRecord
            {
                Id = 2,
                MapId = currentMap.Id,
                Score = 1337,
                RecordType = PlayerRecordType.Time,
                DbMap = new DbMap(currentMap),
                DbPlayer = new DbPlayer{ Id = 3, AccountId = "myplayer3", NickName = "my player3"}
            },
            new DbPlayerRecord
            {
                Id = 3,
                MapId = currentMap.Id,
                Score = 1337,
                RecordType = PlayerRecordType.Time,
                DbMap = new DbMap(currentMap),
                DbPlayer =  new DbPlayer(player)
            },
            new DbPlayerRecord
            {
                Id = 4,
                MapId = currentMap.Id,
                Score = 1337,
                RecordType = PlayerRecordType.Time,
                DbMap = new DbMap(currentMap),
                DbPlayer = new DbPlayer{ Id = 4, AccountId = "myplayer4", NickName = "my player4"}
            },
            new DbPlayerRecord
            {
                Id = 5,
                MapId = currentMap.Id,
                Score = 1337,
                RecordType = PlayerRecordType.Time,
                DbMap = new DbMap(currentMap),
                DbPlayer = new DbPlayer{ Id = 5, AccountId = "myplayer5", NickName = "my player5"}
            },
        ];

        mock.MapService.Setup(m => m.GetCurrentMapAsync()).ReturnsAsync(currentMap);
        mock.LocalRecordRepository.Setup(m => m.GetLocalRecordsOfMapByIdAsync(currentMap.Id))
            .ReturnsAsync(records.Select(r => new DbLocalRecord
            {
                Id = 0,
                MapId = (long)r.MapId!,
                RecordId = r.Id,
                Position = (int)r.Id,
                DbMap = r.DbMap,
                DbRecord = r
            }));

        return (currentMap, player);
    }

    [Fact]
    public async Task GetLocalsOfCurrentMap_Returns_Local_Records_Of_CurrentMap()
    {
        var mock = NewLocalRecordsServiceMock();
        SetupMockRecords(mock);
        var records = await mock.Service.GetLocalsOfCurrentMapAsync();

        Assert.Equal(5, records.Length);
        Assert.All(records, r => Assert.Equal("my map", r.Map.Uid));
    }

    [Fact]
    public async Task Current_Map_Not_Found_Throws_Error()
    {
        var mock = NewLocalRecordsServiceMock();

        mock.MapService.Setup(m => m.GetCurrentMapAsync()).ReturnsAsync((IMap?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => mock.Service.GetLocalsOfCurrentMapAsync());
    }

    [Fact]
    public async Task Widget_Is_Shown_To_Player()
    {
        var mock = NewLocalRecordsServiceMock();
        var mockSetup = SetupMockRecords(mock);

        await mock.Service.ShowWidgetAsync(mockSetup.Player);

        mock.ManialinkManager.Verify(m =>
            m.SendManialinkAsync(mockSetup.Player, "LocalRecordsModule.LocalRecordsWidget", It.IsAny<object>()));
    }

    [Fact]
    public async Task Widget_Is_Shown_To_All()
    {
        var mock = NewLocalRecordsServiceMock();
        var mockSetup = SetupMockRecords(mock);
        var player1 = new OnlinePlayer { Id = 1, AccountId = "player1", State = PlayerState.Playing };
        var player2 = new OnlinePlayer { Id = 2, AccountId = "player2", State = PlayerState.Playing };
        var player3 = new OnlinePlayer { Id = 3, AccountId = "player3", State = PlayerState.Playing };

        mock.PlayerManagerService
            .Setup(m => m.GetOnlinePlayersAsync())
            .ReturnsAsync([player1, player2, player3]);

        var transaction = new Mock<IManialinkTransaction>();
        mock.ManialinkManager.Setup(m => m.CreateTransaction()).Returns(transaction.Object);

        await mock.Service.ShowWidgetToAllAsync();

        transaction.Verify(m => m.SendManialinkAsync(player1, "LocalRecordsModule.LocalRecordsWidget", It.IsAny<object>()), Times.Once);
        transaction.Verify(m => m.SendManialinkAsync(player2, "LocalRecordsModule.LocalRecordsWidget", It.IsAny<object>()), Times.Once);
        transaction.Verify(m => m.SendManialinkAsync(player3, "LocalRecordsModule.LocalRecordsWidget", It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task New_Pb_Is_Not_Good_Enough()
    {
        var mock = NewLocalRecordsServiceMock();
        var newPb = new DbPlayerRecord
        {
            Id = 0,
            PlayerId = 0,
            MapId = null,
            Score = 0,
            RecordType = PlayerRecordType.Time,
            Checkpoints = null,
            CreatedAt = default,
            UpdatedAt = default,
            DbPlayer = null,
            DbMap = null
        };

        mock.LocalRecordRepository
            .Setup(m => m.GetRecordOfPlayerInMapAsync(newPb.Player, newPb.Map))
            .ReturnsAsync((DbLocalRecord?)null);

        mock.LocalRecordRepository
            .Setup(m => m.AddOrUpdateRecordAsync(newPb.Map, newPb))
            .ReturnsAsync((DbLocalRecord?)null);

        await mock.Service.UpdatePbAsync(newPb);
        
        mock.Server.Chat.Verify(m => m.InfoMessageAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task New_Pb_Is_New_Local_Record()
    {
        var mock = NewLocalRecordsServiceMock();
        var mockSetup = SetupMockRecords(mock);
        var newPb = new DbPlayerRecord
        {
            PlayerId = 1, Score = 12345, RecordType = PlayerRecordType.Time, DbPlayer = new DbPlayer(mockSetup.Player)
        };
        var localRecord = new DbLocalRecord { DbRecord = newPb, Position = 1337 };

        mock.LocalRecordRepository
            .Setup(m => m.GetRecordOfPlayerInMapAsync(newPb.Player, newPb.Map))
            .ReturnsAsync((DbLocalRecord?)null);

        mock.LocalRecordRepository
            .Setup(m => m.AddOrUpdateRecordAsync(newPb.Map, newPb))
            .ReturnsAsync(localRecord);

        await mock.Service.UpdatePbAsync(newPb);

        mock.Server.Chat.Verify(m => m.InfoMessageAsync(It.Is<string>(s => s.Contains("gained the"))), Times.Once);
    }
    
    [Fact]
    public async Task New_Pb_Is_Improved_From_Old_Record_With_Position_Change()
    {
        var mock = NewLocalRecordsServiceMock();
        var mockSetup = SetupMockRecords(mock);
        var newPb = new DbPlayerRecord
        {
            PlayerId = 1, Score = 12345, RecordType = PlayerRecordType.Time, DbPlayer = new DbPlayer(mockSetup.Player),
        };
        var oldPlayerRecord = new DbPlayerRecord()
        {
            Score = 123456, RecordType = PlayerRecordType.Time, DbPlayer = new DbPlayer(mockSetup.Player)
        };
        var oldRecord = new DbLocalRecord { DbRecord = oldPlayerRecord, Position = 1337 };
        var localRecord = new DbLocalRecord { DbRecord = newPb, Position = 420 };

        mock.LocalRecordRepository
            .Setup(m => m.GetRecordOfPlayerInMapAsync(newPb.Player, newPb.Map))
            .ReturnsAsync((DbLocalRecord?)oldRecord);

        mock.LocalRecordRepository
            .Setup(m => m.AddOrUpdateRecordAsync(newPb.Map, newPb))
            .ReturnsAsync(localRecord);

        await mock.Service.UpdatePbAsync(newPb);

        mock.Server.Chat.Verify(m => m.InfoMessageAsync(It.Is<string>(s => s.Contains("claimed"))), Times.Once);
    }
    
    [Fact]
    public async Task New_Pb_Is_Improved_From_Old_Record_Without_Position_Change()
    {
        var mock = NewLocalRecordsServiceMock();
        var mockSetup = SetupMockRecords(mock);
        var newPb = new DbPlayerRecord
        {
            PlayerId = 1, Score = 12345, RecordType = PlayerRecordType.Time, DbPlayer = new DbPlayer(mockSetup.Player),
        };
        var oldPlayerRecord = new DbPlayerRecord()
        {
            Score = 123456, RecordType = PlayerRecordType.Time, DbPlayer = new DbPlayer(mockSetup.Player)
        };
        var oldRecord = new DbLocalRecord { DbRecord = oldPlayerRecord, Position = 1337};
        var localRecord = new DbLocalRecord { DbRecord = newPb, Position = 1337 };

        mock.LocalRecordRepository
            .Setup(m => m.GetRecordOfPlayerInMapAsync(newPb.Player, newPb.Map))
            .ReturnsAsync((DbLocalRecord?)oldRecord);

        mock.LocalRecordRepository
            .Setup(m => m.AddOrUpdateRecordAsync(newPb.Map, newPb))
            .ReturnsAsync(localRecord);

        await mock.Service.UpdatePbAsync(newPb);

        mock.Server.Chat.Verify(m => m.InfoMessageAsync(It.Is<string>(s => s.Contains("improved their"))), Times.Once);
    }
    
    [Fact]
    public async Task New_Pb_Is_Equaled_To_Old_Record()
    {
        var mock = NewLocalRecordsServiceMock();
        var mockSetup = SetupMockRecords(mock);
        var newPb = new DbPlayerRecord
        {
            PlayerId = 1, Score = 12345, RecordType = PlayerRecordType.Time, DbPlayer = new DbPlayer(mockSetup.Player),
        };
        var localRecord = new DbLocalRecord { DbRecord = newPb, Position = 1337 };

        mock.LocalRecordRepository
            .Setup(m => m.GetRecordOfPlayerInMapAsync(newPb.Player, newPb.Map))
            .ReturnsAsync((DbLocalRecord?)localRecord);

        mock.LocalRecordRepository
            .Setup(m => m.AddOrUpdateRecordAsync(newPb.Map, newPb))
            .ReturnsAsync(localRecord);

        await mock.Service.UpdatePbAsync(newPb);

        mock.Server.Chat.Verify(m => m.InfoMessageAsync(It.Is<string>(s => s.Contains("equaled their"))), Times.Once);
    }
}
