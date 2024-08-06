using System;
using System.Threading.Tasks;
using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Repository.Maps;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;
using EvoSC.Testing;
using EvoSC.Testing.Database;
using LinqToDB;
using Xunit;

namespace EvoSC.Common.Tests.Database;

public class MapRepositoryTests
{
    private static (MapRepository, IDbConnectionFactory) CreateNewRepository()
    {
        var factory = TestDbSetup.CreateDb(typeof(AddPlayersTable).Assembly);
        return (new MapRepository(factory, TestLoggerSetup.CreateLogger<MapRepository>()), factory);
    }

    private static async Task<IMap> AddTestMap(IDbConnectionFactory dbFactory)
    {
        var player = await DbTestHelper.AddTestPlayer(dbFactory);
        var mapRepo = new MapRepository(dbFactory, TestLoggerSetup.CreateLogger<MapRepository>());
        return await mapRepo.AddMapAsync(
            new MapMetadata
            {
                MapUid = "TestMapUid",
                MapName = "MyMap",
                AuthorId = "MyAuthorId",
                AuthorName = "MyAuthor",
                ExternalId = "MyExternalMapId",
                ExternalVersion = new DateTime(1, 2, 3, 4, 5, 6, 7),
                ExternalMapProvider = MapProviders.ManiaExchange
            }, player, "/Test/Map/Path.Map.Gbx");
    }

    [Fact]
    public async Task Map_Added_To_Database()
    {
        var (repo, dbFactory) = CreateNewRepository();
        var addedMap = await AddTestMap(dbFactory);

        var map = await dbFactory.GetConnection().GetTable<DbMap>().FirstOrDefaultAsync(r => r.Id == addedMap.Id);
        
        Assert.NotNull(map);
        Assert.Equal("TestMapUid", map.Uid);
        Assert.Equal("MyMap", map.Name);
        Assert.Equal("MyExternalMapId", map.ExternalId);
        Assert.Equal(new DateTime(1, 2, 3, 4, 5, 6, 7), map.ExternalVersion);
        Assert.Equal(MapProviders.ManiaExchange, map.ExternalMapProvider);
    }
    
    [Fact]
    public async Task Map_Parameters_Updated()
    {
        var (repo, dbFactory) = CreateNewRepository();
        var addedMap = await AddTestMap(dbFactory);

        await repo.UpdateMapAsync(addedMap.Id,
            new MapMetadata
            {
                MapUid = "TestMapUid1",
                MapName = "MyMap1",
                AuthorId = "",
                AuthorName = "",
                ExternalId = "MyExternalMapId1",
                ExternalVersion = new DateTime(2, 2, 3, 4, 5, 6, 7),
                ExternalMapProvider = MapProviders.TrackmaniaIo
            });
        
        var updatedMap = await dbFactory.GetConnection().GetTable<DbMap>().FirstOrDefaultAsync(r => r.Id == addedMap.Id);
        
        Assert.NotNull(updatedMap);
        Assert.Equal("TestMapUid1", updatedMap.Uid);
        Assert.Equal("MyMap1", updatedMap.Name);
        Assert.Equal("MyExternalMapId1", updatedMap.ExternalId);
        Assert.Equal(new DateTime(2, 2, 3, 4, 5, 6, 7), updatedMap.ExternalVersion);
        Assert.Equal(MapProviders.TrackmaniaIo, updatedMap.ExternalMapProvider);
    }

    [Fact]
    public async Task Map_Removed_From_Database()
    {
        var (repo, dbFactory) = CreateNewRepository();
        var addedMap = await AddTestMap(dbFactory);

        await repo.RemoveMapAsync(addedMap.Id);
        
        var map = await dbFactory.GetConnection().GetTable<DbMap>().FirstOrDefaultAsync(r => r.Id == addedMap.Id);
        
        Assert.Null(map);
    }

    [Fact]
    public async Task Map_Info_Fetched_By_Id()
    {
        var (repo, dbFactory) = CreateNewRepository();
        await AddTestMap(dbFactory);

        var map = await repo.GetMapByIdAsync(1);
        
        Assert.NotNull(map);
        Assert.Equal("TestMapUid", map.Uid);
        Assert.Equal("MyMap", map.Name);
        Assert.Equal("MyExternalMapId", map.ExternalId);
        Assert.Equal(new DateTime(1, 2, 3, 4, 5, 6, 7), map.ExternalVersion);
        Assert.Equal(MapProviders.ManiaExchange, map.ExternalMapProvider);
    }
    
    [Fact]
    public async Task Map_Info_Fetched_By_Uid()
    {
        var (repo, dbFactory) = CreateNewRepository();
        await AddTestMap(dbFactory);

        var map = await repo.GetMapByUidAsync("TestMapUid");
        
        Assert.NotNull(map);
        Assert.Equal("TestMapUid", map.Uid);
        Assert.Equal("MyMap", map.Name);
        Assert.Equal("MyExternalMapId", map.ExternalId);
        Assert.Equal(new DateTime(1, 2, 3, 4, 5, 6, 7), map.ExternalVersion);
        Assert.Equal(MapProviders.ManiaExchange, map.ExternalMapProvider);
    }
}
