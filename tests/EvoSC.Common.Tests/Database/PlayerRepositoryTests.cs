using System.Threading.Tasks;
using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Database.Repository.Players;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Tests.Database.Setup;
using GbxRemoteNet.Structs;
using LinqToDB;
using Xunit;

namespace EvoSC.Common.Tests.Database;

public class PlayerRepositoryTests
{
    private readonly IDbConnectionFactory _dbFactory;
    
    public PlayerRepositoryTests()
    {
        _dbFactory = TestDbSetup.CreateFullDb("PlayerRepositoryTests");
    }

    [Fact]
    public async Task Player_Added_To_Database()
    {
        var db = _dbFactory.GetConnection();
        var playerRepo = new PlayerRepository(_dbFactory);

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
}
