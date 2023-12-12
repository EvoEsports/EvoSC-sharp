using System.Threading.Tasks;
using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Database.Models.Config;
using EvoSC.Common.Database.Repository.Stores;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Testing.Database;
using LinqToDB;
using Xunit;

namespace EvoSC.Common.Tests.Database;

public class ConfigStoreRepositoryTests
{
    private static (ConfigStoreRepository, IDbConnectionFactory) CreateNewRepository()
    {
        var factory = TestDbSetup.CreateDb(typeof(AddPlayersTable).Assembly);
        return (new ConfigStoreRepository(factory), factory);
    }
    
    [Fact]
    public async Task Config_Option_Added()
    {
        var (repo, dbFactory) = CreateNewRepository();

        await repo.AddConfigOptionAsync(new DbConfigOption {Key = "MyKey", Value = "MyValue"});

        var option = await dbFactory.GetConnection().GetTable<DbConfigOption>().FirstOrDefaultAsync(t => t.Key == "MyKey");
        
        Assert.NotNull(option);
        Assert.Equal("MyKey", option.Key);
        Assert.Equal("MyValue", option.Value);
    }
    
    [Fact]
    public async Task Config_Option_Updated()
    {
        var (repo, dbFactory) = CreateNewRepository();

        await repo.AddConfigOptionAsync(new DbConfigOption {Key = "MyKey", Value = "MyValue"});
        await repo.UpdateConfigOptionAsync(new DbConfigOption {Key = "MyKey", Value = "MyValueUpdated"});

        var option = await dbFactory.GetConnection().GetTable<DbConfigOption>().FirstOrDefaultAsync(t => t.Key == "MyKey");
        
        Assert.NotNull(option);
        Assert.Equal("MyKey", option.Key);
        Assert.Equal("MyValueUpdated", option.Value);
    }

    [Fact]
    public async Task Config_Option_Fetched_From_Db_By_Key()
    {
        var (repo, dbFactory) = CreateNewRepository();

        await repo.AddConfigOptionAsync(new DbConfigOption {Key = "MyKey", Value = "MyValue"});

        var option = await repo.GetConfigOptionsByKeyAsync("MyKey");
        
        Assert.NotNull(option);
        Assert.Equal("MyKey", option.Key);
        Assert.Equal("MyValue", option.Value);
    }
}
