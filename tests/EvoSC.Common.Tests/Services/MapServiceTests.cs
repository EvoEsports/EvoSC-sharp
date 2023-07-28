using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using GbxRemoteNet.Structs;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EvoSC.Common.Tests.Services;

public class MapServiceTests
{
    private Mock<IMapRepository> _mapRepository = new();
    private Mock<ILogger<MapService>> _logger = new();
    private Mock<IEvoScBaseConfig> _config = new();
    private Mock<IPlayerManagerService> _playerService = new();
    private (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote) _server = Mocking.NewServerClientMock();

    private MapService _mapService;

    public MapServiceTests()
    {
        _mapService = new MapService(_mapRepository.Object, _logger.Object, _config.Object, _playerService.Object,
            _server.Client.Object);
    }

    [Fact]
    public async Task Get_Map_By_Id_Returns_Map()
    {
        var map = new DbMap
        {
            AuthorId = 1,
            Enabled = true,
            Id = 123,
            ExternalId = "1337",
            Name = "Snippens dream",
            Uid = "Uid"
        };
        _mapRepository.Setup(m => m.GetMapByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult((IMap?)map));

        var retrievedMap = await _mapService.GetMapByIdAsync(123);

        Assert.NotNull(retrievedMap);
        Assert.Equal(map.Id, retrievedMap.Id);
    }

    [Fact]
    public async Task Get_Map_By_Id_Returns_Null_If_Map_Not_Exist()
    {
        _mapRepository.Setup(m => m.GetMapByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult((IMap?)null));

        var retrievedMap = await _mapService.GetMapByIdAsync(123);

        Assert.Null(retrievedMap);
    }

    [Fact]
    public async Task Get_Map_By_Uid_Returns_Map()
    {
        var map = new DbMap
        {
            AuthorId = 1,
            Enabled = true,
            Id = 123,
            ExternalId = "1337",
            Name = "Snippens dream",
            Uid = "Uid"
        };
        _mapRepository.Setup(m => m.GetMapByUidAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((IMap)map)!);

        var retrievedMap = await _mapService.GetMapByUidAsync("123");

        Assert.NotNull(retrievedMap);
        Assert.Equal(map.Uid, retrievedMap.Uid);
    }

    [Fact]
    public async Task Get_Map_By_Uid_Returns_Null_If_Map_Not_Exist()
    {
        _mapRepository.Setup(m => m.GetMapByUidAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((IMap?)null));

        var retrievedMap = await _mapService.GetMapByUidAsync("123");

        Assert.Null(retrievedMap);
    }

    [Fact]
    public async Task Add_Map_Returns_Map()
    {
        using var testStream = new MemoryStream("whatever"u8.ToArray());
        var mapMetadata = new MapMetadata
        {
            AuthorId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            AuthorName = "snippen",
            MapUid = "mapUid",
            MapName = "snippens track",
            ExternalId = "",
            ExternalVersion = new DateTime(),
            ExternalMapProvider = MapProviders.ManiaExchange
        };
        var mapStream = new MapStream(mapMetadata, testStream);
        var player = new DbPlayer
        {
            Id = 1,
            UbisoftName = "snippen",
            AccountId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            NickName = "snippen",
            CreatedAt = new DateTime(),
            UpdatedAt = new DateTime(),
            LastVisit = new DateTime()
        };
        var map = new DbMap
        {
            AuthorId = 1,
            Enabled = true,
            Id = 123,
            ExternalId = "1337",
            Name = "Snippens dream",
            Uid = "Uid",
            ExternalMapProvider = MapProviders.ManiaExchange,
            DbAuthor = player
        };

        _mapRepository.Setup(m => m.GetMapByUidAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((IMap?)null));
        _mapRepository.Setup(m => m.AddMapAsync(It.IsAny<MapMetadata>(), It.IsAny<IPlayer>(), It.IsAny<string>()))
            .Returns(Task.FromResult((IMap)map));
        _config.Setup(c => c.Path.Maps).Returns("maps");
        _playerService.Setup(p => p.GetPlayerAsync(It.IsAny<string>())).Returns(Task.FromResult((IPlayer)player));

        var createdMap = await _mapService.AddMapAsync(mapStream);

        _server.Remote.Verify(m => m.InsertMapAsync("EvoSC/" + mapMetadata.MapName + ".Map.Gbx"));

        Assert.Equal(MapProviders.ManiaExchange, createdMap.ExternalMapProvider);
        Assert.Equal(map.ExternalId, createdMap.ExternalId);
        Assert.Equal(map.Uid, createdMap.Uid);
        Assert.Equal(map.ExternalVersion, createdMap.ExternalVersion);
        Assert.Equal(map.Name, createdMap.Name);
        Assert.Equal(map.AuthorId, createdMap.Author.Id);
    }

    [Fact]
    public async Task Add_Map_New_Map_Version_Returns_Map()
    {
        using var testStream = new MemoryStream("whatever"u8.ToArray());
        var version = DateTime.Now;
        var mapMetadata = new MapMetadata
        {
            AuthorId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            AuthorName = "snippen",
            MapUid = "mapUid",
            MapName = "snippens track",
            ExternalId = "",
            ExternalVersion = version.AddMinutes(2),
            ExternalMapProvider = MapProviders.ManiaExchange
        };
        var mapStream = new MapStream(mapMetadata, testStream);
        var player = new DbPlayer
        {
            Id = 1,
            UbisoftName = "snippen",
            AccountId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            NickName = "snippen",
            CreatedAt = new DateTime(),
            UpdatedAt = new DateTime(),
            LastVisit = new DateTime()
        };
        var map = new DbMap
        {
            AuthorId = 1,
            Enabled = true,
            Id = 123,
            ExternalId = "1337",
            Name = "Snippens dream",
            Uid = "Uid",
            ExternalMapProvider = MapProviders.ManiaExchange,
            ExternalVersion = version,
            DbAuthor = player
        };

        _mapRepository.Setup(m => m.GetMapByUidAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((IMap?)map));
        _mapRepository.Setup(m => m.UpdateMapAsync(It.IsAny<long>(), It.IsAny<MapMetadata>()))
            .Returns(Task.FromResult((IMap)map));
        _config.Setup(c => c.Path.Maps).Returns("maps");
        _playerService.Setup(p => p.GetPlayerAsync(It.IsAny<string>())).Returns(Task.FromResult((IPlayer?)player));

        var updatedMap = await _mapService.AddMapAsync(mapStream);

        _server.Remote.Verify(m => m.InsertMapAsync("EvoSC" + mapMetadata.MapName + ".Map.Gbx"));

        Assert.NotEqual(mapMetadata.ExternalVersion, updatedMap.ExternalVersion);
        Assert.Equal(MapProviders.ManiaExchange, updatedMap.ExternalMapProvider);
        Assert.Equal(map.ExternalId, updatedMap.ExternalId);
        Assert.Equal(map.Uid, updatedMap.Uid);
        Assert.Equal(map.Name, updatedMap.Name);
        Assert.Equal(map.AuthorId, updatedMap.Author.Id);
    }

    [Fact]
    public async Task Add_Map_With_Same_Version_Throws_DuplicateMapException()
    {
        using var testStream = new MemoryStream("whatever"u8.ToArray());
        var version = new DateTime();
        var mapMetadata = new MapMetadata
        {
            AuthorId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            AuthorName = "snippen",
            MapUid = "mapUid",
            MapName = "snippens track",
            ExternalId = "",
            ExternalVersion = version,
            ExternalMapProvider = MapProviders.ManiaExchange
        };
        var mapStream = new MapStream(mapMetadata, testStream);
        var player = new DbPlayer
        {
            Id = 1,
            UbisoftName = "snippen",
            AccountId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            NickName = "snippen",
            CreatedAt = new DateTime(),
            UpdatedAt = new DateTime(),
            LastVisit = new DateTime()
        };
        var map = new DbMap
        {
            AuthorId = 1,
            Enabled = true,
            Id = 123,
            ExternalId = "1337",
            Name = "snippens dream",
            Uid = "Uid",
            ExternalMapProvider = MapProviders.ManiaExchange,
            ExternalVersion = version,
            DbAuthor = player
        };

        _mapRepository.Setup(m => m.GetMapByUidAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((IMap?)map));
        _mapRepository.Setup(m => m.UpdateMapAsync(It.IsAny<long>(), It.IsAny<MapMetadata>()))
            .Returns(Task.FromResult((IMap)map));

        await Assert.ThrowsAsync<DuplicateMapException>(() => _mapService.AddMapAsync(mapStream));
    }

    [Fact]
    public async Task Add_Maps_Return_Maps()
    {
        using var testStream = new MemoryStream("whatever"u8.ToArray());
        var mapMetadata = new MapMetadata
        {
            AuthorId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            AuthorName = "snippen",
            MapUid = "mapUid",
            MapName = "snippens track",
            ExternalId = "",
            ExternalVersion = new DateTime(),
            ExternalMapProvider = MapProviders.ManiaExchange
        };
        var mapMetadata2 = new MapMetadata
        {
            AuthorId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            AuthorName = "snippen",
            MapUid = "mapUid2",
            MapName = "snippens2nd track",
            ExternalId = "",
            ExternalVersion = new DateTime(),
            ExternalMapProvider = MapProviders.ManiaExchange
        };
        var mapStream = new MapStream(mapMetadata, testStream);
        var mapStream2 = new MapStream(mapMetadata2, testStream);
        var player = new DbPlayer
        {
            Id = 1,
            UbisoftName = "snippen",
            AccountId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            NickName = "snippen",
            CreatedAt = new DateTime(),
            UpdatedAt = new DateTime(),
            LastVisit = new DateTime()
        };
        var map = new DbMap
        {
            AuthorId = 1,
            Enabled = true,
            Id = 123,
            ExternalId = "1337",
            Name = "snippens dream",
            Uid = "Uid",
            ExternalMapProvider = MapProviders.ManiaExchange,
            DbAuthor = player
        };

        _mapRepository.Setup(m => m.GetMapByUidAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((IMap?)null));
        _mapRepository.Setup(m => m.AddMapAsync(It.IsAny<MapMetadata>(), It.IsAny<IPlayer>(), It.IsAny<string>()))
            .Returns(Task.FromResult((IMap)map));
        _config.Setup(c => c.Path.Maps).Returns("maps");
        _playerService.Setup(p => p.GetPlayerAsync(It.IsAny<string>())).Returns(Task.FromResult((IPlayer?)player));

        var createdMaps = await _mapService.AddMapsAsync(new List<MapStream> { mapStream, mapStream2 });

        _server.Remote.Verify(m => m.InsertMapAsync(It.IsAny<string>()), Times.Exactly(2));

        Assert.Equal(2, createdMaps.Count());
    }

    [Fact]
    public async Task Remove_Map_Removes_Map()
    {
        var map = new DbMap
        {
            AuthorId = 1,
            Enabled = true,
            Id = 123,
            ExternalId = "1337",
            Name = "Snippens dream",
            Uid = "Uid"
        };
        _mapRepository.Setup(m => m.GetMapByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult((IMap?)map));

        await _mapService.RemoveMapAsync(123);

        _mapRepository.Verify(m => m.RemoveMapAsync(123), Times.Once);
    }

    [Fact]
    public async Task Add_Current_Map_List_Adds_Maplist()
    {
        var tmMapInfo = new TmMapInfo
        {
            Name = "Snippens dream",
            Author = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            AuthorTime = 1337,
            BronzeTime = 1337,
            CopperPrice = 1337,
            Environnement = "Stadium",
            GoldTime = 1337,
            LapRace = false,
            NbCheckpoints = 10
        };
        var player = new DbPlayer
        {
            Id = 1,
            UbisoftName = "snippen",
            AccountId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            NickName = "snippen",
            CreatedAt = new DateTime(),
            UpdatedAt = new DateTime(),
            LastVisit = new DateTime()
        };
        
        _server.Remote.Setup(r => r.GetMapListAsync(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult(new [] { tmMapInfo }));
        _playerService.Setup(p => p.GetOrCreatePlayerAsync(It.IsAny<string>())).Returns(Task.FromResult((IPlayer)player));

        await _mapService.AddCurrentMapListAsync();

        _mapRepository.Verify(m => m.AddMapAsync(It.IsAny<MapMetadata>(), It.IsAny<IPlayer>(), It.IsAny<string>()),
            Times.Exactly(1));
    }

    [Fact]
    public async Task Get_Or_Add_Current_Map_Returns_Current_Map()
    {
        var tmMapInfo = new TmMapInfo
        {
            Name = "snippens track",
            Author = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            AuthorTime = 1337,
            BronzeTime = 1337,
            CopperPrice = 1337,
            Environnement = "Stadium",
            GoldTime = 1337,
            LapRace = false,
            NbCheckpoints = 10,
            UId = "MapUid"
        };
        var player = new DbPlayer
        {
            Id = 1,
            UbisoftName = "snippen",
            AccountId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            NickName = "snippen",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            LastVisit = DateTime.Now
        };
        var mapMetadata = new MapMetadata
        {
            AuthorId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95",
            AuthorName = "snippen",
            MapUid = "MapUid",
            MapName = "snippens track",
            ExternalId = "",
            ExternalVersion = DateTime.Now,
            ExternalMapProvider = MapProviders.ManiaExchange
        };
        var map = new DbMap
        {
            AuthorId = 1,
            Enabled = true,
            Id = 123,
            ExternalId = "1337",
            Name = "snippens track",
            Uid = "MapUid"
        };
        
        _server.Remote.Setup(r => r.GetCurrentMapInfoAsync())
            .Returns(Task.FromResult(tmMapInfo));
        _mapRepository.Setup(m => m.GetMapByUidAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((IMap?)null));
        _mapRepository.Setup(m => m.AddMapAsync(It.IsAny<MapMetadata>(), It.IsAny<IPlayer>(), It.IsAny<string>()))
            .Returns(Task.FromResult((IMap)map));
        _playerService.Setup(p => p.GetOrCreatePlayerAsync(It.IsAny<string>())).Returns(Task.FromResult((IPlayer)player));

        var retrievedMap = await _mapService.GetOrAddCurrentMapAsync();

        Assert.Equal(mapMetadata.MapUid, retrievedMap.Uid);
        Assert.Equal(mapMetadata.MapName, retrievedMap.Name);
    }
}
