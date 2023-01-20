using System.Threading.Tasks;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Database.Repository.Players;
using GbxRemoteNet.Structs;
using LinqToDB;
using Xunit;

namespace EvoSC.Common.Tests.Database;

public class PlayerRepositoryTests
{
    [Fact]
    public async Task Player_Added_To_Database()
    {
        var dbFactory = new TestDbConnectionFactory();
        var db = dbFactory.GetConnection();
        db.CreateTable<DbPlayer>();
        
        var playerRepo = new PlayerRepository(dbFactory);

        await playerRepo.AddPlayerAsync("TestAccountId", new TmPlayerDetailedInfo
        {
            NickName = "TestAccount",
            Path = "World"
        });

        var player = await db.GetTable<DbPlayer>().SingleOrDefaultAsync();
        
        Assert.NotNull(player);
        Assert.Equal("testaccountid", player.AccountId);
        Assert.Equal("TestAccount", player.NickName);
        Assert.Equal("World", player.Zone);
    }
}
