using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Tests.Database;
using EvoSC.Modules.Official.MotdModule.Database.Migrations;
using EvoSC.Modules.Official.MotdModule.Database.Repositories;

namespace MotdModule.Tests;

public class MotdRepositoryTests
{
    private readonly MotdRepository _motdRepo;
    
    public MotdRepositoryTests()
    {
        var factory = CreateFullDb(Guid.NewGuid().ToString());
        _motdRepo = new(factory);
    }
    
    private IDbConnectionFactory CreateFullDb(string identifier)
    {
        var factory = new TestDbConnectionFactory(identifier);
        factory.GetConnection();
        TestDbMigrations.MigrateFromAssembly(factory.ConnectionString, typeof(MotdMigration).Assembly);
        TestDbMigrations.MigrateFromAssembly(factory.ConnectionString, typeof(AddPlayersTable).Assembly);

        return factory;
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(10, false)]
    public async Task InsertOrUpdateEntryAsync_Entry_Added(int id, bool hidden)
    {
        var player = new Player
        {
            Id = id
        };
        await _motdRepo.InsertOrUpdateEntryAsync(player, hidden);

        var returnedPlayer = await _motdRepo.GetEntryAsync(player);
        Assert.NotNull(returnedPlayer);
        Assert.Equal(id, returnedPlayer.PlayerId);
        Assert.Equal(hidden, returnedPlayer.Hidden);
    }
    
    [Fact]
    public async Task InsertOrUpdateEntryAsync_Entry_Updated()
    {
        var player = new Player
        {
            Id = 1
        };
        await _motdRepo.InsertOrUpdateEntryAsync(player, true);

        var returnedPlayer = await _motdRepo.GetEntryAsync(player);
        Assert.NotNull(returnedPlayer);
        Assert.Equal(1, returnedPlayer.PlayerId);
        Assert.True(returnedPlayer.Hidden);

        await _motdRepo.InsertOrUpdateEntryAsync(player, false);
        
        returnedPlayer = await _motdRepo.GetEntryAsync(player);
        Assert.NotNull(returnedPlayer);
        Assert.Equal(1, returnedPlayer.PlayerId);
        Assert.False(returnedPlayer.Hidden);
    }
}
