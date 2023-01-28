using System.Linq;
using System.Threading.Tasks;
using EvoSC.Common.Database.Models.Permissions;
using EvoSC.Common.Database.Repository.Permissions;
using EvoSC.Common.Database.Repository.Players;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Permissions.Models;
using EvoSC.Common.Tests.Database.Setup;
using GbxRemoteNet.Structs;
using LinqToDB;
using Xunit;

namespace EvoSC.Common.Tests.Database;

public class PermissionRepositoryTests
{
    private static (PermissionRepository, IDbConnectionFactory) CreateNewRepository()
    {
        var factory = TestDbSetup.CreateFullDb();
        return (new PermissionRepository(factory, LoggerSetup.CreateLogger<PermissionRepository>()), factory);
    }

    private static async Task<IPlayer> AddTestPlayer(IDbConnectionFactory factory, string accountId)
    {
        var playerRepo = new PlayerRepository(factory);
        
        return await playerRepo.AddPlayerAsync(accountId, new TmPlayerDetailedInfo
        {
            NickName = "TestAccount",
            Path = "World"
        });
    }

    private static Task<IPlayer> AddTestPlayer(IDbConnectionFactory factory) => AddTestPlayer(factory, "TestAccountId");
    
    [Fact]
    public async Task Permission_Added()
    {
        var (repo, dbFactory) = CreateNewRepository();

        await repo.AddPermissionAsync(new Permission {Name = "MyPermission", Description = "My permission description."});

        var perm = await dbFactory.GetConnection().GetTable<DbPermission>().FirstOrDefaultAsync();
        
        Assert.NotNull(perm);
        Assert.Equal("MyPermission", perm.Name);
        Assert.Equal("My permission description.", perm.Description);
    }

    [Fact]
    public async Task Permission_Updated()
    {
        var (repo, dbFactory) = CreateNewRepository();

        await repo.AddPermissionAsync(new Permission {Name = "MyPermission", Description = "My permission description."});
        var oldPerm = await dbFactory.GetConnection().GetTable<DbPermission>().FirstAsync();

        oldPerm.Name = "MyUpdatedPermission";
        await repo.UpdatePermissionAsync(oldPerm);
        
        var newPerm = await dbFactory.GetConnection().GetTable<DbPermission>().FirstAsync();
        
        Assert.Equal("MyUpdatedPermission", newPerm.Name);
    }

    [Fact]
    public async Task Permission_Retrieved_By_Name()
    {
        var (repo, dbFactory) = CreateNewRepository();

        await repo.AddPermissionAsync(new Permission {Name = "MyPermission", Description = "My permission description."});

        var perm = await repo.GetPermissionAsync("MyPermission");
        
        Assert.NotNull(perm);
        Assert.Equal("MyPermission", perm.Name);
        Assert.Equal("My permission description.", perm.Description);
    }

    [Fact]
    public async Task Get_All_Permissions_Of_A_Player()
    {
        var (repo, dbFactory) = CreateNewRepository();

        var player = await AddTestPlayer(dbFactory);
        var group = await repo.AddGroupAsync(new Group {Title = "My Group", Description = "My group description."});

        var permsToAdd = new IPermission[]
        {
            new Permission {Name = "MyPermission1", Description = "My permission 1 description."},
            new Permission {Name = "MyPermission2", Description = "My permission 2 description."},
            new Permission {Name = "MyPermission3", Description = "My permission 3 description."}
        };

        for (var i = 0; i < permsToAdd.Length; i++)
        {
            permsToAdd[i] = await repo.AddPermissionAsync(permsToAdd[i]);
            await repo.AddPermissionToGroupAsync(group.Id, permsToAdd[i].Id);
        }

        await repo.AddPlayerToGroupAsync(player.Id, group.Id);

        var permissions = await repo.GetPlayerPermissionsAsync(player.Id);

        var perm1 = permissions.FirstOrDefault(p => p.Id == permsToAdd.First().Id);
        Assert.NotNull(perm1);
        Assert.Equal("MyPermission1", perm1.Name);
        Assert.Equal("My permission 1 description.", perm1.Description);
        
        var perm2 = permissions.FirstOrDefault(p => p.Id == permsToAdd.Skip(1).First().Id);
        Assert.NotNull(perm2);
        Assert.Equal("MyPermission2", perm2.Name);
        Assert.Equal("My permission 2 description.", perm2.Description);
        
        var perm3 = permissions.FirstOrDefault(p => p.Id == permsToAdd.Skip(2).First().Id);
        Assert.NotNull(perm3);
        Assert.Equal("MyPermission3", perm3.Name);
        Assert.Equal("My permission 3 description.", perm3.Description);
    }

    [Fact]
    public async Task Permission_Properly_Removed()
    {
        var (repo, dbFactory) = CreateNewRepository();
        
        var group = await repo.AddGroupAsync(new Group {Title = "My Group", Description = "My group description."});
        var permission = await repo.AddPermissionAsync(new Permission
        {
            Name = "MyPermission", Description = "My permission description."
        });
        await repo.AddPermissionToGroupAsync(group.Id, permission.Id);

        await repo.RemovePermissionAsync(permission);

        var existingPerm = await dbFactory.GetConnection().GetTable<DbPermission>().FirstOrDefaultAsync();
        var existingRef = await dbFactory.GetConnection().GetTable<DbGroupPermission>().FirstOrDefaultAsync();
        
        Assert.Null(existingPerm);
        Assert.Null(existingRef);
    }
}
