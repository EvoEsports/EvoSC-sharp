using System;
using System.Threading.Tasks;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Database.Repository.Players;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Tests.Database.Setup;
using GbxRemoteNet.Structs;
using LinqToDB;
using Microsoft.Data.Sqlite;
using Xunit;

namespace EvoSC.Common.Tests.Database;

public class PlayerRepositoryTests
{
    [Fact]
    public async Task Player_Added_To_Database()
    {
        var factory = TestDbSetup.CreateFullDb();
        var db = factory.GetConnection();
        var playerRepo = new PlayerRepository(factory);

        await playerRepo.AddPlayerAsync("TestAccountId", new TmPlayerDetailedInfo
        {
            NickName = "TestAccount",
            Path = "World"
        });

        var player = await db.GetTable<DbPlayer>().FirstOrDefaultAsync(r => r.AccountId == "testaccountid");
        
        Assert.NotNull(player);
        Assert.Equal("testaccountid", player.AccountId);
        Assert.Equal("TestAccount", player.NickName);
        Assert.Equal("World", player.Zone);
    }

    [Fact]
    public void Player_With_Same_Account_ID_Fails()
    {
        var factory = TestDbSetup.CreateFullDb();
        var playerRepo = new PlayerRepository(factory);

        Assert.Throws<System.Data.SQLite.SQLiteException>(() =>
        {
            playerRepo.AddPlayerAsync("TestAccountId", new TmPlayerDetailedInfo
            {
                NickName = "TestAccount",
                Path = "World"
            }).GetAwaiter().GetResult();
            
            playerRepo.AddPlayerAsync("TestAccountId", new TmPlayerDetailedInfo
            {
                NickName = "TestAccount",
                Path = "World"
            }).GetAwaiter().GetResult();
        });
    }

    [Fact]
    public async Task Get_Player_By_Account_ID_Returns_Correct()
    {
        var factory = TestDbSetup.CreateFullDb();
        var playerRepo = new PlayerRepository(factory);

        await playerRepo.AddPlayerAsync("TestAccountId", new TmPlayerDetailedInfo
        {
            NickName = "TestAccount",
            Path = "World"
        });

        var player = await playerRepo.GetPlayerByAccountIdAsync("testaccountid");
        
        Assert.NotNull(player);
    }

    [Fact]
    public async Task Player_Last_Visit_Updated()
    {
        var factory = TestDbSetup.CreateFullDb();
        var db = factory.GetConnection();
        var playerRepo = new PlayerRepository(factory);

        var player = await playerRepo.AddPlayerAsync("TestAccountId", new TmPlayerDetailedInfo
        {
            NickName = "TestAccount",
            Path = "World"
        });
        Assert.Null(((DbPlayer) player).LastVisit);

        await Task.Delay(1000);
        await playerRepo.UpdateLastVisitAsync(player);
        
        var updatedPlayer = await db.GetTable<DbPlayer>().FirstAsync(r => r.Id == 2);

        Assert.NotNull(updatedPlayer.LastVisit);
    }
}
